using System;
using System.Data;

using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Database.Interfaces;


namespace Bios.Communication.Packets.Incoming.Marketplace
{
    class RedeemOfferCreditsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int CreditsOwed = 0;

            DataTable Table = null;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `asking_price` FROM `catalog_marketplace_offers` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND `state` = '2'");
                Table = dbClient.getTable();
            }

            if (Table != null)
            {
                foreach (DataRow row in Table.Rows)
                {
                    CreditsOwed += Convert.ToInt32(row["asking_price"]);
                }

                if (CreditsOwed >= 1)
                {
                    Session.GetHabbo().Credits += CreditsOwed;
                    Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                }

                using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("DELETE FROM `catalog_marketplace_offers` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND `state` = '2'");
                }
            }
        }
    }
}