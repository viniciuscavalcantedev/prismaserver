﻿

namespace Bios.Communication.Packets.Outgoing.Rooms.Permissions
{
    class YouAreNotControllerComposer : ServerPacket
    {
        public YouAreNotControllerComposer()
            : base(ServerPacketHeader.YouAreNotControllerMessageComposer)
        {
        }
    }
}
