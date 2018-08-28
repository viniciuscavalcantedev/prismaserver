using Bios.Database.Interfaces;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class DisableMimicCommand : IChatCommand
    {
        public string PermissionRequired => "command_disable_mimic";
        public string Parameters => "";
        public string Description => "Desativa comando ;copiar.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            
            Session.GetHabbo().AllowMimic = !Session.GetHabbo().AllowMimic;
            Session.SendWhisper("Tu " + (Session.GetHabbo().AllowMimic == true ? "agora" : "agora nao") + " pode ser imitado.");

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `allow_mimic` = @AllowMimic WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.AddParameter("AllowMimic", BiosEmuThiago.BoolToEnum(Session.GetHabbo().AllowMimic));
                dbClient.RunQuery();
            }
        }
    }
}