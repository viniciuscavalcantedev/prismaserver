﻿using System;
using System.Data;
using Bios.HabboHotel.Items;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Database.Interfaces;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class ConvertCreditsCommand : IChatCommand
    {
        public string PermissionRequired => "command_convert_credits";
        public string Parameters => "";
        public string Description => "Converter suas moedas por créditos.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            int TotalValue = 0;

            try
            {
                DataTable Table = null;
                using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT `id` FROM `items` WHERE `user_id` = '" + Session.GetHabbo().Id + "' AND (`room_id`=  '0' OR `room_id` = '')");
                    Table = dbClient.getTable();
                }

                if (Table == null)
                {
                    Session.SendWhisper("Atualmente, não tem itens no seu inventário!");
                    return;
                }

                foreach (DataRow Row in Table.Rows)
                {
                    Item Item = Session.GetHabbo().GetInventoryComponent().GetItem(Convert.ToInt32(Row[0]));
                    if (Item == null)
                        continue;

                    if (!Item.GetBaseItem().ItemName.StartsWith("CF_") && !Item.GetBaseItem().ItemName.StartsWith("CFC_"))
                        continue;

                    if (Item.RoomId > 0)
                        continue;

                    string[] Split = Item.GetBaseItem().ItemName.Split('_');
                    int Value = int.Parse(Split[1]);

                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.runFastQuery("DELETE FROM `items` WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }

                    Session.GetHabbo().GetInventoryComponent().RemoveItem(Item.Id);

                    TotalValue += Value;

                    if (Value > 0)
                    {
                        Session.GetHabbo().Credits += Value;
                        Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
                    }
                }

                if (TotalValue > 0)
                    Session.SendNotification("Todos seuscréditos convertido com éxito!\r\r(Valor Total: " + TotalValue + " Créditos!");
                else
                    Session.SendNotification("Parece que não tem itens intercambiáveis!");
            }
            catch
            {
                Session.SendNotification("ocorreu um erro ao converter seus créditos!");
            }
        }
    }
}
