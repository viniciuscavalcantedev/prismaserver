using System;
using Bios.Communication.ConnectionManager;
using Bios.Communication;
using Bios.Core;
using System.Linq;
using System.Collections.Generic;
using Bios.Communication.Packets.Incoming;
using Bios.HabboHotel.Rooms;
using Bios.Database.Interfaces;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.Catalog;
using Bios.Communication.Interfaces;
using Bios.HabboHotel.Users.UserData;
using Bios.Communication.Packets.Outgoing.Sound;
using Bios.Communication.Packets.Outgoing.Rooms.Chat;
using Bios.Communication.Packets.Outgoing.Handshake;
using Bios.Communication.Packets.Outgoing.Navigator;
using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using Bios.Communication.Packets.Outgoing.Inventory.Achievements;
using Bios.Communication.Encryption.Crypto.Prng;
using Bios.HabboHotel.Users.Messenger.FriendBar;
using Bios.Communication.Packets.Outgoing.BuildersClub;
using Bios.HabboHotel.Moderation;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.Users.Messenger;
using Bios.HabboHotel.Permissions;
using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Communication.Packets.Outgoing;
using System.Data;

namespace Bios.HabboHotel.GameClients
{
    public class GameClient
    {
        private readonly int _id;
        private Habbo _habbo;
        public string MachineId;
        private bool _disconnected;
        public ARC4 RC4Client = null;
        private GamePacketParser _packetParser;
        private ConnectionInformation _connection;
        public int PingCount { get; set; }
        internal int CurrentRoomUserId;
        internal DateTime TimePingedReceived;

        public GameClient(int ClientId, ConnectionInformation pConnection)
        {
            _id = ClientId;
            _connection = pConnection;
            _packetParser = new GamePacketParser(this);

            PingCount = 0;
        }

        private void SwitchParserRequest()
        {
            _packetParser.SetConnection(_connection);
            _packetParser.OnNewPacket += Parser_OnNewPacket;
            byte[] data = (_connection.parser as InitialPacketParser).currentData;
            _connection.parser.Dispose();
            _connection.parser = _packetParser;
            _connection.parser.handlePacketData(data);
        }

        private void Parser_OnNewPacket(ClientPacket Message)
        {
            try
            {
                BiosEmuThiago.GetGame().GetPacketManager().TryExecutePacket(this, Message);
            }
            catch (Exception e)
            {

				ExceptionLogger.LogException(e);
			}
        }

        private void PolicyRequest()
        {
            _connection.SendData(BiosEmuThiago.GetDefaultEncoding().GetBytes("<?xml version=\"1.0\"?>\r\n" +
                   "<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n" +
                   "<cross-domain-policy>\r\n" +
                   "<allow-access-from domain=\"*\" to-ports=\"1-31111\" />\r\n" +
                   "</cross-domain-policy>\x0"));
        }


        public void StartConnection()
        {
            if (_connection == null)
                return;

            PingCount = 0;

            (_connection.parser as InitialPacketParser).PolicyRequest += PolicyRequest;
            (_connection.parser as InitialPacketParser).SwitchParserRequest += SwitchParserRequest;
            _connection.startPacketProcessing();
        }

