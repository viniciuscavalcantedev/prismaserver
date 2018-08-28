using System.Data;

using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Catalog.Vouchers;



using Bios.Communication.Packets.Outgoing.Catalog;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Core;
using Bios.Database.Interfaces;
using Bios.HabboHotel.Items;
using Bios.Communication.Packets.Outgoing.Inventory.Furni;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.Communication.Packets.Incoming.Catalog
{
    public class RedeemVoucherEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string VoucherCode = Packet.PopString().Replace("\r", "");

            Voucher Voucher = null;
            if (!BiosEmuThiago.GetGame().GetCatalog().GetVoucherManager().TryGetVoucher(VoucherCode, out Voucher))
            {
                Session.SendMessage(new VoucherRedeemErrorComposer(0));
                return;
            }

            if (Voucher.CurrentUses >= Voucher.MaxUses)
            {
                Session.SendNotification("Você usou esse código o máximo de vezes permitidas!");
                return;
            }

            DataRow GetRow = null;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `user_vouchers` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND `voucher` = @Voucher LIMIT 1");
                dbClient.AddParameter("Voucher", VoucherCode);
                GetRow = dbClient.getRow();
            }

            if (GetRow != null)
            {
                Session.SendNotification("Ops! Você já está usando esse código!");
                return;
            }
            else
            {
                using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("INSERT INTO `user_vouchers` (`user_id`,`voucher`) VALUES ('" + Session.GetHabbo().Id + "', @Voucher)");
                    dbClient.AddParameter("Voucher", VoucherCode);
                    dbClient.RunQuery();
                }
            }

            Voucher.UpdateUses();

            if (Voucher.Type == VoucherType.CREDIT)
            {
                Session.GetHabbo().Credits += Voucher.Value;
                Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acaba de receber um premio! " + Voucher.Value + " créditos. Use com sabedoria " + Session.GetHabbo().Username + ".", ""));
            }
            else if (Voucher.Type == VoucherType.DUCKET)
            {
                Session.GetHabbo().Duckets += Voucher.Value;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Voucher.Value));
                Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acaba de receber um premio! " + Voucher.Value + " duckets. Use com sabedoria " + Session.GetHabbo().Username + ".", ""));
            }
            else if (Voucher.Type == VoucherType.DIAMOND)
            {
                Session.GetHabbo().Diamonds += Voucher.Value;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, Voucher.Value, 5));
                Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acaba de receber um premio! " + Voucher.Value + " diamantes. Use com sabedoria " + Session.GetHabbo().Username + ".", ""));
            }
            else if (Voucher.Type == VoucherType.GOTW)
            {
                Session.GetHabbo().GOTWPoints += Voucher.Value;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, Voucher.Value, 103));
                Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acaba de receber um premio! " + Voucher.Value + " "+ExtraSettings.PTOS_COINS+ ". Use com sabedoria " + Session.GetHabbo().Username + ".", ""));
            }
            else if (Voucher.Type == VoucherType.ITEM)
            {

                ItemData Item = null;
                if (!BiosEmuThiago.GetGame().GetItemManager().GetItem((Voucher.Value), out Item))
                {
                    // No existe este ItemId.
                    return;
                }

                Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                if (GiveItem != null)
                {
                    Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);

                    Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                    Session.SendMessage(new FurniListUpdateComposer());
                    Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acabou de receber o objeto raro, corre " + Session.GetHabbo().Username + ", confira seu invetário algo novo está ai!", ""));
                }

                Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
            }

            Session.SendMessage(new VoucherRedeemOkComposer());
        }
    }
}