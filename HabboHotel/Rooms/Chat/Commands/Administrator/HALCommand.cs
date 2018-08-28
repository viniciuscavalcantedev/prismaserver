using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class HALCommand : IChatCommand
    {
        public string PermissionRequired => "command_hal";
        public string Parameters => "[MENSAJE] [URL]";
        public string Description => "Enviar mensagem com link";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
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
            if (Params.Length == 2)
            {
                Session.SendWhisper("Por favor, escreva uma mensagem e uma URL para enviar.");
                return;
            }

            string URL = Params[2];
            string Message = CommandManager.MergeParams(Params, 2);
            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Alerta do Hotel!", Params[1] + "\r\n" + "- " + Session.GetHabbo().Username, "", URL, URL));
            return;
        }
    }
}
