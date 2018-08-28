namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class IgnoreWhispersCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_ignore_whispers"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Permite que você ignore todos os sussurros na sala, exceto pelo seu."; }
        }

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
        {
            
            Session.GetHabbo().IgnorePublicWhispers = !Session.GetHabbo().IgnorePublicWhispers;
            Session.SendWhisper("Você é " + (Session.GetHabbo().IgnorePublicWhispers ? "agora" : "não mais") + " ignorando os sussurros públicos!");
        }
    }
}
