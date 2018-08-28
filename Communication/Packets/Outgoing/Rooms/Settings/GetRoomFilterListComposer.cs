
using Bios.HabboHotel.Rooms;

namespace Bios.Communication.Packets.Outgoing.Rooms.Settings
{
	class GetRoomFilterListComposer : ServerPacket
    {
        public GetRoomFilterListComposer(Room Instance)
            : base(ServerPacketHeader.GetRoomFilterListMessageComposer)
        {
			WriteInteger(Instance.WordFilterList.Count);
            foreach (string Word in Instance.WordFilterList)
            {
				WriteString(Word);
            }
        }
    }
}
