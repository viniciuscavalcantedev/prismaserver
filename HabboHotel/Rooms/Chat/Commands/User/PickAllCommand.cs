using System.Linq;
using System.Collections.Generic;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.Items;
using Bios.Communication.Packets.Outgoing.Inventory.Furni;
using Bios.Database.Interfaces;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class PickAllCommand : IChatCommand
    {
        public string PermissionRequired => "command_pickall";
        public string Parameters => "";
        public string Description => "Remover todos mobis da sala";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {


            if (!Room.CheckRights(Session, true))
                return;

            Room.GetRoomItemHandler().RemoveItems(Session);
            Room.GetGameMap().GenerateMaps();

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `items` SET `room_id` = '0' WHERE `room_id` = @RoomId AND `user_id` = @UserId");
                dbClient.AddParameter("RoomId", Room.Id);
                dbClient.AddParameter("UserId", Session.GetHabbo().Id);
                dbClient.RunQuery();
                Session.LogsNotif("Todos mobis foram coletados", "command_notification");
            }

            List<Item> Items = Room.GetRoomItemHandler().GetWallAndFloor.ToList();
            if (Items.Count > 0)
                Session.SendWhisper("Ainda há mais elementos nesta sala, removidos manualmente ou usar: ejectall expulsá-los!");

            Session.SendMessage(new FurniListUpdateComposer());
        }
    }
}