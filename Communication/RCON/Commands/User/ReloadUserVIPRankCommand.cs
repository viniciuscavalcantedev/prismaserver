using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;

namespace Bios.Communication.RCON.Commands.User
{
    class ReloadUserVIPRankCommand : IRCONCommand
    {
        public string Description
        {
            get { return "Este comando é usado para recarregar um ranking VIP de usuários e permissões."; }
        }

        public string Parameters
        {
            get { return "%userId%"; }
        }

        public bool TryExecute(string[] parameters)
        {
            int userId = 0;
            if (!int.TryParse(parameters[0].ToString(), out userId))
                return false;

            GameClient client = BiosEmuThiago.GetGame().GetClientManager().GetClientByUserID(userId);
            if (client == null || client.GetHabbo() == null)
                return false;

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `rank_vip` FROM `users` WHERE `id` = @userId LIMIT 1");
                dbClient.AddParameter("userId", userId);
                client.GetHabbo().VIPRank = dbClient.getInteger();
            }

            client.GetHabbo().GetPermissions().Init(client.GetHabbo());
            return true;
        }
    }
}