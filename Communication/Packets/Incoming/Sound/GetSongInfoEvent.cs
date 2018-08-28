using Bios.Communication.Packets.Outgoing.Sound;

namespace Bios.Communication.Packets.Incoming.Sound
{
    class GetSongInfoEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new TraxSongInfoComposer());
        }
    }
}
