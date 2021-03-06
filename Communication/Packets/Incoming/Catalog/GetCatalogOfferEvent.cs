﻿using Bios.HabboHotel.Catalog;
using Bios.Communication.Packets.Outgoing.Catalog;

namespace Bios.Communication.Packets.Incoming.Catalog
{
    class GetCatalogOfferEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int OfferId = Packet.PopInt();
            if (!BiosEmuThiago.GetGame().GetCatalog().ItemOffers.ContainsKey(OfferId))
                return;

            int PageId = BiosEmuThiago.GetGame().GetCatalog().ItemOffers[OfferId];

            CatalogPage Page;
            if (!BiosEmuThiago.GetGame().GetCatalog().TryGetPage(PageId, out Page))
                return;

            if (!Page.Enabled || !Page.Visible || Page.MinimumRank  >  Session.GetHabbo().Rank || (Page.MinimumVIP > Session.GetHabbo().VIPRank && Session.GetHabbo().Rank == 1))
                return;

            CatalogItem Item = null;
            if (!Page.ItemOffers.ContainsKey(OfferId))
                return;

            Item = (CatalogItem)Page.ItemOffers[OfferId];
            if (Item != null)
                Session.SendMessage(new CatalogOfferComposer(Item));
        }
    }
}
