using Bios.Communication.Packets.Incoming;
using Bios.HabboHotel.GameClients;

namespace Bios.Communication.Packets
{
    public interface IPacketEvent
    {
        void Parse(GameClient Session, ClientPacket Packet);
    }
}