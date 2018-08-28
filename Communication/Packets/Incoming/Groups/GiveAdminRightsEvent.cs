using Bios.HabboHotel.Users;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Groups;
using Bios.Communication.Packets.Outgoing.Groups;
using Bios.Communication.Packets.Outgoing.Rooms.Permissions;



namespace Bios.Communication.Packets.Incoming.Groups
{
    class GiveAdminRightsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!BiosEmuThiago.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Session.GetHabbo().Id != Group.CreatorId || !Group.IsMember(UserId))
                return;

            Habbo Habbo = BiosEmuThiago.GetHabboById(UserId);
            if (Habbo == null)
            {
                Session.SendNotification("Ocorreu um erro ao encontrar esse usuário!");
                return;
            }

            Group.MakeAdmin(UserId);
          
            Room Room = null;
            if (BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(Group.RoomId, out Room))
            {
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(UserId);
                if (User != null)
                {
                    if (!User.Statusses.ContainsKey("flatctrl 3"))
                        User.SetStatus("flatctrl 3", "");
                    User.UpdateNeeded = true;
                    if (User.GetClient() != null)
                        User.GetClient().SendMessage(new YouAreControllerComposer(3));
                }
            }

            Session.SendMessage(new GroupMemberUpdatedComposer(GroupId, Habbo, 1));
        }
    }
}