        public bool TryAuthenticate(string AuthTicket)
        {
            try
            {
				UserData userData = UserDataFactory.GetUserData(AuthTicket, out byte errorCode);
				if (errorCode == 1 || errorCode == 2)
                {
                    Disconnect();
                    return false;
                }

                #region Ban Checking
                //Let's have a quick search for a ban before we successfully authenticate..
                ModerationBan BanRecord = null;
                if (!string.IsNullOrEmpty(MachineId))
                {
                    if (BiosEmuThiago.GetGame().GetModerationManager().IsBanned(MachineId, out BanRecord))
                    {
                        if (BiosEmuThiago.GetGame().GetModerationManager().MachineBanCheck(MachineId))
                        {
                            Disconnect();
                            return false;
                        }
                    }
                }

                if (userData.user != null)
                {
                    //Now let us check for a username ban record..
                    BanRecord = null;
                    if (BiosEmuThiago.GetGame().GetModerationManager().IsBanned(userData.user.Username, out BanRecord))
                    {
                        if (BiosEmuThiago.GetGame().GetModerationManager().UsernameBanCheck(userData.user.Username))
                        {
                            Disconnect();
                            return false;
                        }
                    }
                }
                #endregion

                BiosEmuThiago.GetGame().GetClientManager().RegisterClient(this, userData.userID, userData.user.Username);
                _habbo = userData.user;
                _habbo.ssoTicket = AuthTicket;
                if (_habbo != null)
                {
                    userData.user.Init(this, userData);

                    SendMessage(new AuthenticationOKComposer());
                    SendMessage(new AvatarEffectsComposer(_habbo.Effects().GetAllEffects));
                    SendMessage(new NavigatorSettingsComposer(_habbo.HomeRoom));
                    SendMessage(new FavouritesComposer(userData.user.FavoriteRooms));
                    SendMessage(new FigureSetIdsComposer(_habbo.GetClothing().GetClothingParts));
                    SendMessage(new UserRightsComposer(_habbo));
                    SendMessage(new AvailabilityStatusComposer());
                    SendMessage(new AchievementScoreComposer(_habbo.GetStats().AchievementPoints));
                    SendMessage(new BuildersClubMembershipComposer());
                    SendMessage(new CfhTopicsInitComposer(BiosEmuThiago.GetGame().GetModerationManager().UserActionPresets));
                    SendMessage(new BadgeDefinitionsComposer(BiosEmuThiago.GetGame().GetAchievementManager()._achievements));
                    SendMessage(new SoundSettingsComposer(_habbo.ClientVolume, _habbo.ChatPreference, _habbo.AllowMessengerInvites, _habbo.FocusPreference, FriendBarStateUtility.GetInt(_habbo.FriendbarState)));

                    if (GetHabbo().GetMessenger() != null)
                        GetHabbo().GetMessenger().OnStatusChanged(true);

                    if (!string.IsNullOrEmpty(MachineId))
                    {
                        if (_habbo.MachineId != MachineId)
                        {
                            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("UPDATE `users` SET `machine_id` = @MachineId WHERE `id` = @id LIMIT 1");
                                dbClient.AddParameter("MachineId", MachineId);
                                dbClient.AddParameter("id", _habbo.Id);
                                dbClient.RunQuery();
                            }
                        }

                        _habbo.MachineId = MachineId;
                    }

					if (BiosEmuThiago.GetGame().GetPermissionManager().TryGetGroup(_habbo.Rank, out PermissionGroup PermissionGroup))
					{
						if (!String.IsNullOrEmpty(PermissionGroup.Badge))
							if (!_habbo.GetBadgeComponent().HasBadge(PermissionGroup.Badge))
								_habbo.GetBadgeComponent().GiveBadge(PermissionGroup.Badge, true, this);
					}

					if (!BiosEmuThiago.GetGame().GetCacheManager().ContainsUser(_habbo.Id))
                        BiosEmuThiago.GetGame().GetCacheManager().GenerateUser(_habbo.Id);

                    _habbo.InitProcess();

                    if (userData.user.GetPermissions().HasRight("mod_tickets"))
                    {
                        SendMessage(new ModeratorInitComposer(
                         BiosEmuThiago.GetGame().GetModerationManager().UserMessagePresets,
                         BiosEmuThiago.GetGame().GetModerationManager().RoomMessagePresets,
                         BiosEmuThiago.GetGame().GetModerationManager().GetTickets));
                    }

                    if (BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("user.login.message.enabled") == "1")
                        SendMessage(new MOTDNotificationComposer(BiosEmuThiago.GetGame().GetLanguageManager().TryGetValue("user.login.message")));

                    if (ExtraSettings.WELCOME_MESSAGE_ENABLED)
                    {
                        SendMessage(new MOTDNotificationComposer(ExtraSettings.WelcomeMessage.Replace("%username%", GetHabbo().Username)));
                    }

                    if (GetHabbo().Rank >= 3)
                    {
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT * FROM `ranks` WHERE id = '" + GetHabbo().Rank + "'");
                            DataRow Table = dbClient.getRow();

                            if (GetHabbo().GetBadgeComponent().HasBadge(Convert.ToString(Table["badgeid"])))
                            {

                            }
                            else
                            {

                                GetHabbo().GetBadgeComponent().GiveBadge(Convert.ToString(Table["badgeid"]), true, GetHabbo().GetClient());
                                SendMessage(RoomNotificationComposer.SendBubble("badge/" + Table["badgeid"], "Você recebeu o emblema staff do seu rank!", "/inventory/open/badge"));
                            }
                        }
                    }

                    if (ExtraSettings.TARGETED_OFFERS_ENABLED)
                    {
                        if (BiosEmuThiago.GetGame().GetTargetedOffersManager().TargetedOffer != null)
                        {
                            BiosEmuThiago.GetGame().GetTargetedOffersManager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                            TargetedOffers TargetedOffer = BiosEmuThiago.GetGame().GetTargetedOffersManager().TargetedOffer;

                            if (TargetedOffer.Expire > BiosEmuThiago.GetIUnixTimestamp())
                            {

                                if (TargetedOffer.Limit != GetHabbo()._TargetedBuy)
                                {

                                    SendMessage(BiosEmuThiago.GetGame().GetTargetedOffersManager().TargetedOffer.Serialize());
                                }
                            }
                            else
                            {
                                using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                                    dbClient.runFastQuery("UPDATE targeted_offers SET active = 'false'");
                                using (var dbClient2 = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                                    dbClient2.runFastQuery("UPDATE users SET targeted_buy = '0' WHERE targeted_buy > 0");
                            }
                        }
                    }

                    // Fixe do Presentes do HC By Thiago Araujo
                    //DateTime dateGregorians = new DateTime();
                    //dateGregorians = DateTime.Today;
                    //int days = (dateGregorians.Day);
                    //if (30 == days)
                    //{
                    //    SendMessage(new HCGiftsAlertComposer());
                    //    SendMessage(new RoomNotificationComposer("sumando", "message", "Hoje é dia de presentes HC, pegue o seu antes que acabe!"));
                    //}

                    // Da a conquista de login por dia feito por Thiago Araujo
                    string dFrank = null;
                    using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT Datahoje FROM users WHERE id = '" + userData.user.GetClient().GetHabbo().Id + "' LIMIT 1");
                        dFrank = dbClient.getString();
                    }
                    int dFrankInt = Int32.Parse(dFrank);
                    DateTime dateGregorian = new DateTime();
                    dateGregorian = DateTime.Today;
                    int day = (dateGregorian.Day);
                    if (dFrankInt != day)
                    {
                        using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE users SET Datahoje = '" + day + "' WHERE id = " + GetHabbo().Id + ";");
                        }
                        BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(userData.user.GetClient(), "ACH_Login", 1);

                    }

