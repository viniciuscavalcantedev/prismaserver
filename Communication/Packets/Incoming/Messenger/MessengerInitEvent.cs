using System.Linq;
using System.Collections.Generic;

using Bios.HabboHotel.Users.Messenger;
using Bios.Communication.Packets.Outgoing.Messenger;

namespace Bios.Communication.Packets.Incoming.Messenger
{
    class MessengerInitEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || Session.GetHabbo().GetMessenger() == null)
                return;

            Session.GetHabbo().GetMessenger().OnStatusChanged(false);

            ICollection<MessengerBuddy> Friends = new List<MessengerBuddy>();
            foreach (MessengerBuddy Buddy in Session.GetHabbo().GetMessenger().GetFriends().ToList())
            {
                if (Buddy == null || Buddy.IsOnline) continue;
                Friends.Add(Buddy);
            }

            Session.SendMessage(new MessengerInitComposer(Session));
            Session.SendMessage(new BuddyListComposer(Friends, Session.GetHabbo()));

            Session.GetHabbo().GetMessenger().ProcessOfflineMessages();
        }
    }
}