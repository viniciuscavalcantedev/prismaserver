using System.Linq;
using System.Text;
using System.Collections.Generic;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.Rooms.Chat.Commands.User;
using Bios.HabboHotel.Rooms.Chat.Commands.User.Fun;
using Bios.HabboHotel.Rooms.Chat.Commands.Moderator;
using Bios.HabboHotel.Rooms.Chat.Commands.Moderator.Fun;
using Bios.HabboHotel.Rooms.Chat.Commands.Administrator;
using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Database.Interfaces;
using Bios.HabboHotel.Rooms.Chat.Commands.Events;
using Bios.HabboHotel.Items.Wired;



namespace Bios.HabboHotel.Rooms.Chat.Commands
{
    public class CommandManager
    {
        private string _prefix = ":";
        private readonly Dictionary<string, IChatCommand> _commands;
        public CommandManager(string Prefix)
        {
            this._prefix = Prefix;
            this._commands = new Dictionary<string, IChatCommand>();
            this.RegisterVIP();
            this.RegisterUser();
            this.RegisterEvents();
            this.RegisterModerator();
            this.RegisterAdministrator();
        }

        public bool Parse(GameClient Session, string Message)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().CurrentRoom == null)
                return false;

            if (!Message.StartsWith(_prefix))
                return false;

            if (Message == _prefix + "comandos")
            {
                StringBuilder List = new StringBuilder();
                List.Append("- LISTA DE COMANDOS DISPONIVEIS -\n\n");
                foreach (var CmdList in _commands.ToList())
                {
                    if (!string.IsNullOrEmpty(CmdList.Value.PermissionRequired))
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand(CmdList.Value.PermissionRequired))
                            continue;
                    }

