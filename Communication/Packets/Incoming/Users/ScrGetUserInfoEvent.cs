using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Users;
using Bios.Communication.Packets.Outgoing.Handshake;

namespace Bios.Communication.Packets.Incoming.Users
{
    class ScrGetUserInfoEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
            Session.SendMessage(new UserRightsComposer(Session.GetHabbo()));
        }
    }
}
