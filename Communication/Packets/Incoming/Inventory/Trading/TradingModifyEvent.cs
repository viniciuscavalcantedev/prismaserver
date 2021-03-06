﻿using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Rooms.Trading;

namespace Bios.Communication.Packets.Incoming.Inventory.Trading
{
    class TradingModifyEvent : IPacketEvent
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

            Trade.Unaccept(Session.GetHabbo().Id);
        }
    }
}