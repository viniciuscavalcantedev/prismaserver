using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Handshake;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class FlagUserCommand : IChatCommand
    {
        public string PermissionRequired => "command_flaguser";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Renomeie um usuário.";

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
                Session.SendWhisper("Digite o nome de usuário que deseja alterar.");
                return;
            }

            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário, talvez eles não estejam online.");
                return;
            }

            if (TargetClient.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                Session.SendWhisper("O usuário não tem permissão para marcá-lo.");
                return;
            }
            else
            {
                TargetClient.GetHabbo().LastNameChange = 0;
                TargetClient.GetHabbo().ChangingName = true;
                TargetClient.SendNotification("Por favor, note que, se seu nome de usuário for considerado inapropriado, você será banido sem dúvida. \r\rObserve também que a equipe não permitirá que você altere seu nome de usuário novamente caso tenha um problema com o que você tem escolhido.\r\rFeche esta janela e clique em você mesmo para começar a escolher um novo nome de usuário!");
                TargetClient.SendMessage(new UserObjectComposer(TargetClient.GetHabbo()));
            }

        }
    }
}
