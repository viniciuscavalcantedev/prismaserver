using System;
using Bios.Utilities;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Quests;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Engine;

using Bios.Database.Interfaces;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using System.Data;

namespace Bios.Communication.Packets.Incoming.Rooms.Avatar
{
    class ChangeMottoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendNotification("Bem, agora você está mudo: Você não pode mudar a sua missão.");
                return;
            }

            if ((DateTime.Now - Session.GetHabbo().LastMottoUpdateTime).TotalSeconds <= 2.0)
            {
                Session.GetHabbo().MottoUpdateWarnings += 1;
                if (Session.GetHabbo().MottoUpdateWarnings >= 25)
                    Session.GetHabbo().SessionMottoBlocked = true;
                return;
            }

            if (Session.GetHabbo().SessionMottoBlocked)
                return;

            Session.GetHabbo().LastMottoUpdateTime = DateTime.Now;

            string newMotto = StringCharFilter.Escape(Packet.PopString().Trim());

            if (newMotto.Length > 38)
                newMotto = newMotto.Substring(0, 38);

            if (newMotto == Session.GetHabbo().Motto)
                return;

            string word;
            if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override"))
                newMotto = BiosEmuThiago.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(newMotto, out word) ? "Spam" : newMotto;

            Session.GetHabbo().Motto = newMotto;

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `motto` = @motto WHERE `id` = @userId LIMIT 1");
                dbClient.AddParameter("userId", Session.GetHabbo().Id);
                dbClient.AddParameter("motto", newMotto);
                dbClient.RunQuery();
            }

            if (Session.GetHabbo().Rank > 0)
            {
                DataRow presothiago = null;
                using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT Presidio FROM users WHERE id = '" + Session.GetHabbo().Id + "'");
                    presothiago = dbClient.getRow();
                }

                if (Convert.ToBoolean(presothiago["Presidio"]) == true)
                {
                    if (Session.GetHabbo().Rank > 0)
                    {
                        string thiago = Session.GetHabbo().Look;
                        Session.SendMessage(new RoomNotificationComposer("police_announcement", "message", "Você esta preso e não pode troca sua missão."));
                        return;
                    }
                }
            }

            BiosEmuThiago.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.PROFILE_CHANGE_MOTTO);
            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_Motto", 1);

            if (Session.GetHabbo().InRoom)
            {
                Room Room = Session.GetHabbo().CurrentRoom;
                if (Room == null)
                    return;

                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (User == null || User.GetClient() == null)
                    return;

                Room.SendMessage(new UserChangeComposer(User, false));
            }
        }
    }
}
