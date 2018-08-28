using Bios.Communication.Packets.Outgoing;
//             (            (          )            *   )   )                     (           
//           )\  (     (  )\ ) (  ( /(          ` )  /(( /((     ) (  (         )\   (      )   (  (       
//           (((_) )(   ))\(()/( )\ )\())(  (      ( )(_))\())\ ( /( )\))( (   ((((_)( )(  ( /(  ))\ )\  (   
//           )\___(()\ /((_)((_))(_)_))/ )\ )\ _  (_(_())(_)((_))(_))(_))\ )\   )\ _ )(()\ )(_))/((_)(_) )\  
//           ((/ __|((_)_))  _| | (_) |_ ((_)(_)_) |_   _| |(_)_)(_)_ (()(_)(_)  (_)_\(_)(_)(_)_(_))(  ! ((_) 
//           | (__| '_/ -_) _` | | |  _/ _ (_-<_    | | | ' \| / _` / _` / _ \   / _ \| '_/ _` | || || / _ \ 
//            \___|_| \___\__,_| |_|\__\___/__(_)   |_| |_||_|_\__,_\__, \___/  /_/ \_\_| \__,_|\_,_|/ \___/ 
//                                                        |___/                          |__/      
//                        © 2016 - 2017 SaoDev Corporation Ltd. Todos os direitos reservados.
using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Communication.Packets.Outgoing.Rooms.Chat;
using Bios.Communication.Packets.Outgoing.Rooms.Engine;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class EmojiCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }
        public string Parameters
        {
            get { return ""; }
        }
        public string Description
        {
            get { return "Mande um emoji no chat do hotel! 1 a 189 emojis"; }
        }
        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Opa, digite um número 1-189! Para ver a lista de gravações emoji :emoji lista");
                return;
            }
            string emoji = Params[1];

            if (emoji.Equals("lista"))
            {
                ServerPacket notif = new ServerPacket(ServerPacketHeader.NuxAlertMessageComposer);
                notif.WriteString("habbopages/chat/emoji.txt");
                Session.SendMessage(notif);
            }
            else
            {
                int emojiNum;
                bool isNumeric = int.TryParse(emoji, out emojiNum);
                if (isNumeric)
                {
                    switch (emojiNum)
                    {
                        default:
                            bool isValid = true;
                            if (emojiNum < 1)
                            {
                                isValid = false;
                            }

                            if (emojiNum > 189 && Session.GetHabbo().Rank < 6)
                            {
                                isValid = false;
                            }

                            // Comando editaveu abaixo mais cuidado pra não faze merda
                            if (isValid)
                            {
                                string Username;
                                RoomUser TargetUser = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);
                                if (emojiNum < 10)
                                {
                                    Username = "<img src='/swf/c_images/emoji/sao/thiago/Emoji Smiley-0" + emojiNum + ".png' height='20' width='20'><br>    >";
                                }
                                else
                                {
                                    Username = "<img src='/swf/c_images/emoji/sao/thiago/Emoji Smiley-" + emojiNum + ".png' height='20' width='20'><br>    >";
                                }
                                if (Room != null)
                                    Room.SendMessage(new UserNameChangeComposer(Session.GetHabbo().CurrentRoomId, TargetUser.VirtualId, Username));

                                string Message = " ";
                                Room.SendMessage(new ChatComposer(TargetUser.VirtualId, Message, 0, TargetUser.LastBubble));
                                TargetUser.SendNamePacket();

                            }
                            else
                            {
                                Session.SendWhisper("Emoji inválido, deve ser 1-189 número. Para ver a lista de digite escreve: ':emoji lista'");
                            }

                            break;
                    }
                }
                else
                {
                    Session.SendWhisper("Emoji inválido, deve ser 1-189 número. Para ver a lista de Emoji digite ':emoji lista'");
                }
            }
        }
    }
}
