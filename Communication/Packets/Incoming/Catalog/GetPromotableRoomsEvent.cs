using System.Linq;
using System.Collections.Generic;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Catalog;

namespace Bios.Communication.Packets.Incoming.Catalog
{
	class GetPromotableRoomsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            List<RoomData> Rooms = Session.GetHabbo().UsersRooms;
            Rooms = Rooms.Where(x => (x.Promotion == null || x.Promotion.TimestampExpires < BiosEmuThiago.GetUnixTimestamp())).ToList();
            Session.SendMessage(new PromotableRoomsComposer(Rooms));
        }
    }
}
