using Bios.HabboHotel.Rooms;
using Bios.Communication.Packets.Outgoing.Moderation;

namespace Bios.Communication.Packets.Incoming.Moderation
{
    class GetModeratorRoomInfoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int RoomId = Packet.PopInt();

            RoomData Data = BiosEmuThiago.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            if (Data == null)
                return;

            Room Room;

            if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(RoomId, out Room))
                return;

            Session.SendMessage(new ModeratorRoomInfoComposer(Data, (Room.GetRoomUserManager().GetRoomUserByHabbo(Data.OwnerName) != null)));
        }
    }
}
