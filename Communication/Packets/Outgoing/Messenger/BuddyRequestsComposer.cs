using System.Collections.Generic;
using Bios.HabboHotel.Users.Messenger;
using Bios.HabboHotel.Cache.Type;

namespace Bios.Communication.Packets.Outgoing.Messenger
{
	class BuddyRequestsComposer : ServerPacket
    {
        public BuddyRequestsComposer(ICollection<MessengerRequest> Requests)
            : base(ServerPacketHeader.BuddyRequestsMessageComposer)
        {
			WriteInteger(Requests.Count);
			WriteInteger(Requests.Count);

            foreach (MessengerRequest Request in Requests)
            {
				WriteInteger(Request.From);
				WriteString(Request.Username);

                UserCache User = BiosEmuThiago.GetGame().GetCacheManager().GenerateUser(Request.From);
				WriteString(User != null ? User.Look : "");
            }
        }
    }
}