                    if (ExtraSettings.STAFF_MENSG_ENTERTHIAGO)
                    {
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT * FROM `ranks` WHERE id = '" + GetHabbo().Rank + "'");
                            DataRow Table = dbClient.getRow();

                            if (GetHabbo().Rank == 1)
                            {
                                // Thiago é muito lindo ser é doido
                            }
                            else
                            {
                                string figure = this.GetHabbo().Look;

                                BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figure, 3, "O " + Convert.ToString(Table["name"]) + " " + userData.user.GetClient().GetHabbo().Username + " entrou no hotel!", ""));

                            }
                        }

                        if (GetHabbo().isMedal)
                        {
                            var nuxStatus = new ServerPacket(ServerPacketHeader.NuxUserStatus);
                            nuxStatus.WriteInteger(2);
                            SendMessage(nuxStatus);

                            string thiagolindogostoso = this.GetHabbo().Look;

                            BiosEmuThiago.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + thiagolindogostoso, 3, "Hey o usuário: " + GetHabbo().Username + " acaba de se registra no hotel.", ""));

                            if (GetHabbo().isMedal)
                            {
                                string thiago = this.GetHabbo().Look;

                                SendMessage(new RoomNotificationComposer("fig/" + thiago, 3, "Hey " + userData.user.GetClient().GetHabbo().Username + " Bem-vindo ao nosso hotel!", ""));
                            }


                            if (GetHabbo().isMedal == false)
                            {
                                string thiagolindo = this.GetHabbo().Look;

                                SendMessage(new RoomNotificationComposer("fig/" + thiagolindo, 3, "Hey " + userData.user.GetClient().GetHabbo().Username + " Bem-vindo de volta ao nosso hotel!", ""));

                            }
                        }
                    }

                    if (GetHabbo().Rank > Convert.ToInt32(BiosEmuThiago.GetConfig().data["MineRankStaff"]))
                    {
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE users SET prefix_name = '' WHERE id = '" + GetHabbo().Id + "'");
                            dbClient.RunQuery("UPDATE users SET prefix_name_color = '' WHERE id = '" + GetHabbo().Id + "'");
                        }
                        GetHabbo()._NamePrefixColor = "";
                        GetHabbo()._NamePrefix = "";
                    }

                    if (GetHabbo().Rank > 0)
                    {
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT * FROM `users` WHERE id = '" + GetHabbo().Id + "'");
                            DataRow Table = dbClient.getRow();

                            if (Convert.ToString(Table["LalaConf"]) == "0")
                            {
                                // Thiago é muito lindo ser é doido
                            }
                            else
                            {
                                BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(userData.user.GetClient(), "ACH_TraderPass", 1);
                                BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(userData.user.GetClient(), "ACH_AvatarTags", 1);
                                BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(userData.user.GetClient(), "ACH_EmailVerification", 1);
                            }
                        }
                    }

                        if (GetHabbo().isMedal)
                    {
                        if (ExtraSettings.WELCOME_NEW_MESSAGE_ENABLED)
                        {
                            ServerPacket notif = new ServerPacket(ServerPacketHeader.NuxAlertMessageComposer);
                            notif.WriteString(ExtraSettings.WELCOME_MESSAGE_URL);
                            SendMessage(notif);

                        }
                        using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE users SET isMedal = '0' WHERE id = " + GetHabbo().Id + ";");
                        }
                        GetHabbo().isMedal = false;

                    }

                    BiosEmuThiago.GetGame().GetRewardManager().CheckRewards(this);
                    BiosEmuThiago.GetGame().GetAchievementManager().TryProgressHabboClubAchievements(this);
                    BiosEmuThiago.GetGame().GetAchievementManager().TryProgressRegistrationAchievements(this);
                    BiosEmuThiago.GetGame().GetAchievementManager().TryProgressLoginAchievements(this);
					ICollection<MessengerBuddy> Friends = new List<MessengerBuddy>();
					foreach (MessengerBuddy Buddy in GetHabbo().GetMessenger().GetFriends().ToList())
					{
						if (Buddy == null)
							continue;

						GameClient Friend = BiosEmuThiago.GetGame().GetClientManager().GetClientByUserID(Buddy.Id);
						if (Friend == null)
							continue;
						string figure = GetHabbo().Look;

                        Friend.SendMessage(new RoomNotificationComposer("fig/" + figure, 3, this.GetHabbo().Username + ", seu amigo acabou de entrar no hotel!", ""));

                    }
					return true;
				}
			}
			catch (Exception e)
			{
				ExceptionLogger.LogException(e);
			}
			return false;
		}

		public void SendWhisper(string Message, int Colour = 0)
        {
            if (GetHabbo() == null || GetHabbo().CurrentRoom == null)
                return;

            RoomUser User = GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(GetHabbo().Username);
            if (User == null)
                return;

            SendMessage(new WhisperComposer(User.VirtualId, Message, 0, (Colour == 0 ? User.LastBubble : Colour)));
        }

        public void SendNotification(string Message)
        {
            SendMessage(new BroadcastMessageAlertComposer(Message));
        }

        public void LogsNotif(string Message, string Key)
        {
            SendMessage(new RoomNotificationComposer(Message, Key));
        }

        public void SendMessage(IServerPacket Message)
        {
            byte[] bytes = Message.GetBytes();

            if (Message == null)
                return;

            if (GetConnection() == null)
                return;

            GetConnection().SendData(bytes);
        }

        public void SendShout(string Message, int Colour = 0)
        {
            if (this == null || GetHabbo() == null || GetHabbo().CurrentRoom == null)
                return;

            RoomUser User = GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(GetHabbo().Username);
            if (User == null)
                return;

            SendMessage(new ShoutComposer(User.VirtualId, Message, 0, (Colour == 0 ? User.LastBubble : Colour)));
        }


        public int ConnectionID
        {
            get { return _id; }
        }

        public ConnectionInformation GetConnection()
        {
            return _connection;
        }

        public Habbo GetHabbo()
        {
            return _habbo;
        }

        public void Disconnect()
        {
            try
            {
                if (GetHabbo() != null)
                {
                    ICollection<MessengerBuddy> Friends = new List<MessengerBuddy>();
                    foreach (MessengerBuddy Buddy in GetHabbo().GetMessenger().GetFriends().ToList())
                    {
                        if (Buddy == null)
                            continue;

                        GameClient Friend = BiosEmuThiago.GetGame().GetClientManager().GetClientByUserID(Buddy.Id);
                        if (Friend == null)
                            continue;
                        string figure = this.GetHabbo().Look;

                        Friend.SendMessage(new RoomNotificationComposer("fig/" + figure, 3, this.GetHabbo().Username + ", seu amigo saiu do hotel!", ""));

                    }
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery(GetHabbo().GetQueryString);
                    }

                    GetHabbo().OnDisconnect();
                    BiosEmuThiago.GetGame().GetClientManager().DispatchEventDisconnect(this);

                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
            }


            if (!_disconnected)
            {
                if (_connection != null)
                    _connection.Dispose();
                _disconnected = true;
            }
        }

        public void Dispose()
        {
            CurrentRoomUserId = -1;

            if (GetHabbo() != null)
                GetHabbo().OnDisconnect();

            MachineId = string.Empty;
            _disconnected = true;
            _habbo = null;
            _connection = null;
            RC4Client = null;
            _packetParser = null;
        }
    }
}