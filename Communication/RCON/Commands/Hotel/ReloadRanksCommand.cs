using System.Linq;
using Bios.HabboHotel.GameClients;

namespace Bios.Communication.RCON.Commands.Hotel
{
    class ReloadRanksCommand : IRCONCommand
    {
        public string Description => "Se utiliza para recarregar os ranks";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            BiosEmuThiago.GetGame().GetPermissionManager().Init();

            foreach (GameClient client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
            {
                if (client == null || client.GetHabbo() == null || client.GetHabbo().GetPermissions() == null)
                    continue;

                client.GetHabbo().GetPermissions().Init(client.GetHabbo());
            }
            
            return true;
        }
    }
}