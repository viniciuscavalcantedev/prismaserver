
using System.Linq;


using Bios.Communication.Packets.Outgoing.Inventory.Achievements;

namespace Bios.Communication.Packets.Incoming.Inventory.Achievements
{
    class GetAchievementsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new AchievementsComposer(Session, BiosEmuThiago.GetGame().GetAchievementManager()._achievements.Values.ToList()));
        }
    }
}
