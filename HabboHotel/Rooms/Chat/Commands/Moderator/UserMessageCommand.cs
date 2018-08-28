using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;
using Bios.HabboHotel.GameClients;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class UserMessageCommand : IChatCommand
    {
        public string PermissionRequired => "command_alert_user";
        public string Parameters => "[USUÁRIO] [MENSAGE]";
        public string Description => "Enviar mensagem para um usuário.";

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
                Session.SendWhisper("Digite o nome de usuário do usuário.");
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
                Session.SendWhisper("Você não pdoe manda uma mensagem para você mesmo.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 2);

            TargetClient.SendMessage(new RoomNotificationComposer("command_gmessage", "message", "" + Message + "!"));
            Session.SendMessage(new RoomNotificationComposer("command_gmessage", "message", "Mensagem enviada com sucesso para " + TargetClient.GetHabbo().Username));
        }
    }
}
