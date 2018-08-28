using Bios.Core;
using Bios.HabboHotel.GameClients;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class AlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_alert_user";
        public string Parameters => "[USUARIO] [MENSAJE]";
        public string Description => "Enviar alerta a um usuário.";

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
                Session.SendWhisper("Digite o nome do usuário que deseja alertar.");
                return;
            }

            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez eles não estejam online.");
                return;
            }

            if (TargetClient.GetHabbo() == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez eles não estejam online.");
                return;
            }

            if (TargetClient.GetHabbo().Username == Session.GetHabbo().Username)
            {
                Session.SendWhisper("Você não pode manda alerta para você mesmo!");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 2);

            TargetClient.SendNotification(Session.GetHabbo().Username + " alertou você com a seguinte mensagem:\n\n" + Message);
            Session.SendWhisper("Alerta enviada com sucesso para " + TargetClient.GetHabbo().Username);

        }
    }
}
