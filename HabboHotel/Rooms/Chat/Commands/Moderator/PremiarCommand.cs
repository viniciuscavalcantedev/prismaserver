//             (            (          )            *   )   )                     (           
//           )\  (     (  )\ ) (  ( /(          ` )  /(( /((     ) (  (         )\   (      )   (  (       
//           (((_) )(   ))\(()/( )\ )\())(  (      ( )(_))\())\ ( /( )\))( (   ((((_)( )(  ( /(  ))\ )\  (   
//           )\___(()\ /((_)((_))(_)_))/ )\ )\ _  (_(_())(_)((_))(_))(_))\ )\   )\ _ )(()\ )(_))/((_)(_) )\  
//           ((/ __|((_)_))  _| | (_) |_ ((_)(_)_) |_   _| |(_)_)(_)_ (()(_)(_)  (_)_\(_)(_)(_)_(_))(  ! ((_) 
//           | (__| '_/ -_) _` | | |  _/ _ (_-<_    | | | ' \| / _` / _` / _ \   / _ \| '_/ _` | || || / _ \ 
//            \___|_| \___\__,_| |_|\__\___/__(_)   |_| |_||_|_\__,_\__, \___/  /_/ \_\_| \__,_|\_,_|/ \___/ 
//                                                        |___/                          |__/      
//                        © 2016 - 2017 SaoDev Corporation Ltd. Todos os direitos reservados.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Pathfinding;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Chat;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Database.Interfaces;
using System.Data;
using Bios.Communication.Packets.Outgoing.Users;
using Bios.HabboHotel.Quests;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class PremiarCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_alert_user"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Faz todas as funções para premia um ganahdor de evento."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
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
                Session.SendWhisper("Por favor, digite o usuário que deseja premiar!");
                return;
            }

            GameClient Target = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Opa, não foi possível encontrar esse usuário!");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Target.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Usuário não encontrado! Talvez ele não esteja online ou nesta sala.");
                return;
            }

            if (Target.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pode se premiar!");
                return;
            }

            // Comando editaveu abaixo mais cuidado pra não faze merda

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (ThisUser == null)
                {
                    return;
                }
                else
                {

                    // Parte da Moedas by: Thiago Araujo
                    Target.GetHabbo().Credits = Target.GetHabbo().Credits += Convert.ToInt32(BiosEmuThiago.GetConfig().data["Moedaspremiar"]);
                    Target.SendMessage(new CreditBalanceComposer(Target.GetHabbo().Credits));
                    Target.GetHabbo().Duckets += Convert.ToInt32(BiosEmuThiago.GetConfig().data["Ducketspremiar"]);
                    Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Duckets, 500));
                    Target.GetHabbo().Diamonds += Convert.ToInt32(BiosEmuThiago.GetConfig().data["Diamantespremiar"]);
                    Target.SendMessage(new HabboActivityPointNotificationComposer(Target.GetHabbo().Diamonds, 1, 5));

                    // MEnsagem pro ganhador
                    Target.SendMessage(new RoomNotificationComposer("moedas", "message", "Você ganhou " + Convert.ToInt32(BiosEmuThiago.GetConfig().data["Ducketspremiar"]) + " Ducket(s)! " + Convert.ToInt32(BiosEmuThiago.GetConfig().data["Moedaspremiar"]) + " Credito(s) " + Convert.ToInt32(BiosEmuThiago.GetConfig().data["Diamantespremiar"]) + " Diamante(s) parabéns " + Target.GetHabbo().Username + "!"));

                    // Sistema de entra o mobi pro ganhador by: Thiago Araujo
                    if (Target.GetHabbo().Rank >= 0)
                    {
                        DataRow dFurni = null;
                        using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT public_name FROM furniture WHERE id = '42636366'");
                            dFurni = dbClient.getRow();
                        }
                        Target.GetHabbo().GetInventoryComponent().AddNewItem(0, 42636366, Convert.ToString(dFurni["public_name"]), 1, true, false, 0, 0);
                        Target.GetHabbo().GetInventoryComponent().UpdateItems(false);

                    }

                    if (Session.GetHabbo().Rank >= 0)
                    {
                        DataRow nivel = null;
                        using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT Premiar FROM users WHERE id = '" + Target.GetHabbo().Id + "'");
                            nivel = dbClient.getRow();
                            dbClient.RunQuery("UPDATE users SET Premiar = Premiar + '1' WHERE id = '" + Target.GetHabbo().Id + "'");
                            dbClient.RunQuery("UPDATE users SET pontos_evento = pontos_evento + '1' WHERE id = '" + Target.GetHabbo().Id + "'");
                            dbClient.RunQuery();
                        }
                        // Parte da codificação do dar emblema by: Thiago Araujo
                        if (Convert.ToString(nivel["Premiar"]) != BiosEmuThiago.GetConfig().data["NiveltotalGames"])
                        {
                            string emblegama = "NV" + Convert.ToString(nivel["Premiar"]);

                        if (!Target.GetHabbo().GetBadgeComponent().HasBadge(emblegama))
                            {
                                Target.GetHabbo().GetBadgeComponent().GiveBadge(emblegama, true, Target);
                                if (Target.GetHabbo().Id != Session.GetHabbo().Id)
                                    Target.SendMessage(new RoomNotificationComposer("badge/" + emblegama, 3, "Você acaba de receber um emblema game de nivel: " + emblegama + " !", ""));
                                // Parte da Quest do evento  by: Thiago Araujo
                                BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Target, "ACH_Evento", 1);
                            // Parte da notificação com cabeça do usuário by: Thiago Araujo
                            string figure = Target.GetHabbo().Look;
                                BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figure, 3, TargetUser.GetUsername() + " ganhou um evento no hotel. Parabéns!", " Nivel de emblema game: NIVEL " + Convert.ToString(nivel["Premiar"]) + " !"));
                            }
                            else
                                Session.SendWhisper("Ops, ocorreu um erro no sistema de dar emblemas automáticos! Erro no emblema: (" + emblegama + ") !");
                            // Mensagem de finalização do evento pro staff
                            Session.SendWhisper("Comando (Premiar) realizado com sucesso!");
                        }
                   }
              }
        }

        private void SendMessage(RoomNotificationComposer roomNotificationComposer)
        {
            throw new NotImplementedException();
        }
    }
}
