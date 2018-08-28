﻿using System.Linq;

using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Items;
using Bios.HabboHotel.Items.Data.Moodlight;

using Bios.Communication.Packets.Outgoing.Rooms.Furni.Moodlight;

namespace Bios.Communication.Packets.Incoming.Rooms.Furni.Moodlight
{
	class GetMoodlightConfigEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room;

            if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session, true))
                return;

            if (Room.MoodlightData == null)
            {
                foreach (Item item in Room.GetRoomItemHandler().GetWall.ToList())
                {
                    if (item.GetBaseItem().InteractionType == InteractionType.MOODLIGHT)
                        Room.MoodlightData = new MoodlightData(item.Id);
                }
            }

            if (Room.MoodlightData == null)
                return;

            Session.SendMessage(new MoodlightConfigComposer(Room.MoodlightData));
        }
    }
}