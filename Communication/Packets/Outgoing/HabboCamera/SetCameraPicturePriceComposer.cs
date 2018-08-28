﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios.Communication.Packets.Outgoing.HabboCamera
{
    class SetCameraPicturePriceComposer : ServerPacket
    {
        public SetCameraPicturePriceComposer(int BuyPicCreditCost, int BuyPicDucketCost, int PublishPicDucketCost)
            : base(ServerPacketHeader.SetCameraPicturePriceMessageComposer)
        {
            base.WriteInteger(BuyPicCreditCost);
            base.WriteInteger(BuyPicDucketCost);
            base.WriteInteger(PublishPicDucketCost);

        }
    }
}
