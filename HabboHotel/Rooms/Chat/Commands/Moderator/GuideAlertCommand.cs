using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class GuideAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_guide_alert";
        public string Parameters => "[MENSAGE]";
        public string Description => "Envie uma mensagem de alerta para todos os funcionários on-line.";

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
            if (Session.GetHabbo()._guidelevel < 1)
            {
                Session.SendWhisper("Você não pode enviar alertas para guias, se não estiver rank.");
                return;
              
            }
            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite a mensagem que deseja enviar.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            BiosEmuThiago.GetGame().GetClientManager().GuideAlert(new MOTDNotificationComposer("[GUÍAS][" + Session.GetHabbo().Username + "]\r\r" + Message));
            return;
        }
    }
}