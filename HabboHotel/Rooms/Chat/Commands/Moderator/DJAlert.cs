using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class DJAlert : IChatCommand
    {
        public string PermissionRequired => "command_djalert";
        public string Parameters => "[MENSAGE]";
        public string Description => "Envie um alerta para todo o hotel DJ LIVE.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Escreva a mensagem para enviar");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            BiosEmuThiago.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("DJAlertNEW", "DJ " + Message + " É transmitido ao vivo! linha " + BiosEmuThiago.HotelName+ "FM agora e aproveite ao máximo.", ""));
            return;
        }
    }
}
