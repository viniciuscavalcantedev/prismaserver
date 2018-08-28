using Bios.Core;

namespace Bios.Communication.RCON.Commands.Hotel
{
    class ReloadServerSettingsCommand : IRCONCommand
    {
        public string Description => "Se utiliza para recarregar as configurações";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            BiosEmuThiago.GetGame().GetSettingsManager().Init();
            return true;
        }
    }
}