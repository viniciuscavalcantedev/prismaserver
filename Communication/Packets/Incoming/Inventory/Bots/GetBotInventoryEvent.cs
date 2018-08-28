using System.Collections.Generic;

using Bios.HabboHotel.Users.Inventory.Bots;
using Bios.Communication.Packets.Outgoing.Inventory.Bots;

namespace Bios.Communication.Packets.Incoming.Inventory.Bots
{
    class GetBotInventoryEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().GetInventoryComponent() == null)
                return;

            ICollection<Bot> Bots = Session.GetHabbo().GetInventoryComponent().GetBots();
            Session.SendMessage(new BotInventoryComposer(Bots));
        }
    }
}
