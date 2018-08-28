using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Session;

namespace Bios.Communication.Packets.Incoming.Rooms.Connection
{
    class GoToFlatEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            if (!Session.GetHabbo().EnterRoom(Session.GetHabbo().CurrentRoom))
                Session.SendMessage(new CloseConnectionComposer());
        }
    }
}
