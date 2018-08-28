namespace Bios.Communication.Packets.Incoming.Quests
{
	class GetCurrentQuestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            BiosEmuThiago.GetGame().GetQuestManager().GetCurrentQuest(Session, Packet);
        }
    }
}
