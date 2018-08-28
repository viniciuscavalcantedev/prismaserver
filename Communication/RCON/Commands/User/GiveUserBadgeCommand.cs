using System;

using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.Communication.RCON.Commands.User
{
    class GiveUserBadgeCommand : IRCONCommand
    {
        public string Description
        {
            get { return "Este comando é usado para dar ao usuário um Emblema."; }
        }

        public string Parameters
        {
            get { return "%userId% %badgeId%"; }
        }

        public bool TryExecute(string[] parameters)
        {
            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = BiosEmuThiago.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            // Validate the badge
            if (string.IsNullOrEmpty(Convert.ToString(parameters[1])))
                return false;

            string badge = Convert.ToString(parameters[1]);

            if (client != null)
            {
                if (!client.GetHabbo().GetBadgeComponent().HasBadge(badge))
                {
                    client.SendMessage(RoomNotificationComposer.SendBubble("badge/" + badge, "Você acaba de ganha um emblema!", "/inventory/open/badge"));
                    client.GetHabbo().GetBadgeComponent().GiveBadge(badge, true, client);
                }
            }
            return true;
        }
    }
}