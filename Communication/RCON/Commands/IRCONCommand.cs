namespace Bios.Communication.RCON.Commands
{
    public interface IRCONCommand
    {
        string Parameters { get; }
        string Description { get; }
        bool TryExecute(string[] parameters);
    }
}