using Bios.Communication.Packets.Outgoing.Catalog;
using Bios.HabboHotel.GameClients;

namespace Bios.Communication.Packets.Incoming.Catalog
{
    public class GetRecyclerRewardsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new RecyclerRewardsComposer());
        }
    }
}