using Bios.HabboHotel.Users;
using Bios.Communication.Packets.Outgoing.Users;

namespace Bios.Communication.Packets.Incoming.Users
{
    class GetSelectedBadgesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int UserId = Packet.PopInt();
            Habbo Habbo = BiosEmuThiago.GetHabboById(UserId);
            if (Habbo == null)
                return;

            Session.SendMessage(new HabboUserBadgesComposer(Habbo));
        }
    }
}