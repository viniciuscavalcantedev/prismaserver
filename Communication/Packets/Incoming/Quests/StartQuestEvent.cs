namespace Bios.Communication.Packets.Incoming.Quests
{
	class StartQuestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int QuestId = Packet.PopInt();

            BiosEmuThiago.GetGame().GetQuestManager().ActivateQuest(Session, QuestId);
        }
    }
}
