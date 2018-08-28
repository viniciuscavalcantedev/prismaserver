using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using System.Text;
using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class GiveCommand : IChatCommand
    {
        public string PermissionRequired => "command_give";
        public string Parameters => "[USUÁRIO] [MOEDA] [QUANTIDADE]";
        public string Description => "Dar créditos, duckets, diamantes e gotws a um usuário.";

        public void Execute(GameClient Session, Room Room, string[] Params)
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
                List.Append("Como posso dar créditos, diamantes, duckets ou " + Core.ExtraSettings.PTOS_COINS+ "?\n········································································\n");
                List.Append(":dar [USUÁRIO] credits [QUANTIDADE] - Créditos a um usuário.\n········································································\n");
                List.Append(":dar [USUÁRIO] diamonds [QUANTIDADE] - Diamantes a um usuário.\n········································································\n");
                List.Append(":dar [USUÁRIO] duckets [QUANTIDADE] - Duckets a um usuário.\n········································································\n");
                List.Append(":dar [USUÁRIO] " + Core.ExtraSettings.PTOS_COINS + " [QUANTIDADE] - " + Core.ExtraSettings.PTOS_COINS + " a um usuário.\n········································································\n");
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return;
            }

            GameClient Target = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Uau, não conseguiu encontrar esse usuário!");
                return;
            }

            string UpdateVal = Params[2];
            switch (UpdateVal.ToLower())
            {
                case "coins":
                case "credits":
                case "creditos":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_coins"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        else
                        {
                            int Amount;
                            if (int.TryParse(Params[3], out Amount))
                            {
                                Target.GetHabbo().Credits = Target.GetHabbo().Credits += Amount;
                                Target.SendMessage(new CreditBalanceComposer(Target.GetHabbo().Credits));

                                Session.SendMessage(new RoomNotificationComposer("tickets", "message", "Você enviou, " + Amount + " crédito(s) a " + Target.GetHabbo().Username + "!"));
                                Target.SendMessage(new RoomNotificationComposer("cred", "message", "Você recebeu " + Amount + " crédito(s) de " + Session.GetHabbo().Username + "!"));
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                                break;
                            }
                        }
                    }

                case "pixels":
                case "duckets":
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_pixels"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        else
                        {
                            int Amount;
                            if (int.TryParse(Params[3], out Amount))
                            {
                                Target.GetHabbo().Duckets += Amount;
                                Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Duckets, Amount));

                                Session.SendMessage(new RoomNotificationComposer("tickets", "message", "Você enviou, " + Amount + " ducket(s) a " + Target.GetHabbo().Username + "!"));
                                Target.SendMessage(new RoomNotificationComposer("duckets", "message", "Você recebeu " + Amount + " ducket(s) de " + Session.GetHabbo().Username + "!"));
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                                break;
                            }
                        }

                case "diamonds":
                case "diamantes":
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_diamonds"))
                        {
                            Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                            break;
                        }
                        else
                        {
                            int Amount;
                            if (int.TryParse(Params[3], out Amount))
                            {
                                Target.GetHabbo().Diamonds += Amount;
                            Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Diamonds, 0, 5));
                            Session.SendMessage(new RoomNotificationComposer("tickets", "message", "Você enviou, " + Amount + " diamante(s) a " + Target.GetHabbo().Username + "!"));
                                Target.SendMessage(new RoomNotificationComposer("diamonds", "message", "Você recebeu " + Amount + " diamante(s) de " + Session.GetHabbo().Username + "!"));
                                break;
                            }
                            else
                            {
                                Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                                break;
                            }
                        }

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
                        else
                        {
                            int Amount;
                        if (int.TryParse(Params[3], out Amount))
                        {
                            if (Amount > 500)
                            {
                                Session.SendWhisper("Não podem enviar mais de 500 pontos, isso será notificado ao CEO e as ações apropriadas serão tomadas.");
                                return;
                            }

                            Target.GetHabbo().GOTWPoints = Target.GetHabbo().GOTWPoints + Amount;
                            Target.GetHabbo().UserPoints = Target.GetHabbo().UserPoints + 1;
                            Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().GOTWPoints, Amount, 103));

                            if (Target.GetHabbo().Id != Session.GetHabbo().Id)
                                Target.SendMessage(RoomNotificationComposer.SendBubble("moedas", "" + Session.GetHabbo().Username + " enviou a você " + Amount + " " + Core.ExtraSettings.PTOS_COINS + ".\nClique para ver os prêmios disponíveis.", "catalog/open/gotws"));
                            Session.SendMessage(RoomNotificationComposer.SendBubble("furni_placement_error", "Você acabou de enviar " + Amount + " " + Core.ExtraSettings.PTOS_COINS + " " + Target.GetHabbo().Username + "\nLembre - se de que depositamos sua confiança em você e que esses comandos são vistos ao vivo.", "catalog/open/gotws"));
                            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Target, "ACH_EventsWon", 1);
                            break;
                        }
                        else
                        {
                            Session.SendWhisper("Você só pode inserir parâmetros numéricos, de 1 a 50.");
                            break;
                        }
                    }
                case "gotwt":
                case "gotwpointst":
                case "famet":
                case "famat":
                case "famepointst":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_give_gotw"))
                    {
                        Session.SendWhisper("Uau, parece que você não tem as permissões necessárias para usar esse comando!");
                        break;
                    }
                    else
                    {
                        int Amount;
                        if (int.TryParse(Params[3], out Amount))
                        {
                            Target.GetHabbo().GOTWPoints = Target.GetHabbo().GOTWPoints + Amount;
                            Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().GOTWPoints, Amount, 103));

                            Session.SendMessage(new RoomNotificationComposer("tickets", "message", "Você enviou, " + Amount + " " + Core.ExtraSettings.PTOS_COINS + " a " + Target.GetHabbo().Username + "!"));
                            Target.SendMessage(new RoomNotificationComposer("moedas", "message", "Você recebeu " + Amount + " " + Core.ExtraSettings.PTOS_COINS + " de " + Session.GetHabbo().Username + "!"));
                            break;
                        }
                        else
                        {
                            Session.SendWhisper("Uau, isso parece ser um valor inválido!");
                            break;
                        }
                    }
                default:
                    Session.SendWhisper("'" + UpdateVal + "' no es una moneda válida!");
                    break;
            }
        }
    }
}