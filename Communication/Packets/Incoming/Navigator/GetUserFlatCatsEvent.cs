using System.Collections.Generic;
using Bios.HabboHotel.Navigator;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Navigator;

namespace Bios.Communication.Packets.Incoming.Navigator
{
    public class GetUserFlatCatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            ICollection<SearchResultList> Categories = BiosEmuThiago.GetGame().GetNavigator().GetFlatCategories();

            Session.SendMessage(new UserFlatCatsComposer(Categories, Session.GetHabbo().Rank));
        }
    }
}