﻿
using Bios.HabboHotel.Rooms;

namespace Bios.Communication.Packets.Incoming.Inventory.Trading
{
	class TradingCancelConfirmEvent : IPacketEvent
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

            Room.TryStopTrade(Session.GetHabbo().Id);
        }
    }
}
