﻿using System.Collections.Generic;
using Bios.HabboHotel.Rooms;

namespace Bios.Communication.Packets.Outgoing.Navigator
{
	class GuestRoomSearchResultComposer : ServerPacket
    {
       public GuestRoomSearchResultComposer(int Mode, string UserQuery, ICollection<RoomData> Rooms)
           : base(ServerPacketHeader.GuestRoomSearchResultMessageComposer)
       {
			WriteInteger(Mode);
			WriteString(UserQuery);

			WriteInteger(Rooms.Count);
           foreach (RoomData data in Rooms)
           {
               RoomAppender.WriteRoom(this, data, data.Promotion);
           }

			WriteBoolean(false);
       }
    }
}
