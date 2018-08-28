using Bios.HabboHotel.GameClients;
using System.Collections.Generic;
using System.Linq;
using Bios.Communication.Packets.Outgoing.Rooms.Session;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class MakePublicCommand : IChatCommand
    {
        public string PermissionRequired => "command_make_public";
        public string Parameters => "";
        public string Description => "Converta esta sala em público.";

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
            var room = Session.GetHabbo().CurrentRoom;
            using (var queryReactor = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                queryReactor.runFastQuery(string.Format("UPDATE rooms SET roomtype = 'public' WHERE id = {0}",
                    room.RoomId));

            var roomId = Session.GetHabbo().CurrentRoom.RoomId;
            var users = new List<RoomUser>(Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUsers().ToList());

            BiosEmuThiago.GetGame().GetRoomManager().UnloadRoom(Session.GetHabbo().CurrentRoom);

            RoomData Data = BiosEmuThiago.GetGame().GetRoomManager().GenerateRoomData(roomId);
            Session.GetHabbo().PrepareRoom(Session.GetHabbo().CurrentRoom.RoomId, "");

            BiosEmuThiago.GetGame().GetRoomManager().LoadRoom(roomId);

            var data = new RoomForwardComposer(roomId);

            foreach (var user in users.Where(user => user != null && user.GetClient() != null))
                user.GetClient().SendMessage(data);
        }
    }
}
