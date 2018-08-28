using Bios.Database.Interfaces;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class EnableFriendsCommand : IChatCommand
    {
        public string PermissionRequired => "command_enable_friends";
        public string Parameters => ""; 
        public string Description => "Ativar solicitaçoes de Amizade.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            
            Session.GetHabbo().AllowFriendRequests = !Session.GetHabbo().AllowFriendRequests;
            Session.SendWhisper("You're " + (Session.GetHabbo().AllowFriendRequests == true ? "Agora" : "nao") + " Capaz de ser amigo.");

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `block_newfriends` = '0' WHERE `id` = '" + Session.GetHabbo().Id + "'");
               
                dbClient.RunQuery();
            }
        }
    }
}