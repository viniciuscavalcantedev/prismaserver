using Bios.Core;
using Bios.HabboHotel.GameClients;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class KickCommand : IChatCommand
    {
        public string PermissionRequired => "command_kick";
        public string Parameters => "[USUÁRIO] [MENSAGE]";
        public string Description => "Expulse o usuário e envie o motivo.";

        public void Execute(GameClient Session, Room Room, string[] Params)
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
                Session.SendWhisper("Digite o nome do usuário que deseja kicka.");
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
                Session.SendWhisper("Você não pode kick você mesmo!");
                return;
            }

            if (!TargetClient.GetHabbo().InRoom)
            {
                Session.SendWhisper("Este usuário não está atualmente em uma sala.");
                return;
            }

            Room TargetRoom;
            if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(TargetClient.GetHabbo().CurrentRoomId, out TargetRoom))
                return;

            if (Params.Length > 2)
                TargetClient.SendNotification("Um moderador expulsou você da sala pelo seguinte motivo: " + CommandManager.MergeParams(Params, 2));
            else
                TargetClient.SendNotification("Um moderador expulsou você da sala.");

            TargetRoom.GetRoomUserManager().RemoveUserFromRoom(TargetClient, true, false);
        }
    }
}
