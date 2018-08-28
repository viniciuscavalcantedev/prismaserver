using Bios.HabboHotel.GameClients;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    class DiscoCommand : IChatCommand
    {
        public string PermissionRequired => "command_disco";
        public string Parameters => "";
        public string Description => "Easter Egg";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Room != null || !Room.CheckRights(Session))
            {
                //Room.DiscoMode = !Room.DiscoMode;
            }
        }
    }
}