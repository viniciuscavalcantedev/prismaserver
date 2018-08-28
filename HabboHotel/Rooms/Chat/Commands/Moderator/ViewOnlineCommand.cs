using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;



using Bios.HabboHotel.Users;

using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class ViewOnlineCommand : IChatCommand
    {
        public string PermissionRequired => "command_view_online";
        public string Parameters => "";
        public string Description => "Veja usuários online.";

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
            Dictionary<Habbo, UInt32> clients = new Dictionary<Habbo, UInt32>();

            StringBuilder content = new StringBuilder();
            content.Append("- LISTA DE USUÁRIOS ONLINE -\r\n");

            foreach (var client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
            {
                if (client == null)
                    continue;

                content.Append("¥ " + client.GetHabbo().Username + " » Está no quarto: " + ((client.GetHabbo().CurrentRoom == null) ? "Em qualquer sala." : client.GetHabbo().CurrentRoom.RoomData.Name) + "\r\n");
            }

            Session.SendMessage(new MOTDNotificationComposer(content.ToString()));
            return;
        }
    }
}
