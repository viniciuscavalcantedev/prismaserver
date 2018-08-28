using Bios.Core;
using Bios.HabboHotel.GameClients;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class GOTOCommand : IChatCommand
    {
        public string PermissionRequired => "command_goto";
        public string Parameters => "[IDSALA]";
        public string Description => "Ir a uma sala.";

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
                Session.SendWhisper("Você deve especificar uma ID do quarto!");
                return;
            }

            int roomId = 0;
            if (!int.TryParse(Params[1], out roomId))
            {
                Session.SendWhisper("Você deve inserir uma ID de quarto válida");
            }
            else
            {
                Room room = null;
                if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(roomId, out room))
                {
                    Session.SendWhisper("Este quarto não existe!");
                    return;
                }

                Session.GetHabbo().PrepareRoom(room.Id, "");
            }
        }
    }
}