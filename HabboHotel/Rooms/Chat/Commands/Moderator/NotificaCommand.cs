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
using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class NotificaCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_alert_user"; }
        }

        public string Parameters
        {
            get { return "[NOTIFICAÇÃO]"; }
        }

        public string Description
        {
            get { return "Envia uma notificação a todos os usuários."; }
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
                    Session.SendWhisper("Opa, você deve escolher o modelo da notificação(;notifica lista) que você planeja usar!");
                    return;
                }
                string notificathiago = Params[1];
                string Colour = notificathiago.ToUpper();
                switch (notificathiago)
                {
                    // Comando editaveu abaixo mais cuidado pra não faze merda
                    case "lista":
                    case "modelos":
                    case "heapp":
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append("Lista de notificações \r");
                        stringBuilder.Append("------------------------------------------------------------------------------\r");
                        stringBuilder.Append(":notifica normal texto / notifica com a imagem do microfone                            ");
                        stringBuilder.Append(":notifica custom texto / notifica com seu boneco na imagem                             ");
                        stringBuilder.Append(":notifica emoji texto  / notifica com emoji escolhido                                  ");
                        stringBuilder.Append(":notifica link texto   / notifica um site                                              ");
                        stringBuilder.Append(":notifica quarto texto / notifica usuário é levado ao quarto quanto clicka             ");
                        stringBuilder.Append("------------------------------------------------------------------------------\r");
                        stringBuilder.Append("Creditos ao Thiago Happy SofKing \r");
                        Session.SendMessage(new MOTDNotificationComposer(stringBuilder.ToString()));
                        break;

                    case "normal":
                    case "comum":
                    case "micro":
                        string Message = CommandManager.MergeParams(Params, 2);

                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("micro", "message", "" + Message + ""));
                        break;

                    case "custom":
                    case "novo":
                    case "cabeça":
                        string Messagecustom = CommandManager.MergeParams(Params, 2);

                        string figure = Session.GetHabbo().Look;
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figure, 3, "" + Messagecustom + "", ""));
                        break;

                    case "quarto":
                    case "seguir":
                    case "ir":
                        string Messageseguir = CommandManager.MergeParams(Params, 2);
                        string Messageseguirs = CommandManager.MergeParams(Params, 3);

                        string figureseguir = Session.GetHabbo().Look;
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("fig/" + figureseguir, 3, Messageseguir + " n/ @Click para ir!@", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
                        break;

                    case "link":
                    case "http":
                    case "url":
                        string URL = Params[4];
                        string Messagelink = CommandManager.MergeParams(Params, 2);

                        string figureurl = Session.GetHabbo().Look;
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("micro", "message", "" + Messagelink + "", URL, URL));
                        break;

                    case "imagem":
                    case "foto":
                    case "emoji":
                        string Messageimagem = CommandManager.MergeParams(Params, 2);
                        string Messageimagems = CommandManager.MergeParams(Params, 3);

                        string figureimagem = Session.GetHabbo().Look;
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("<img src='/swf/c_images/emoji/sao/Emoji Smiley-" + Messageimagems + ".png' height='20' width='20'><br>    >", 3, "" + Messageimagem, ""));
                        break;

                    case "emblema":
                    case "git":
                    case "emb":
                        string Messageemblema = Params[2];
                        string Messageemblemas = Params[3];

                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("badge/" + Messageemblemas, 3, "" + Messageemblema + "", ""));
                        break;
                }
        }
    }
}
