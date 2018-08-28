using System.Collections.Generic;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Navigator;
using Bios.HabboHotel.Navigator;

namespace Bios.Communication.Packets.Incoming.Navigator
{
    class GetNavigatorFlatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            ICollection<SearchResultList> Categories = BiosEmuThiago.GetGame().GetNavigator().GetEventCategories();

            Session.SendMessage(new NavigatorFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}