using Bios.Core;
using System;
using System.Collections.Generic;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class RoomMuteCommand : IChatCommand
    {
        public string PermissionRequired => "command_give_room";
        public string Parameters => "[MENSAGE]";
        public string Description => "Mutar a sala";

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
                Session.SendWhisper("Forneça um motivo para silenciar a sala para mostrar os usuários.");
                return;
            }

            if (!Room.RoomMuted)
                Room.RoomMuted = true;

            string Msg = CommandManager.MergeParams(Params, 1);

            List<RoomUser> RoomUsers = Room.GetRoomUserManager().GetRoomUsers();
            if (RoomUsers.Count > 0)
            {
                foreach (RoomUser User in RoomUsers)
                {
                    if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null || User.GetClient().GetHabbo().Username == Session.GetHabbo().Username)
                        continue;

                    User.GetClient().SendWhisper("Este quarto foi silenciado porque: " + Msg);
                }
            }
        }
    }
}
