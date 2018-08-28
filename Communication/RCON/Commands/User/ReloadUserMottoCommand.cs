using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Rooms;
using Bios.Communication.Packets.Outgoing.Rooms.Engine;

namespace Bios.Communication.RCON.Commands.User
{
    class ReloadUserMottoCommand : IRCONCommand
    {
        public string Description
        {
            get { return "Este comando é usado para recarregar o lema dos usuários do banco de dados."; }
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
                dbClient.SetQuery("SELECT `motto` FROM `users` WHERE `id` = @userID LIMIT 1");
                dbClient.AddParameter("userID", userId);
                client.GetHabbo().Motto = dbClient.getString();
            }

            // If we're in a room, we cannot really send the packets, so flag this as completed successfully, since we already updated it.
            if (!client.GetHabbo().InRoom)
            {
                return true;
            }
            else
            {
                //We are in a room, let's try to run the packets.
                Room Room = client.GetHabbo().CurrentRoom;
                if (Room != null)
                {
                    RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(client.GetHabbo().Id);
                    if (User != null)
                    {
                        Room.SendMessage(new UserChangeComposer(User, false));
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
