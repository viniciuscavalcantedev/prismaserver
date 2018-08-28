using Bios.Database.Interfaces;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Users;
using Bios.Communication.Packets.Outgoing.Rooms.Action;

namespace Bios.Communication.Packets.Incoming.Rooms.Action
{
    class IgnoreUserEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            if (!session.GetHabbo().InRoom)
                return;

            Room Room = session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            string Username = packet.PopString();

            Habbo Player = BiosEmuThiago.GetHabboByUsername(Username);
            if (Player == null || Player.GetPermissions().HasRight("mod_tool"))
                return;

            if (session.GetHabbo().GetIgnores().TryGet(Player.Id))
                return;

            if (session.GetHabbo().GetIgnores().TryAdd(Player.Id))
            {
                using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO `user_ignores` (`user_id`,`ignore_id`) VALUES(@uid,@ignoreId);");
                    dbClient.AddParameter("uid", session.GetHabbo().Id);
                    dbClient.AddParameter("ignoreId", Player.Id);
                    dbClient.RunQuery();
                }

                session.SendMessage(new IgnoreStatusComposer(1, Player.Username));

                BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(session, "ACH_SelfModIgnoreSeen", 1);
            }
        }
    }
}
