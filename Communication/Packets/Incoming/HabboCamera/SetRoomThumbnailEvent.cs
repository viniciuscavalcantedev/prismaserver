using Bios.Communication.Packets.Outgoing.HabboCamera;
using Bios.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios.Communication.Packets.Incoming.HabboCamera
{
    public class SetRoomThumbnailEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new SendRoomThumbnailAlertComposer());
        }
    }
}
