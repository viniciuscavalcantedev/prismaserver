using Bios.HabboHotel.Users.Messenger;

namespace Bios.Communication.Packets.Outgoing.Messenger
{
	class FriendNotificationComposer : ServerPacket
    {
        public FriendNotificationComposer(int UserId, MessengerEventTypes type, string data)
            : base(ServerPacketHeader.FriendNotificationMessageComposer)
        {
			WriteString(UserId.ToString());
			WriteInteger(MessengerEventTypesUtility.GetEventTypePacketNum(type));
			WriteString(data);
        }
    }
}
