﻿using Bios.HabboHotel.Rooms;

namespace Bios.Communication.Packets.Outgoing.Rooms.Avatar
{
    public class SleepComposer : ServerPacket
    {
        public SleepComposer(RoomUser User, bool IsSleeping)
            : base(ServerPacketHeader.SleepMessageComposer)
        {
			WriteInteger(User.VirtualId);
			WriteBoolean(IsSleeping);
        }
    }
}