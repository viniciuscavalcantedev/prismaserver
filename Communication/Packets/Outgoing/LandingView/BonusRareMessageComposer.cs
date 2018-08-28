using Bios.Communication.Packets.Outgoing.Inventory.Furni;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Items;
using Bios.HabboHotel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Bios.Communication.Packets.Outgoing.LandingView
{
    class BonusRareMessageComposer : ServerPacket
    {
        public BonusRareMessageComposer(GameClient Session)
            : base(ServerPacketHeader.BonusRareMessageComposer)
        {
            string product = BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("bonus_rare_productdata_name");
            int baseid = int.Parse(BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("bonus_rare_item_baseid"));
            int score = int.Parse(BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("bonus_rare_total_score"));

            base.WriteString(product);
            base.WriteInteger(baseid);
            base.WriteInteger(score);
            base.WriteInteger(Session.GetHabbo().BonusPoints >= score ? score : score - Session.GetHabbo().BonusPoints); //Total To Gain
            if (Session.GetHabbo().BonusPoints >= score)
            {
                Session.GetHabbo().BonusPoints -= score;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().BonusPoints, score, 101));
                Session.SendMessage(new RoomAlertComposer("Você completou o seu Bônus Raro, você já possui seu prêmio no inventário! Você receberá outro quando você pega novos bônus."));
                ItemData Item = null;
                if (!BiosEmuThiago.GetGame().GetItemManager().GetItem((baseid), out Item))
                {
                    return;
                }

                Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                if (GiveItem != null)
                {
                    Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);

                    Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                    Session.SendMessage(new FurniListUpdateComposer());
                }

                Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
            }
        }
    }
}
