﻿

namespace Bios.Communication.Packets.Outgoing.Rooms.Permissions
{
    class YouAreOwnerComposer : ServerPacket
    {
        public YouAreOwnerComposer()
            : base(ServerPacketHeader.YouAreOwnerMessageComposer)
        {
        }
    }
}
