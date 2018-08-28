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

using Bios.Utilities;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.GameClients;
using Bios.Database.Interfaces;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Communication.Packets.Outgoing.Catalog;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class MegaOferta : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_update"; }
        }

        public string Parameters
        {
            get { return "%LIGAR% ou %DESLIGAR%"; }
        }

        public string Description
        {
            get { return "Ligar ou desligar uma mega oferta."; ; }
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
                Session.SendMessage(new RoomNotificationComposer("erro", "message", "Ops, você deve digita assim: ':megaoferta ligar ou :megaoferta desligar'!"));
                return;
            }

            if (Params[1] == "ligar")
            {
                // Comando editaveu abaixo mais cuidado pra não faze merda
                using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE targeted_offers SET active = 'true' WHERE active = 'false'");
                    dbClient.RunQuery("UPDATE users SET targeted_buy = '0'");
                }
                BiosEmuThiago.GetGame().GetTargetedOffersManager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("volada", "message", "Corre, nova mega oferta foi colocada!"));
                Session.SendWhisper("Nova mega oferta iniciada!");
            }

            if (Params[1] == "desligar")
            {
                // Comando editaveu abaixo mais cuidado pra não faze merda
                using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.RunQuery("UPDATE targeted_offers SET active = 'false' WHERE active = 'true'");
                    dbClient.RunQuery("UPDATE users SET targeted_buy = '0'");
                }
                BiosEmuThiago.GetGame().GetTargetedOffersManager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("ADM", "message", "Que pena, a mega oferta foi retirada!"));
                Session.SendWhisper("Mega oferta retirada!");
            }

            if (Params[1] != "ligar" || Params[1] != "desligar")
            {
                Session.SendMessage(new RoomNotificationComposer("erro", "message", "Ops, você deve digita assim: ':megaoferta ligar ou :megaoferta desligar'!"));
            }
        }
    }
}
