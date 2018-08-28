using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios.Communication.Packets.Outgoing.Rooms.Engine
{
    class RoomPropertyComposer : ServerPacket
    {
        public RoomPropertyComposer(string name, string val)
            : base(ServerPacketHeader.RoomPropertyMessageComposer)
        {
           base.WriteString(name);
           base.WriteString(val);
        }
    }
}
