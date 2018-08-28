using System;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Moderation;

namespace Bios.Communication.RCON.Commands.User
{
    class AlertUserCommand : IRCONCommand
    {
        public string Description
        {
            get { return "Este comando é usado para alertar um usuário."; }
        }

        public string Parameters
        {
            get { return "%userId% %message%"; }
        }

        public bool TryExecute(string[] parameters)
        {
            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = BiosEmuThiago.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            // Validate the message
            if (string.IsNullOrEmpty(Convert.ToString(parameters[1])))
                return false;

            string message = Convert.ToString(parameters[1]);

            client.SendMessage(new BroadcastMessageAlertComposer(message));
            return true;
        }
    }
}