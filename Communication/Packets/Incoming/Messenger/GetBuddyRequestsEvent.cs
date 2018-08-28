using System.Linq;
using System.Collections.Generic;

using Bios.HabboHotel.Users.Messenger;
using Bios.Communication.Packets.Outgoing.Messenger;

namespace Bios.Communication.Packets.Incoming.Messenger
{
    class GetBuddyRequestsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<MessengerRequest> Requests = Session.GetHabbo().GetMessenger().GetRequests().ToList();

            Session.SendMessage(new BuddyRequestsComposer(Requests));
        }
    }
}
