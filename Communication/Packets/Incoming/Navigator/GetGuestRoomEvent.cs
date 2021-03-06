﻿using Bios.Communication.Packets.Outgoing.Navigator;
using Bios.HabboHotel.Rooms;

namespace Bios.Communication.Packets.Incoming.Navigator
{
    class GetGuestRoomEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int roomID = Packet.PopInt();

            RoomData roomData = BiosEmuThiago.GetGame().GetRoomManager().GenerateRoomData(roomID);
            if (roomData == null)
                return;

            bool isLoading = Packet.PopInt() == 1;
            bool checkEntry = Packet.PopInt() == 1;

            Session.SendMessage(new GetGuestRoomResultComposer(Session, roomData, isLoading, checkEntry));
        }
    }
}
