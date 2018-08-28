using Bios.HabboHotel.Cache.Type;

namespace Bios.Communication.Packets.Outgoing.Messenger
{
	class NewBuddyRequestComposer : ServerPacket
    {
        public NewBuddyRequestComposer(UserCache Habbo)
            : base(ServerPacketHeader.NewBuddyRequestMessageComposer)
        {
			WriteInteger(Habbo.Id);
			WriteString(Habbo.Username);
			WriteString(Habbo.Look);
        }
    }
}
