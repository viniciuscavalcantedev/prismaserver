/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.LandingView;

namespace Bios.Communication.Packets.Incoming.Quests
{
    class GetDailyQuestEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int UsersOnline = BiosEmuThiago.GetGame().GetClientManager().Count;

            Session.SendMessage(new ConcurrentUsersGoalProgressComposer(UsersOnline));
        }
    }
}*/
