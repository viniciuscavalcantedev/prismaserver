using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Catalog;
using Bios.Communication.Packets.Outgoing.BuildersClub;

namespace Bios.Communication.Packets.Incoming.Catalog
{
    public class GetCatalogIndexEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            /*int Sub = 0;

            if (Session.GetHabbo().GetSubscriptionManager().HasSubscription)
            {
                Sub = Session.GetHabbo().GetSubscriptionManager().GetSubscription().SubscriptionId;
            }*/

            Session.SendMessage(new CatalogIndexComposer(Session, BiosEmuThiago.GetGame().GetCatalog().GetPages()));//, Sub));
            Session.SendMessage(new CatalogItemDiscountComposer());
            Session.SendMessage(new BCBorrowedItemsComposer());
        }
    }
}