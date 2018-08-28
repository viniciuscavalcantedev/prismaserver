//             (            (          )            *   )   )                     (           
//           )\  (     (  )\ ) (  ( /(          ` )  /(( /((     ) (  (         )\   (      )   (  (       
//           (((_) )(   ))\(()/( )\ )\())(  (      ( )(_))\())\ ( /( )\))( (   ((((_)( )(  ( /(  ))\ )\  (   
//           )\___(()\ /((_)((_))(_)_))/ )\ )\ _  (_(_())(_)((_))(_))(_))\ )\   )\ _ )(()\ )(_))/((_)(_) )\  
//           ((/ __|((_)_))  _| | (_) |_ ((_)(_)_) |_   _| |(_)_)(_)_ (()(_)(_)  (_)_\(_)(_)(_)_(_))(  ! ((_) 
//           | (__| '_/ -_) _` | | |  _/ _ (_-<_    | | | ' \| / _` / _` / _ \   / _ \| '_/ _` | || || / _ \ 
//            \___|_| \___\__,_| |_|\__\___/__(_)   |_| |_||_|_\__,_\__, \___/  /_/ \_\_| \__,_|\_,_|/ \___/ 
//                                                        |___/                          |__/      
//                        © 2016 - 2017 CoreDev Corporation Ltd. Todos os direitos reservados.
using System;
using Bios.Core;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class InfoCommand : IChatCommand
    {
        public string PermissionRequired => "command_pickall";
        public string Parameters => "";
        public string Description => "Mostra as informações do BiosEmulador";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            TimeSpan Uptime = DateTime.Now - BiosEmuThiago.ServerStarted;
            int OnlineUsers = BiosEmuThiago.GetGame().GetClientManager().Count;
            int RoomCount = BiosEmuThiago.GetGame().GetRoomManager().Count;

            Session.SendMessage(new RoomNotificationComposer("BiosEmulador O melhor da tecnologia:",
                 "<font color=\"#0653b4\"><b>Infomações do BiosEmulador:</b></font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">BiosEmulador para " + BiosEmuThiago.HotelName + " </font>" +
                 "<font size=\"11\" color=\"#1C1C1C\">BiosEmulador 1.0 contém todas as características necessárias para um hotel estável. A edição 1.0 foi editada com o que há de melhor para este emulador.</font>\n\n" +
                 "<font color=\"#0653b4\" size=\"13\"><b>Informações:</b></font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Usuários: </b> " + OnlineUsers + "</font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Quartos: </b> " + RoomCount + "</font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Tempo On: </b> " + Uptime.Days + " día(s), " + Uptime.Hours + " horas é " + Uptime.Minutes + " minutos.</font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Update: </b> " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + "</font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Recorde da ultima atualização: </b>  " + Game.SessionUserRecord + "</font>\n\n" +
                 "<font color=\"#0653b4\" size=\"13\"><b>Créditos:</b></font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Happy - </b> Thiago Araujo (DEVEMU)</font>\n" +
                 "<font size=\"11\" color=\"#1C1C1C\">  <b> · Sain - </b> Messias (DEVSWF) </font>\n\n" +
                 "<font color=\"#0077d2\">Licencia:  <b>" + BiosEmuThiago.Licenseto + "</b></font>\n\n", "biostecthiago", ""));
        }
    }
}
