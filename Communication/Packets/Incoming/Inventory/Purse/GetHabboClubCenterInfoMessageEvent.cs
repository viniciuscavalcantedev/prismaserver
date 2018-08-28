using Bios.Communication.Packets.Outgoing.Users;

namespace Bios.Communication.Packets.Incoming.Inventory.Purse
{
    class GetHabboClubCenterInfoMessageEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new GetHabboClubCenterInfoMessageComposer(Session));
        }
    }
}