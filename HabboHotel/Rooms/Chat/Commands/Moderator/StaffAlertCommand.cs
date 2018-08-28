using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class StaffAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_staff_alert";
        public string Parameters => "[MENSAGE]";
        public string Description => "Enviar mensage a todos os staff.";

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
            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite uma mensagem para enviar.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            BiosEmuThiago.GetGame().GetClientManager().StaffAlert(new MOTDNotificationComposer("Mensage da Equipe Staff:\r\r" + Message + "\r\n" + "- " + Session.GetHabbo().Username));
            return;
        }
    }
}