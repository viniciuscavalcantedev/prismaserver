using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Engine;

namespace Bios.Communication.Packets.Incoming.Rooms.Engine
{
    class GetFurnitureAliasesEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new FurnitureAliasesComposer());
        }
    }
}
