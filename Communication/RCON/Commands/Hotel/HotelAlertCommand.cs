using System;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Moderation;

namespace Bios.Communication.RCON.Commands.Hotel
{
    class HotelAlertCommand : IRCONCommand
    {
        public string Description => "Este comando se utiliza para mandar alerta a um usuário .";
        public string Parameters => "[MENSAJE]";

        public bool TryExecute(string[] parameters)
        {
            string message = Convert.ToString(parameters[0]);

            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(BiosEmuThiago.GetGame().GetLanguageManager().TryGetValue("server.console.alert") + "\n\n" + message));
            return true;
        }
    }
}