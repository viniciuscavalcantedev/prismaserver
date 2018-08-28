using System.Linq;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using System.Text;
using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class GlobalGiveCommand : IChatCommand
    {
        public string PermissionRequired => "commandglobal_currency";
        public string Parameters => "[MOEDA] [QUANTIDADE]";
        public string Description => "Enviar moedas a todos.";

        public void Execute(GameClient Session, Room room, string[] Params)
        {
            if (ExtraSettings.STAFF_EFFECT_ENABLED_ROOM)
            {
                if (Session.GetHabbo().isLoggedIn && Session.GetHabbo().Rank > Convert.ToInt32(BiosEmuThiago.GetConfig().data["MineRankStaff"]))
                {
                }
                else
                {
                    Session.SendWhisper("Você precisa estar logado como staff para usar este comando.");
                    return;
                }
            }
            if (Params.Length == 1)
            {
                StringBuilder List = new StringBuilder();
                List.Append("Como posso dar créditos, diamantes, duckets ou " + Core.ExtraSettings.PTOS_COINS + "?\n········································································\n");
                List.Append(":globalgive credits [QUANTIDADE] - Créditos para todos os usuários.\n········································································\n");
                List.Append(":globalgive diamonds [QUANTIDADE] - Diamantes para todos os usuários.\n········································································\n");
                List.Append(":globalgive duckets [QUANTIDADE] - Duckets para todos os usuários.\n········································································\n");
                List.Append(":globalgive " + Core.ExtraSettings.PTOS_COINS + " [QUANTIDADE] - " + Core.ExtraSettings.PTOS_COINS + " para todos os usuários.\n········································································\n");
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return;
            }

            string updateVal = Params[1];
            int amount;
            switch (updateVal.ToLower())
            {
                case "coins":
                case "credits":
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_coins"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Credits += amount;
                                client.SendMessage(new CreditBalanceComposer(client.GetHabbo().Credits));
                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu "+amount+" crédito(s) globais!"));
                            }
                            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("UPDATE users SET credits = credits + " + amount);
                            }
                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;

                case "pixels":
                case "duckets":
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_pixels"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Duckets += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(
                                    client.GetHabbo().Duckets, amount));
                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " ducket(s) globais!"));
                            }
                            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("UPDATE users SET activity_points = activity_points + " + amount);
                            }
                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;

                case "diamonds":
                case "diamantes":
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_diamonds"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().Diamonds += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().Diamonds,
                                    amount,
                                    5));
                            }
                            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("UPDATE users SET vip_points = vip_points + " + amount);
                            }
                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;
                case "gotw":
                case "gotws":
                case "gotwpoints":
                case "fame":
                case "fama":
                case "famepoints":
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (GameClient client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
                            {
                                client.GetHabbo().GOTWPoints = client.GetHabbo().GOTWPoints + amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().GOTWPoints,
                                    amount, 103));
                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " "+Core.ExtraSettings.PTOS_COINS+" globais!"));
                            }
                            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.runFastQuery("UPDATE users SET gotw_points = gotw_points + " + amount);
                            }
                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;
            }
        }
    }
}