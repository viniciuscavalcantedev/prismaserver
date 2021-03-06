﻿using Bios.Communication.Packets.Outgoing.Rooms.Session;
using Bios.Communication.Packets.Outgoing.Rooms.Nux;
using Bios.HabboHotel.Rooms;

namespace Bios.Communication.Packets.Incoming.Navigator
{
    class FindRandomFriendingRoomEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            var type = Packet.PopString();
            if (type == "predefined_noob_lobby")
            {
                Session.SendMessage(new NuxAlertComposer("nux/lobbyoffer/hide"));
                Session.SendMessage(new RoomForwardComposer(4));
                return;
            }

            Room Instance = BiosEmuThiago.GetGame().GetRoomManager().TryGetRandomLoadedRoom();
            if (Instance != null)
                Session.SendMessage(new RoomForwardComposer(Instance.Id));
        }
    }
}