                    List.Append(":" + CmdList.Key + " " + CmdList.Value.Parameters + " >> " + CmdList.Value.Description + "\n········································································\n");
                }
               
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return true;
            }

            Message = Message.Substring(1);
            string[] Split = Message.Split(' ');

            if (Split.Length == 0)
                return false;

			if (Session.GetHabbo().Rank == 1)
			{
				this.LogCommand(Session.GetHabbo().Id, Message, Session.GetHabbo().MachineId);
			}
			if (_commands.TryGetValue(Split[0].ToLower(), out IChatCommand Cmd))
            {

                if (Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                    this.LogCommandStaff(Session.GetHabbo().Id, Message, Session.GetHabbo().MachineId, Session.GetHabbo().Username);

                if (!string.IsNullOrEmpty(Cmd.PermissionRequired))
                {
                    if (!Session.GetHabbo().GetPermissions().HasCommand(Cmd.PermissionRequired))
                        return false;
                }


                Session.GetHabbo().IChatCommand = Cmd;
                Session.GetHabbo().CurrentRoom.GetWired().TriggerEvent(WiredBoxType.TriggerUserSaysCommand, Session.GetHabbo(), this);

                Cmd.Execute(Session, Session.GetHabbo().CurrentRoom, Split);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Registers the VIP set of commands.
        /// </summary>
        private void RegisterVIP()
        {
            this.Register("spull", new SuperPullCommand());
        }

        /// <summary>
        /// Registers the Events set of commands.
        /// </summary>
        private void RegisterEvents()
        {
            this.Register("eha", new EventAlertCommand());
            this.Register("eventha", new EventAlertCommand());
            this.Register("pha", new PublicityAlertCommand());
            this.Register("da2", new DiceAlertCommand());
        }

        /// <summary>
        /// Registers the default set of commands.
        /// </summary>
        private void RegisterUser()
        {
            this.Register("groupchat", new GroupChatCommand());
            this.Register("chatgrupo", new GroupChatCommand());
            this.Register("sobre", new InfoCommand());
            this.Register("about", new InfoCommand());
            this.Register("builder", new Builder());
            this.Register("pickall", new PickAllCommand());
            this.Register("ejectall", new EjectAllCommand());
            this.Register("lay", new LayCommand());
            this.Register("sit", new SitCommand());
            this.Register("help", new HelpCommand());
            this.Register("stand", new StandCommand());
            this.Register("kiss", new KissCommand());
            this.Register("mutepets", new MutePetsCommand());
            this.Register("mutebots", new MuteBotsCommand());
            this.Register("beijar", new KissCommand());
            this.Register("bater", new  BaterCommand());
            this.Register("curar", new CurarCommand());
			this.Register("cor", new ColourCommand());
			this.Register("sexo", new SexCommand());
			this.Register("fumar", new WeedCommand());

			this.Register("mimic", new MimicCommand());
            this.Register("copiar", new MimicCommand());
            this.Register("dance", new DanceCommand());
            this.Register("push", new PushCommand());
            this.Register("pull", new PullCommand());
            this.Register("enable", new EnableCommand());
            this.Register("efeito", new EnableCommand());
            this.Register("follow", new FollowCommand());
            this.Register("faceless", new FacelessCommand());
            this.Register("moonwalk", new MoonwalkCommand());

            this.Register("unload", new UnloadCommand());
            this.Register("reload", new UnloadCommand(true));
            this.Register("fixroom", new RegenMaps());
            this.Register("empty", new EmptyItems());
            this.Register("setmax", new SetMaxCommand());
            this.Register("setspeed", new SetSpeedCommand());
            this.Register("disablefriends", new DisableFriendsCommand());
            this.Register("enablefriends", new EnableFriendsCommand());
            this.Register("disablediagonal", new DisableDiagonalCommand());
            this.Register("flagme", new FlagMeCommand());
            this.Register("stats", new StatsCommand());
            this.Register("kickpets", new KickPetsCommand());
            this.Register("kickbots", new KickBotsCommand());

            this.Register("room", new RoomCommand());
            this.Register("dnd", new DNDCommand());
            this.Register("matar", new MatarCommand());
            this.Register("disablegifts", new DisableGiftsCommand());
            this.Register("convertcredits", new ConvertCreditsCommand());
            this.Register("convertduckets", new ConvertDucketsCommand());
            this.Register("disablewhispers", new DisableWhispersCommand());
            this.Register("disablemimic", new DisableMimicCommand()); ;
            this.Register("pet", new PetCommand());
            this.Register("spush", new SuperPushCommand());
            this.Register("superpush", new SuperPushCommand());
            this.Register("menudecompra", new GiveSpecialReward());
            this.Register("alertas", new DisableEventsCommand());
            this.Register("emoji", new EmojiCommand());
            // Comandos de venda de Quarto By: Thiago Araujo
            this.Register("venderquarto", new roomSelllCommand());
            this.Register("comprarquarto", new roomBuyyCommand());
            this.Register("negaroferta", new roomDeclineOfferr());
            this.Register("aceitaroferta", new roomAccepttOffer());

        }

        /// <summary>
        /// Registers the moderator set of commands.
        /// </summary>
        private void RegisterModerator()
        {
            this.Register("ban", new BanCommand());
            this.Register("unban", new UnBanCommand());
            this.Register("mip", new MIPCommand());
            this.Register("ipban", new IPBanCommand());
            this.Register("bpu", new BanPubliCommand());
            this.Register("prefixname", new PrefixNameCommand());
            this.Register("pcolor", new ColourPrefixCommand());
            this.Register("ui", new UserInfoCommand());
            this.Register("userinfo", new UserInfoCommand());
            this.Register("roomcredits", new GiveRoom());
            this.Register("sa", new StaffAlertCommand());
            this.Register("ga", new GuideAlertCommand());
            this.Register("roomunmute", new RoomUnmuteCommand());
            this.Register("roommute", new RoomMuteCommand());
            this.Register("roombadge", new RoomBadgeCommand());
            this.Register("roomalert", new RoomAlertCommand());
            this.Register("roomkick", new RoomKickCommand());
            this.Register("mute", new MuteCommand());
			this.Register("unmute", new UnmuteCommand());
			this.Register("massbadge", new MassBadgeCommand());
            this.Register("massgive", new MassGiveCommand());
            this.Register("globalgive", new GlobalGiveCommand());
            this.Register("kick", new KickCommand());
            this.Register("skick", new KickCommand());
            this.Register("ha", new HotelAlertCommand());
            this.Register("hal", new HALCommand());
            this.Register("dar", new GiveCommand());
            this.Register("givebadge", new GiveBadgeCommand());
            this.Register("darbadge", new GiveBadgeCommand());
            this.Register("daremblema", new GiveBadgeCommand());
            this.Register("rbadge", new TakeUserBadgeCommand());
            this.Register("dc", new DisconnectCommand());
            this.Register("disconnect", new DisconnectCommand());
            this.Register("alert", new AlertCommand());

            this.Register("tradeban", new TradeBanCommand());
            this.Register("poll", new PollCommand()); // By: Thiago Araujo
            this.Register("quizz", new IdolQuizCommand());
            this.Register("lastmsg", new LastMessagesCommand());
            this.Register("lastconsolemsg", new LastConsoleMessagesCommand());

            this.Register("teleport", new TeleportCommand());
            this.Register("summon", new SummonCommand());
            this.Register("senduser", new SendUserCommand());
            this.Register("override", new OverrideCommand());
            this.Register("massenable", new MassEnableCommand());
            this.Register("massdance", new MassDanceCommand());
            this.Register("freeze", new FreezeCommand());
            this.Register("unfreeze", new UnFreezeCommand());
            this.Register("fastwalk", new FastwalkCommand());
            this.Register("superfastwalk", new SuperFastwalkCommand());
            this.Register("coords", new CoordsCommand());
            this.Register("alleyesonme", new AllEyesOnMeCommand());
            this.Register("allaroundme", new AllAroundMeCommand());
            this.Register("forcesit", new ForceSitCommand());

            this.Register("ignorewhispers", new IgnoreWhispersCommand());
            this.Register("forced_effects", new DisableForcedFXCommand());

            this.Register("makesay", new MakeSayCommand());
            this.Register("flaguser", new FlagUserCommand());
            this.Register("filtro", new FilterCommand());
            this.Register("usermsj", new UserMessageCommand());
            this.Register("globalmsj", new GlobalMessageCommand());
            this.Register("userson", new ViewOnlineCommand()); // By: Thiago Araujo
            this.Register("makepublic", new MakePublicCommand()); // By: Thiago Araujo
            this.Register("makeprivate", new MakePrivateCommand()); // By: Thiago Araujo
            this.Register("custonalert", new CustomizedHotelAlert()); // By: Thiago Araujo
            this.Register("premiar", new PremiarCommand()); // By: Thiago Araujo
            this.Register("loginstaff", new LogMeInCommand()); // By: Thiago Araujo
            this.Register("offstaff", new LogOffCommand()); // By: Thiago Araujo
            this.Register("premiarbonus", new PremiaBonusraros()); // By: Thiago Araujo
            this.Register("terminapoll", new EndPollCommand()); // By: Thiago Araujo
            // Comandos Policial By: Thiago Araujo
            this.Register("virarpolicial", new OfficerCommand()); // By: Thiago Araujo
            this.Register("prender", new PrisonCommand()); // By: Thiago Araujo
            this.Register("desprender", new UnPrisonCommand()); // By: Thiago Araujo
            this.Register("darvip", new ReloadUserrVIPRankCommand()); // By: Thiago Araujo
            this.Register("notifica", new NotificaCommand()); // By: Thiago Araujo
        }

        /// <summary>
        /// Registers the administrator set of commands.
        /// </summary>
        private void RegisterAdministrator()
        {
            this.Register("colocapack", new AddPredesignedCommand());
            this.Register("tirapack", new RemovePredesignedCommand());
            this.Register("bubble", new BubbleCommand());
            this.Register("staffson", new StaffInfo());
            this.Register("staffons", new StaffInfo());
            this.Register("bubblebot", new BubbleBotCommand());
            this.Register("update", new UpdateCommand());
            this.Register("atualizar", new UpdateCommand());
            this.Register("emptyuser", new EmptyUser());
            this.Register("deletegroup", new DeleteGroupCommand());
            this.Register("handitem", new CarryCommand());
            this.Register("goto", new GOTOCommand());
            this.Register("dj", new DJAlert());
            this.Register("summonall", new SummonAll());
            this.Register("djalert", new DJAlert());
            this.Register("catup", new CatalogUpdateAlert());
            this.Register("megaoferta", new MegaOferta()); // By: Thiago Araujo
            this.Register("cadstaff", new RegisterstaffCommand()); // By: Thiago Araujo
            this.Register("registrastaff", new RegisterstaffCommand()); // By: Thiago Araujo
        }

        /// <summary>
        /// Registers a Chat Command.
        /// </summary>
        /// <param name="CommandText">Text to type for this command.</param>
        /// <param name="Command">The command to execute.</param>
        public void Register(string CommandText, IChatCommand Command)
        {
            this._commands.Add(CommandText, Command);
        }

        public static string MergeParams(string[] Params, int Start)
        {
            var Merged = new StringBuilder();
            for (int i = Start; i < Params.Length; i++)
            {
                if (i > Start)
                    Merged.Append(" ");
                Merged.Append(Params[i]);
            }

            return Merged.ToString();
        }

        public void LogCommand(int UserId, string Data, string MachineId)
        {
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `logs_client_user` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
                dbClient.AddParameter("UserId", UserId);
                dbClient.AddParameter("Data", Data);
                dbClient.AddParameter("MachineId", MachineId);
                dbClient.AddParameter("Timestamp", BiosEmuThiago.GetUnixTimestamp());
                dbClient.RunQuery();
            }
        }

        public void LogCommandStaff(int UserId, string Data, string MachineId, string Username)
        {
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `logs_client_staff` (`user_id`,`data_string`,`machine_id`, `timestamp`) VALUES (@UserId,@Data,@MachineId,@Timestamp)");
                dbClient.AddParameter("UserId", UserId);
                dbClient.AddParameter("Data", Data);
                dbClient.AddParameter("MachineId", MachineId);
                dbClient.AddParameter("Timestamp", BiosEmuThiago.GetUnixTimestamp());
                dbClient.RunQuery();
            }

        }

        public bool TryGetCommand(string Command, out IChatCommand IChatCommand)
        {
            return this._commands.TryGetValue(Command, out IChatCommand);
        }
    }
}