using System.Collections.Generic;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Quests;
using Bios.Communication.Packets.Incoming;

namespace Bios.Communication.Packets.Incoming.Quests
{
    public class GetQuestListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            BiosEmuThiago.GetGame().GetQuestManager().GetList(Session, null);
        }
    }
}