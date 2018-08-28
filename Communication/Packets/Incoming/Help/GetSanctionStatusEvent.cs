using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Help;

namespace Bios.Communication.Packets.Incoming.Help
{
    class GetSanctionStatusEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new SanctionStatusComposer());
        }
    }
}
