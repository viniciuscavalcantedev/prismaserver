﻿using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Items;
using Bios.Communication.Packets.Outgoing.Rooms.Furni.Stickys;

namespace Bios.Communication.Packets.Incoming.Rooms.Furni.Stickys
{
	class GetStickyNoteEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            Item Item = Room.GetRoomItemHandler().GetItem(Packet.PopInt());
            if (Item == null || Item.GetBaseItem().InteractionType != InteractionType.POSTIT)
                return;

            Session.SendMessage(new StickyNoteComposer(Item.Id.ToString(), Item.ExtraData));
        }
    }
}