using Bios.Database.Interfaces;
using System.Linq;
using System.Collections.Generic;
using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.Communication.Packets.Outgoing.Messenger;
using Bios.Communication.Packets.Outgoing.Rooms.Session;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class DeleteGroupCommand : IChatCommand
    {
        public string PermissionRequired => "command_delete_group";
        public string Parameters => "";
        public string Description => "Apaga um grupo do banco de dados e do hotel.";

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
            Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            if (Room.Group == null)
            {
                Session.SendWhisper("Bem, não há nenhum grupo aqui?");
                return;
            }

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("DELETE FROM `groups` WHERE `id` = '" + Room.Group.Id + "'");
                dbClient.runFastQuery("DELETE FROM `group_memberships` WHERE `group_id` = '" + Room.Group.Id + "'");
                dbClient.runFastQuery("DELETE FROM `group_requests` WHERE `group_id` = '" + Room.Group.Id + "'");
                dbClient.runFastQuery("UPDATE `rooms` SET `group_id` = '0' WHERE `group_id` = '" + Room.Group.Id + "' LIMIT 1");
                dbClient.runFastQuery("UPDATE `user_stats` SET `groupid` = '0' WHERE `groupid` = '" + Room.Group.Id + "' LIMIT 1");
                dbClient.runFastQuery("DELETE FROM `items_groups` WHERE `group_id` = '" + Room.Group.Id + "'");
            }

            BiosEmuThiago.GetGame().GetGroupManager().DeleteGroup(Room.RoomData.Group.Id);

            Room.Group = null;
            Room.RoomData.Group = null;

            BiosEmuThiago.GetGame().GetRoomManager().UnloadRoom(Room);
            if (Room.RoomData.Group.HasChat)
            {
                var Client = BiosEmuThiago.GetGame().GetClientManager().GetClientByUserID(Session.GetHabbo().Id);
                if (Client != null)
                {
                    Client.SendMessage(new FriendListUpdateComposer(Room.RoomData.Group, -1));
                    Client.SendMessage(new BroadcastMessageAlertComposer(BiosEmuThiago.GetGame().GetLanguageManager().TryGetValue("server.console.alert") + "\n\n Você deixou o grupo, por favor, se você ver o grupo de chat, no entanto, relogue no jogo."));
                }
            }

            var roomId = Session.GetHabbo().CurrentRoomId;
            List<RoomUser> UsersToReturn = new List<RoomUser>(Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUsers().ToList());

            RoomData Data = BiosEmuThiago.GetGame().GetRoomManager().GenerateRoomData(roomId);
            Session.GetHabbo().PrepareRoom(Session.GetHabbo().CurrentRoom.RoomId, "");
            BiosEmuThiago.GetGame().GetRoomManager().LoadRoom(roomId);

            foreach (RoomUser User in UsersToReturn)
            {
                if (User == null || User.GetClient() == null)
                    continue;

                User.GetClient().SendMessage(new RoomForwardComposer(roomId));
            }

            Session.SendNotification("Éxito, grupo eliminado.");
            return;
        }
    }
}
