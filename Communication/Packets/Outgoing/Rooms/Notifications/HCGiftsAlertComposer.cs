﻿using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Bios.Communication.Packets.Outgoing.Rooms.Notifications
{
    class HCGiftsAlertComposer : ServerPacket
    {
        public HCGiftsAlertComposer() : base(ServerPacketHeader.HCGiftsAlertComposer)
        { base.WriteInteger(1); }
    }
}