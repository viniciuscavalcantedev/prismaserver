using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Catalog;

namespace Bios.Communication.Packets.Incoming.Catalog
{
    class GetClubGiftsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

            Session.SendMessage(new ClubGiftsComposer());
        }
    }
}
