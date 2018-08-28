using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;

namespace Bios.Communication.RCON.Commands.User
{
    class ReloadUserRankCommand : IRCONCommand
    {
        public string Description
        {
            get { return "Este comando é usado para recarregar uma classificação e permissões de usuários."; }
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
                dbClient.SetQuery("SELECT `rank` FROM `users` WHERE `id` = @userId LIMIT 1");
                dbClient.AddParameter("userId", userId);
                client.GetHabbo().Rank = dbClient.getInteger();
            }

            client.GetHabbo().GetPermissions().Init(client.GetHabbo());

            if (client.GetHabbo().GetPermissions().HasRight("mod_tickets"))
            {
                client.SendMessage(new ModeratorInitComposer(
                  BiosEmuThiago.GetGame().GetModerationManager().UserMessagePresets,
                  BiosEmuThiago.GetGame().GetModerationManager().RoomMessagePresets,
                  BiosEmuThiago.GetGame().GetModerationManager().GetTickets));
            }
            return true;
        }
    }
}