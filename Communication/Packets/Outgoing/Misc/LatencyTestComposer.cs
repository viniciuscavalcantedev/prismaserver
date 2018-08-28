using System;
using Bios.HabboHotel.GameClients;

namespace Bios.Communication.Packets.Outgoing.Misc
{
	class LatencyTestComposer : ServerPacket
    {
        public LatencyTestComposer(GameClient Session, int testResponce)
            : base(ServerPacketHeader.LatencyResponseMessageComposer)
        {
            if (Session == null)
                return;

            Session.TimePingedReceived = DateTime.Now;

			WriteInteger(testResponce);
            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_AllTimeHotelPresence", 1);
        }
    }
}
