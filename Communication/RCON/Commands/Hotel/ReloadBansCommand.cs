﻿namespace Bios.Communication.RCON.Commands.Hotel
{
    class ReloadBansCommand : IRCONCommand
    {
        public string Description => "Se utiliza para actualizar los baneos";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            BiosEmuThiago.GetGame().GetModerationManager().ReCacheBans();

            return true;
        }
    }
}