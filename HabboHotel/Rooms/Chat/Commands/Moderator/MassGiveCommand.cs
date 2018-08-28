using System.Linq;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.GameClients;
using System.Text;
using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class MassGiveCommand : IChatCommand
    {
        public string PermissionRequired => "command_mass_give";
        public string Parameters => "[MOEDA] [QUANTIDADE]";
        public string Description => "Dê créditos, duckets, diamantes a todos na sala.";

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
                List.Append("Como posso dar créditos, diamantes, duckets ou " + Core.ExtraSettings.PTOS_COINS + " ?\n\n");
                List.Append(":massgive credits [QUANTIDADE] - Créditos para todos os usuários da sala.\n\n");
                List.Append(":massgive diamonds [QUANTIDADE] - Diamantes para todos os usuários da sala.\n\n");
                List.Append(":massgive duckets [QUANTIDADE] - Duckets para todos os usuários da sala.\n\n");
                List.Append(":massgive " + Core.ExtraSettings.PTOS_COINS + " [QUANTIDADE] - " + Core.ExtraSettings.PTOS_COINS + " para todos os usuários da sala.\n\n");
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return;
            }

            var updateVal = Params[1];
            switch (updateVal.ToLower())
            {
                case "coins":
                case "credits":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_coins"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (var client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != Session.GetHabbo().Username))
                            {
                                client.GetHabbo().Credits = client.GetHabbo().Credits += amount;
                                client.SendMessage(new CreditBalanceComposer(client.GetHabbo().Credits));

                                 client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " crédito(s) de " + Session.GetHabbo().Username + "!"));
                            }

                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;
                    }

                case "pixels":
                case "duckets":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_pixels"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (var client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != Session.GetHabbo().Username))
                            {
                                client.GetHabbo().Duckets += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(
                                    client.GetHabbo().Duckets, amount));

                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " ducket(s) de " + Session.GetHabbo().Username + "!"));
                            }
                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;
                    }

                case "diamonds":
                case "diamantes":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_diamonds"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (var client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != Session.GetHabbo().Username))
                            {
                                client.GetHabbo().Diamonds += amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().Diamonds,
                                    amount,
                                    5));

                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " diamante(s) de " + Session.GetHabbo().Username + "!"));
                            }

                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;
                    }

                case "gotws":
                case "gotw":
                case "gotwpoints":
                case "fame":
                case "fama":
                case "ptf":
                case "famepoints":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        int Amount;
                        if (int.TryParse(Params[2], out Amount))
                        {
                            if (Amount > 50)
                            {
                                Session.SendWhisper("Não podem enviar mais de 50 Pontos, isso será notificado ao CEO e tomará medidas.");
                                return;
                            }

                            foreach (GameClient Target in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
                            {
                                if (Target == null || Target.GetHabbo() == null || Target.GetHabbo().Username == Session.GetHabbo().Username)
                                    continue;

                                Target.GetHabbo().GOTWPoints = Target.GetHabbo().GOTWPoints + Amount;
                                Target.GetHabbo().UserPoints = Target.GetHabbo().UserPoints + 1;
                                Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().GOTWPoints, Amount, 103));

                                if (Target.GetHabbo().Id != Session.GetHabbo().Id)
                                    Target.SendMessage(RoomNotificationComposer.SendBubble("command_notification_credits", "" + Session.GetHabbo().Username + " enviou " + Amount + " " + Core.ExtraSettings.PTOS_COINS + ".", "")); 
                            }

                            break;
                        }
                        else
                        {
                            Session.SendWhisper("Opa, as quantidades apenas em números ...!");
                            break;
                        }
                    }
                case "gotwt":
                case "gotwpointst":
                case "famet":
                case "famat":
                case "ptft":
                case "famepointst":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        int amount;
                        if (int.TryParse(Params[2], out amount))
                        {
                            foreach (var client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList().Where(client => client?.GetHabbo() != null && client.GetHabbo().Username != Session.GetHabbo().Username))
                            {
                                client.GetHabbo().GOTWPoints = client.GetHabbo().GOTWPoints + amount;
                                client.SendMessage(new HabboActivityPointNotificationComposer(client.GetHabbo().GOTWPoints,
                                    amount, 103));

                                client.SendMessage(new RoomNotificationComposer("command_notification_credits", "message", "Recebeu " + amount + " " + Core.ExtraSettings.PTOS_COINS + " de " + Session.GetHabbo().Username + "!"));
                            }
                            break;
                        }
                        Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                        break;
                    }
                default:
                    Session.SendWhisper("'" + updateVal + "' não é uma moeda válida!");
                    break;
            }
        }
    }
}