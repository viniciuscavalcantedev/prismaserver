using Bios.Communication.Packets.Outgoing.Catalog;

namespace Bios.Communication.Packets.Incoming.Catalog
{
    class GetGroupFurniConfigEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new GroupFurniConfigComposer(BiosEmuThiago.GetGame().GetGroupManager().GetGroupsForUser(Session.GetHabbo().Id)));
        }
    }
}
