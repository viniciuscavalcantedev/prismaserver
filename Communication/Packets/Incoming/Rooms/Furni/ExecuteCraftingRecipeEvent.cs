using System;

using Bios.HabboHotel.Items;
using Bios.Communication.Packets.Outgoing.Inventory.Furni;

using Bios.Communication.Packets.Outgoing.Rooms.Furni;
using Bios.HabboHotel.Items.Crafting;

namespace Bios.Communication.Packets.Incoming.Rooms.Furni
{
	class ExecuteCraftingRecipeEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int craftingTable = Packet.PopInt();
            string RecetaFinal = Packet.PopString();


            CraftingRecipe recipe = BiosEmuThiago.GetGame().GetCraftingManager().GetRecipeByPrize(RecetaFinal);

            if (recipe == null) return;
            ItemData resultItem = BiosEmuThiago.GetGame().GetItemManager().GetItemByName(recipe.Result);
            if (resultItem == null) return;
            bool success = true;
            foreach (var need in recipe.ItemsNeeded)
            {
                for (var i = 1; i <= need.Value; i++)
                {
                    ItemData item = BiosEmuThiago.GetGame().GetItemManager().GetItemByName(need.Key);
                    if (item == null)
                    {
                        success = false;
                        continue;
                    }

                    var inv = Session.GetHabbo().GetInventoryComponent().GetFirstItemByBaseId(item.Id);
                    if (inv == null)
                    {
                        success = false;
                        continue;
                    }

                    using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor()) dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + inv.Id + "' AND `user_id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    Session.GetHabbo().GetInventoryComponent().RemoveItem(inv.Id);
                    Console.WriteLine(inv.Id);
                }
            }

            Session.GetHabbo().GetInventoryComponent().UpdateItems(true);

            Session.SendMessage(new CraftingResultComposer(recipe, true));
            Session.SendMessage(new CraftableProductsComposer());
            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CrystalCracker", 1);
            Session.SendNotification("Opa," + Session.GetHabbo().Username + " você craftor o item " + resultItem.Id + "!\n\n Você teve sorte!");

            Session.GetHabbo().GetInventoryComponent().AddNewItem(0, resultItem.Id, "", 0, true, false, 0, 0);
            Session.SendMessage(new FurniListUpdateComposer());
            Session.GetHabbo().GetInventoryComponent().UpdateItems(true);

            if (success)
            {
                Session.GetHabbo().GetInventoryComponent().AddNewItem(0, resultItem.Id, "", 0, true, false, 0, 0);
                Session.SendMessage(new FurniListUpdateComposer());
                Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                Session.SendMessage(new CraftableProductsComposer());

                switch (recipe.Type)
                {
                    case 1:
                        BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CrystalCracker", 1);
                        break;

                    case 2:
                        BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetLover", 1);
                        break;

                    case 3:
                        BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_PetLover", 1);
                        break;
                }
            }

            Session.SendMessage(new CraftingResultComposer(recipe, success));
            return;
        }
    }
}