using Bios.HabboHotel.Catalog.PredesignedRooms;
using System.Text;
using System.Linq;
using System.Globalization;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class RemovePredesignedCommand : IChatCommand
    {
        public string PermissionRequired => "command_update";
        public string Parameters => "";
        public string Description => "Tira um quarto de pack do hotel!";

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
            if (Room == null) return;
            var predesignedId = 0U;
            using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT id FROM catalog_predesigned_rooms WHERE room_id = " + Room.Id + ";");
                predesignedId = (uint)dbClient.getInteger();

                dbClient.RunQuery("DELETE FROM catalog_predesigned_rooms WHERE room_id = " + Room.Id + " AND id = " +
                    predesignedId + ";");
            }

            BiosEmuThiago.GetGame().GetCatalog().GetPredesignedRooms().predesignedRoom.Remove(predesignedId);
            Session.SendWhisper("Você tiro a sala dos pack do hotel!");
        }
    }
}