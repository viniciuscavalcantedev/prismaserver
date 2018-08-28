using System;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Utilities;
using Bios.HabboHotel.Items;
using Bios.Communication.Packets.Outgoing.Inventory.Furni;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Communication.Packets.Outgoing.Rooms.Nux;

namespace Bios.Communication.Packets.Incoming.Rooms.Nux
{
    class NuxAcceptGiftsMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new NuxItemListComposer());
        }
    }
}