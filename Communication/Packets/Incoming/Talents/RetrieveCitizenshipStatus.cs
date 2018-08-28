using Bios.Communication.Packets.Outgoing.Talents;

namespace Bios.Communication.Packets.Incoming.Talents
{
	class RetrieveCitizenshipStatus : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            Session.SendMessage(new TalentTrackLevelComposer(Session, Type));
        }
    }
}
