﻿using System;
using System.Data;


using Bios.Communication.Packets.Outgoing.Marketplace;
using Bios.Database.Interfaces;

namespace Bios.Communication.Packets.Incoming.Marketplace
{
    class GetMarketplaceItemStatsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int ItemId = Packet.PopInt();
            int SpriteId = Packet.PopInt();

            DataRow Row = null;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `avgprice` FROM `catalog_marketplace_data` WHERE `sprite` = @SpriteId LIMIT 1");
                dbClient.AddParameter("SpriteId", SpriteId);
                Row = dbClient.getRow();
            }

            Session.SendMessage(new MarketplaceItemStatsComposer(ItemId, SpriteId, (Row != null ? Convert.ToInt32(Row["avgprice"]) : 0)));
        }
    }
}
