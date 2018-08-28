﻿using Bios.Communication.Packets.Outgoing;
using Bios.Communication.Packets.Outgoing.Catalog;
using Bios.Communication.Packets.Outgoing.Inventory.Furni;
using Bios.HabboHotel.Items;
using System;

namespace Bios.Communication.Packets.Incoming.Catalog
{
    class FurniMaticRecycleEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null) return;
            if (!Session.GetHabbo().InRoom) return;
            var itemsCount = Packet.PopInt();
            for (int i = 0; i < itemsCount; i++)
            {
                var itemId = Packet.PopInt();
                using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor()) dbClient.runFastQuery("DELETE FROM `items` WHERE `id` = '" + itemId + "' AND `user_id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                Session.GetHabbo().GetInventoryComponent().RemoveItem(itemId);
            }

            var reward = BiosEmuThiago.GetGame().GetFurniMaticRewardsMnager().GetRandomReward();
            if (reward == null) return;
            int rewardId;
            var furniMaticBoxId = 4692;
            ItemData data = null;
            BiosEmuThiago.GetGame().GetItemManager().GetItem(furniMaticBoxId, out data);
            var maticData = DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day;
            using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("INSERT INTO `items` (`base_item`,`user_id`,`extra_data`) VALUES ('" + data.Id + "', '" + Session.GetHabbo().Id + "', @extra_data)");
                dbClient.AddParameter("extra_data", maticData);
                rewardId = Convert.ToInt32(dbClient.InsertQuery());
                dbClient.runFastQuery("INSERT INTO `user_presents` (`item_id`,`base_id`,`extra_data`) VALUES ('" + rewardId + "', '" + reward.GetBaseItem().Id + "', '')");
                dbClient.runFastQuery("DELETE FROM `items` WHERE `id` = " + rewardId + " LIMIT 1;");
            }

            var GiveItem = ItemFactory.CreateGiftItem(data, Session.GetHabbo(), maticData, maticData, rewardId, 0, 0);
            if (GiveItem != null)
            {
                Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);
                Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                Session.SendMessage(new PurchaseOKComposer());
                Session.SendMessage(new FurniListAddComposer(GiveItem));
                Session.SendMessage(new FurniListUpdateComposer());
            }

            var response = new ServerPacket(ServerPacketHeader.FurniMaticReceiveItem);
            response.WriteInteger(1);
            response.WriteInteger(0); 
            Session.SendMessage(response);
            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_FurnimaticQuest", 1);
        }
    }
}