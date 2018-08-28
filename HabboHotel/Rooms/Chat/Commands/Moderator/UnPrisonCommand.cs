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

using Bios.Utilities;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.GameClients;


using Bios.HabboHotel.Moderation;

using Bios.Database.Interfaces;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class UnPrisonCommand : IChatCommand
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
            get { return "Desprender um usuário"; ; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Escolha um jogador para remover da cadeia");
                return;
            }

            GameClient TargetClient = null;
            TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);

            if (TargetClient.GetHabbo().Id == Session.GetHabbo().Id)
            {
                Session.SendWhisper("Você não pode ser remover!");
            }

            if (TargetClient.GetHabbo().Username == null)
            {
                Session.SendWhisper("O usuário não existe!");
                return;
            }

            if (TargetClient.GetHabbo().GetPermissions().HasRight("mod_soft_ban") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Você não pode remover este usuário!");
                return;
            }

            // Comando editaveu abaixo mais cuidado pra não faze merda

            if (TargetClient.GetHabbo().Id != Session.GetHabbo().Id && Session.GetHabbo().isOfficer)
            {
                using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE users SET prefix_name = 'LIVRE' WHERE id = '" + TargetClient.GetHabbo().Id + "'");
                    dbClient.RunQuery("UPDATE users SET prefix_name_color = '#30ba50' WHERE id = '" + TargetClient.GetHabbo().Id + "'");

                    Session.GetHabbo()._NamePrefixColor = "#30ba50";
                    Session.GetHabbo()._NamePrefix = "LIVRE";
                    TargetClient.SendWhisper("Tag PRISONEIRO foi desativada é foi ligada a LIVRE!");

                    dbClient.RunQuery("UPDATE users SET prison = '0' WHERE ID ='" + TargetClient.GetHabbo().Id + "' LIMIT 1");
                    dbClient.RunQuery("UPDATE `users` SET `home_room` = '0' WHERE `home_room` = '" + TargetClient.GetHabbo().Id + "'");
                    dbClient.RunQuery("UPDATE users SET Presidio = 'false' WHERE id = " + TargetClient.GetHabbo().Id + ";");
                    dbClient.RunQuery("UPDATE users SET motto = 'Agora esto livre da Prisão do hotel!' WHERE ID = '" + TargetClient.GetHabbo().Id + "' LIMIT 1");
                    dbClient.AddParameter("updateU", TargetClient.GetHabbo().Id);
                    dbClient.AddParameter("updateU1", TargetClient.GetHabbo().Id);
                    dbClient.AddParameter("updateU2", TargetClient.GetHabbo().Id);

                    Session.SendWhisper("Removido " + TargetClient.GetHabbo().Username + " Da prisão!");

                    string figure = TargetClient.GetHabbo().Look;
                    BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figure, 3, "O Usuário " + Params[1] + " saiu da prisão!", ""));
                }
            }
            else
            {
                Session.SendWhisper("Você não pode usar este comandos ou você não está no modo policial!");
            }
        }
    }
}