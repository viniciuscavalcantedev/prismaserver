namespace Bios.Communication.Packets.Incoming.Quests
{
	class CancelQuestEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            BiosEmuThiago.GetGame().GetQuestManager().CancelQuest(Session, Packet);
        }
    }
}
