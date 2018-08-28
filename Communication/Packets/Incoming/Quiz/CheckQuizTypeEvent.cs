using Bios.HabboHotel.GameClients;

namespace Bios.Communication.Packets.Incoming.Quiz
{
	class CheckQuizTypeEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_SafetyQuizGraduate", 1, false);
        }
    }
}
