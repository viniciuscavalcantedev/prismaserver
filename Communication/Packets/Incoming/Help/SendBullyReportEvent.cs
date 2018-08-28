

using Bios.Communication.Packets.Outgoing.Help;

namespace Bios.Communication.Packets.Incoming.Help
{
    class SendBullyReportEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new SendBullyReportComposer());
        }
    }
}
