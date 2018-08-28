using Bios.HabboHotel.Rooms;

namespace Bios.Communication.Packets.Outgoing.Rooms.Avatar
{
	class DanceComposer : ServerPacket
    {
        public DanceComposer(RoomUser Avatar, int Dance)
            : base(ServerPacketHeader.DanceMessageComposer)
        {
			WriteInteger(Avatar.VirtualId);
			WriteInteger(Dance);
        }
    }
}