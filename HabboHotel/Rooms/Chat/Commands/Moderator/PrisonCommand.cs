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
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Bios.Database.Interfaces;

using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Session;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Communication.Packets.Outgoing.Rooms.Engine;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class PrisonCommand : IChatCommand
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
            get { return "Prender um jogador"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);

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
                Session.SendWhisper("Escolha o usuário que você quer prender!");
                return;
            }

            if (TargetClient.GetHabbo().Id == Session.GetHabbo().Id)
            {
                Session.SendWhisper("Você não pode prender-se!");
            }

            if (TargetClient.GetHabbo().Username == null)
            {
                Session.SendWhisper("O usuário não existe!");
                return;
            }

            if (TargetClient.GetHabbo().GetPermissions().HasRight("mod_soft_ban") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Você não pode prender esse usuário!");
                return;
            }

            if (Session.GetHabbo().Rank < TargetClient.GetHabbo().Rank)
            {
                Session.SendWhisper("Você não pode prender esse usuário! (O cargo desse usuário é maio que o seu!)");
                return;
            }

            Random rnd = new Random();
            int prisonerClothes = rnd.Next(1, 2);

            if (TargetClient.GetHabbo().Id != Session.GetHabbo().Id && Session.GetHabbo().isOfficer)
            {

                using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT ip_reg FROM users WHERE id=' " + TargetClient.GetHabbo().Id + "' LIMIT 1");
                    dbClient.AddParameter("selectt", TargetClient.GetHabbo().Id);
                    string ipreg = dbClient.ToString();

                    dbClient.SetQuery("SELECT ip_reg FROM users where ip_reg='" + ipreg + "' LIMTI 1");
                    dbClient.AddParameter("select", TargetClient.GetHabbo().Id);
                    string ipMatch = dbClient.ToString();



                    // Comando editaveu abaixo mais cuidado pra não faze merda

                    if (ipreg == ipMatch)
                    {
                        dbClient.RunQuery("UPDATE users SET prison ='1' WHERE ip_reg='" + ipMatch + "' LIMIT 1");
                        dbClient.AddParameter("updateP", TargetClient.GetHabbo().Id);
                    }

                    string figure = TargetClient.GetHabbo().Look;
                    BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figure, 3, "O Usuário " + Params[1] + " foi preso!", ""));

                    dbClient.RunQuery("UPDATE users SET prefix_name = 'PRISONEIRO' WHERE id = '" + TargetClient.GetHabbo().Id + "'");
                    dbClient.RunQuery("UPDATE users SET prefix_name_color = '#dc335b' WHERE id = '" + TargetClient.GetHabbo().Id + "'");

                    TargetClient.GetHabbo()._NamePrefixColor = "#dc335b";
                    TargetClient.GetHabbo()._NamePrefix = "PRESO";
                    TargetClient.SendWhisper("Tag PRISONEIRO foi ativada!");

                    dbClient.RunQuery("UPDATE users SET prison = '1' WHERE ID ='" + TargetClient.GetHabbo().Id + "' LIMIT 1");
                    dbClient.RunQuery("UPDATE `users` SET `home_room` = '12' WHERE id ='" + TargetClient.GetHabbo().Id + "' LIMIT 1");
                    dbClient.RunQuery("UPDATE users SET Presidio = 'true' WHERE id = " + TargetClient.GetHabbo().Id + ";");
                    dbClient.RunQuery("UPDATE users SET motto = 'Eu sou um presidiário do Habbz Hotel!' WHERE ID = '" + TargetClient.GetHabbo().Id + "' LIMIT 1");
                    dbClient.AddParameter("updateU", TargetClient.GetHabbo().Id);
                    dbClient.AddParameter("updateU1", TargetClient.GetHabbo().Id);
                    dbClient.AddParameter("updateU2", TargetClient.GetHabbo().Id);

                    // Comando feito por Thiago Araujo: Servidores de SAO.

                    Session.SendWhisper("Você prendeu o jogador com exito!");
                    TargetClient.SendWhisper("Você foi preso é sera reiniciado em alguns minutos.");

                    TargetClient.GetHabbo().Gender = TargetClient.GetHabbo().Gender;
                    TargetClient.GetHabbo().Look = "fa-568282-1195.lg-270-70.cp-3125-70.hd-195-1.ca-5840877-62.ch-210-70.sh-290-1189.hr-3163-1035";

                    if (Session.GetHabbo().Rank > 0)
                    {

                        dbClient.SetQuery("UPDATE `users` SET `gender` = @gender, `look` = @look WHERE `id` = @id LIMIT 1");
                        dbClient.AddParameter("gender", TargetClient.GetHabbo().Gender);
                        dbClient.AddParameter("look", TargetClient.GetHabbo().Look);
                        dbClient.AddParameter("id", TargetClient.GetHabbo().Id);
                        dbClient.RunQuery();

                        RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                        if (User != null)
                        {
                            Session.SendMessage(new UserChangeComposer(User, true));
                            Room.SendMessage(new UserChangeComposer(User, false));
                        }

                    }

                }
                Session.SendWhisper(TargetClient.GetHabbo().Username + " Está agora preso e foi enviado para a prisão!");

                if (!TargetClient.GetHabbo().InRoom)
                    TargetClient.SendMessage(new RoomForwardComposer(BiosEmuThiago.Prisao));
                else
                    TargetClient.GetHabbo().PrepareRoom(BiosEmuThiago.Prisao, "");
            }
            else
            {
                Session.SendWhisper("Você não tem acesso a isso, ou você não está no modo policial!");
            }
        }
    }
}