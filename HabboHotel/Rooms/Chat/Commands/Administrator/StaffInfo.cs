using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Items;
using Bios.Communication.Packets.Outgoing.Inventory.Furni;
using Bios.Database.Interfaces;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class StaffInfo : IChatCommand
    {
        public string PermissionRequired => "command_staffinfo";
        public string Parameters => "";
        public string Description => "Ver lista de staffs online";

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
            Dictionary<Habbo, UInt32> clients = new Dictionary<Habbo, UInt32>();

            StringBuilder content = new StringBuilder();
            content.Append("Status da Equipe Iniciada " + BiosEmuThiago.HotelName + ":\r\n");

            foreach (var client in BiosEmuThiago.GetGame().GetClientManager()._clients.Values)
            {
                if (client != null && client.GetHabbo() != null && client.GetHabbo().Rank > 3)
                    clients.Add(client.GetHabbo(), (Convert.ToUInt16(client.GetHabbo().Rank)));
            }

            foreach (KeyValuePair<Habbo, UInt32> client in clients.OrderBy(key => key.Value))
            {
                if (client.Key == null)
                    continue;

                content.Append("¥ " + client.Key.Username + " [Rank: " + client.Key.Rank + "] » Se na sala: " + ((client.Key.CurrentRoom == null) ? "em nenhuma sala." : client.Key.CurrentRoom.RoomData.Name) + "\r\n");
            }

            Session.SendMessage(new MOTDNotificationComposer(content.ToString()));
            return;
        }
    }
}