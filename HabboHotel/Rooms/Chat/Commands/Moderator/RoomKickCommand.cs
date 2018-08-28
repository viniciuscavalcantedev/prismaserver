using Bios.Core;
using System;
using System.Linq;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class RoomKickCommand : IChatCommand
    {
        public string PermissionRequired => "command_room_kick";
        public string Parameters => "[MENSAGE]";
        public string Description => "Kicka todos os usuários nessa sala.";

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
                Session.SendWhisper("Por favor, forneça um motivo kick todos da sala.");
                return;
            }

            string Message = CommandManager.MergeParams(Params, 1);
            foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (RoomUser == null || RoomUser.IsBot || RoomUser.GetClient() == null || RoomUser.GetClient().GetHabbo() == null || RoomUser.GetClient().GetHabbo().GetPermissions().HasRight("mod_tool") || RoomUser.GetClient().GetHabbo().Id == Session.GetHabbo().Id)
                    continue;

                RoomUser.GetClient().SendNotification("Você foi kickado por um moderador: " + Message);

                Room.GetRoomUserManager().RemoveUserFromRoom(RoomUser.GetClient(), true, false);
            }

            Session.SendWhisper("Kicko com sucesso todos os usuários da sala.");
        }
    }
}
