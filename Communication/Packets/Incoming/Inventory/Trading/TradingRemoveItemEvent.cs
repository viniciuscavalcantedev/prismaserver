﻿using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Items;
using Bios.HabboHotel.Rooms.Trading;

namespace Bios.Communication.Packets.Incoming.Inventory.Trading
{
    class TradingRemoveItemEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CanTradeInRoom)
                return;

            Trade Trade = Room.GetUserTrade(Session.GetHabbo().Id);
            if (Trade == null)
                return;

            Item Item = Session.GetHabbo().GetInventoryComponent().GetItem(Packet.PopInt());
            if (Item == null)
                return;

            Trade.TakeBackItem(Session.GetHabbo().Id, Item);
        }
    }
}