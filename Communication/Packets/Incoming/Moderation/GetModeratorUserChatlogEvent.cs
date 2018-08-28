using System;
using System.Data;
using System.Collections.Generic;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.Rooms.Chat.Logs;
using Bios.Database.Interfaces;
using Bios.Utilities;
using Bios.HabboHotel.Rooms;
using Bios.Communication.Packets.Outgoing.Moderation;

namespace Bios.Communication.Packets.Incoming.Moderation
{
    class GetModeratorUserChatlogEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int UserId = Packet.PopInt();
            Habbo Habbo = BiosEmuThiago.GetHabboById(UserId);

            if (Habbo == null)
            {
                Session.SendNotification("Não foi possível encontrar este utilizador.");
                return;
            }

            BiosEmuThiago.GetGame().GetChatManager().GetLogs().FlushAndSave();

            List<KeyValuePair<RoomData, List<ChatlogEntry>>> Chatlogs = new List<KeyValuePair<RoomData, List<ChatlogEntry>>>();
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `room_id`,`entry_timestamp`,`exit_timestamp` FROM `user_roomvisits` WHERE `user_id` = '" + Habbo.Id + "' ORDER BY `entry_timestamp` DESC LIMIT 7");
                DataTable GetLogs = dbClient.getTable();

                if (GetLogs != null)
                {
                    foreach (DataRow Row in GetLogs.Rows)
                    {
                        RoomData RoomData = BiosEmuThiago.GetGame().GetRoomManager().GenerateRoomData(Convert.ToInt32(Row["room_id"]));
                        if (RoomData == null)
                        {
                            continue;
                        }

                        double TimestampExit = (Convert.ToDouble(Row["exit_timestamp"]) <= 0 ? UnixTimestamp.GetNow() : Convert.ToDouble(Row["exit_timestamp"]));

                        Chatlogs.Add(new KeyValuePair<RoomData, List<ChatlogEntry>>(RoomData, GetChatlogs(RoomData, Convert.ToDouble(Row["entry_timestamp"]), TimestampExit)));
                    }
                }

                Session.SendMessage(new ModeratorUserChatlogComposer(Habbo, Chatlogs));
            }
        }

        private List<ChatlogEntry> GetChatlogs(RoomData RoomData, double TimeEnter, double TimeExit)
        {
            List<ChatlogEntry> Chats = new List<ChatlogEntry>();

            DataTable Data = null;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `user_id`, `timestamp`, `message` FROM `chatlogs` WHERE `room_id` = " + RoomData.Id + " AND `timestamp` > " + TimeEnter + " AND `timestamp` < " + TimeExit + " ORDER BY `timestamp` DESC LIMIT 100");
                Data = dbClient.getTable();

                if (Data != null)
                {
                    foreach (DataRow Row in Data.Rows)
                    {
                        Habbo Habbo = BiosEmuThiago.GetHabboById(Convert.ToInt32(Row["user_id"]));

                        if (Habbo != null)
                        {
                            Chats.Add(new ChatlogEntry(Convert.ToInt32(Row["user_id"]), RoomData.Id, Convert.ToString(Row["message"]), Convert.ToDouble(Row["timestamp"]), Habbo));
                        }
                    }
                }
            }

            return Chats;
        }
    }
}