using Bios.Database.Interfaces;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class BubbleBotCommand : IChatCommand
    {
        public string PermissionRequired => "command_bubble";
        public string Parameters => "[BOTNAME] [BUBBLEID]";
        public string Description => "Ultilize um balão de fala para conversar";

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
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            if (Params.Length == 1)
            {
                Session.SendWhisper("Oh, não se esqueça de inserir o nome do bot!");
                return;
            }

            if (Params.Length == 2)
            {
                Session.SendWhisper("Oh, esqueceu-se de introduzir um ID!");
                return;
            }
            string BotName = CommandManager.MergeParams(Params, 1);
            string Bubble = CommandManager.MergeParams(Params, 2);
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `bots` SET `chat_bubble` =  '" + Params[2] + "' WHERE `name` =  '" + Params[1] + "' AND  `room_id` =  '" + Session.GetHabbo().CurrentRoomId + "'");
                Session.LogsNotif("Você mudou a fala do bot: " + Params[1] + "!", "command_notification");
            }
        }
    }
}