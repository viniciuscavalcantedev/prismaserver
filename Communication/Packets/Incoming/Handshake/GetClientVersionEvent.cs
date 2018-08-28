using Bios.HabboHotel.GameClients;


namespace Bios.Communication.Packets.Incoming.Handshake
{
    public class GetClientVersionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Build = Packet.PopString();

            if (BiosEmuThiago.SWFRevision != Build)
                BiosEmuThiago.SWFRevision = Build;
        }
    }
}