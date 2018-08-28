
using System.Collections.Generic;

using Bios.HabboHotel.Games;
using Bios.Communication.Packets.Outgoing.GameCenter;

namespace Bios.Communication.Packets.Incoming.GameCenter
{
    class GetGameListingEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<GameData> Games = BiosEmuThiago.GetGame().GetGameDataManager().GameData;

            Session.SendMessage(new GameListComposer(Games));
        }
    }
}
