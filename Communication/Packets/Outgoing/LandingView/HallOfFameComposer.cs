using Bios.Communication.Packets.Incoming.LandingView;

namespace Bios.Communication.Packets.Outgoing.LandingView
{
	class HallOfFameComposer : ServerPacket
    {
        public HallOfFameComposer() : base(ServerPacketHeader.UpdateHallOfFameListMessageComposer)
        {
			WriteString("halloffame.staff");
            GetHallOfFame.GetInstance().Serialize(this);
            return;
        }
    }
}
