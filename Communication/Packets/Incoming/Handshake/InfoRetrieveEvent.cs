
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Handshake;

namespace Bios.Communication.Packets.Incoming.Handshake
{
    public class InfoRetrieveEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new UserObjectComposer(Session.GetHabbo()));
            Session.SendMessage(new UserPerksComposer(Session.GetHabbo()));
        }
    }
}