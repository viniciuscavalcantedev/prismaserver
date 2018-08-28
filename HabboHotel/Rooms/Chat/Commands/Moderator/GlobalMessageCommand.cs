using System.Linq;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.GameClients;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class GlobalMessageCommand : IChatCommand
    {
        public string PermissionRequired => "command_alert_user";
        public string Parameters => "[MENSAGE]";
        public string Description => "Enviar alerta 'BUBBLE' global";

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
                Session.SendWhisper("Digite a mensagem.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            foreach (GameClient client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
            {
                client.SendMessage(new RoomNotificationComposer("command_gmessage", "message", "" + Message + "!"));
            }
        }
    }
}
