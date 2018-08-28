using Bios.Core;
using Bios.HabboHotel.GameClients;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class EmptyUser : IChatCommand
    {
        public string PermissionRequired => "command_emptyuser";
        public string Parameters => "[USUARIO]";
        public string Description => "Limpar o inventario de um usúario";

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
                Session.SendWhisper("Escreva o nome do usúario que você deseja limpar.");
                return;
            }

            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient == null)
            {
                Session.SendWhisper("¡Oops! Provavelmente o usuário não está online.");
                return;
            }

            if (TargetClient.GetHabbo().Rank >= Session.GetHabbo().Rank)
            {
                Session.SendWhisper("Você não pode limpar o inventário desse usuário.");
                return;
            }

            TargetClient.GetHabbo().GetInventoryComponent().ClearItems();
        }
    }
}