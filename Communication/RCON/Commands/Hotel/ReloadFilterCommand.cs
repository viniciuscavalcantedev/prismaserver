namespace Bios.Communication.RCON.Commands.Hotel
{
    class ReloadFilterCommand : IRCONCommand
    {
        public string Description => "Se utiliza para actualizar os filtros.";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            BiosEmuThiago.GetGame().GetChatManager().GetFilter().InitWords();
            BiosEmuThiago.GetGame().GetChatManager().GetFilter().InitCharacters();
            return true;
        }
    }
}