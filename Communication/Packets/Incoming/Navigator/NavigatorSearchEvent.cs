using System.Collections.Generic;
using Bios.Communication.Packets.Outgoing.Navigator;
using Bios.HabboHotel.Navigator;

namespace Bios.Communication.Packets.Incoming.Navigator
{
    class NavigatorSearchEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            string Category = packet.PopString();
            string Search = packet.PopString();

            ICollection<SearchResultList> Categories = new List<SearchResultList>();

            if (!string.IsNullOrEmpty(Search))
            {
                SearchResultList QueryResult = null;
                if (BiosEmuThiago.GetGame().GetNavigator().TryGetSearchResultList(0, out QueryResult))
                {
                    Categories.Add(QueryResult);
                }
            }
            else
            {
                Categories = BiosEmuThiago.GetGame().GetNavigator().GetCategorysForSearch(Category);
                if (Categories.Count == 0)
                {
                    //Are we going in deep?!
                    Categories = BiosEmuThiago.GetGame().GetNavigator().GetResultByIdentifier(Category);
                    if (Categories.Count > 0)
                    {
                        session.SendMessage(new NavigatorSearchResultSetComposer(Category, Search, Categories, session, 2, 100));
                        return;
                    }
                }
            }

            session.SendMessage(new NavigatorSearchResultSetComposer(Category, Search, Categories, session));
        }
    }
}
