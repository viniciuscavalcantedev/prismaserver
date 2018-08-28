using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Bios.Communication.Packets.Outgoing.LandingView;

namespace Bios.Communication.Packets.Incoming.LandingView
{
    class RequestBonusRareEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BonusRareMessageComposer(Session));
        }
    }
}
