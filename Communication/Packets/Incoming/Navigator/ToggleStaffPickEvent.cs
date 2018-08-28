using Bios.HabboHotel.Navigator;
using Bios.HabboHotel.GameClients;
using Bios.Database.Interfaces;
using Bios.HabboHotel.Rooms;
using Bios.Communication.Packets.Outgoing.Navigator;
using Bios.Communication.Packets.Outgoing.Rooms.Settings;

namespace Bios.Communication.Packets.Incoming.Navigator
{
    class ToggleStaffPickEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(session.GetHabbo().CurrentRoom.OwnerName);
            if (!session.GetHabbo().GetPermissions().HasRight("room.staff_picks.management"))
                return;

            Room room = null;
            if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(packet.PopInt(), out room))
                return;

            StaffPick staffPick = null;
            if (!BiosEmuThiago.GetGame().GetNavigator().TryGetStaffPickedRoom(room.Id, out staffPick))
            {
                if (BiosEmuThiago.GetGame().GetNavigator().TryAddStaffPickedRoom(room.Id))
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("INSERT INTO `navigator_staff_picks` (`room_id`,`image`) VALUES (@roomId, null)");
                        dbClient.AddParameter("roomId", room.Id);
                        dbClient.RunQuery();
                    }
                    BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(TargetClient, "ACH_Spr", 1, false);
                }
            }
            else
            {
                if (BiosEmuThiago.GetGame().GetNavigator().TryRemoveStaffPickedRoom(room.Id))
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("DELETE FROM `navigator_staff_picks` WHERE `room_id` = @roomId LIMIT 1");
                        dbClient.AddParameter("roomId", room.Id);
                        dbClient.RunQuery();
                    }
                }
            }

            room.SendMessage(new RoomSettingsSavedComposer(room.RoomId));
            room.SendMessage(new RoomInfoUpdatedComposer(room.RoomId));
        }
    }
}