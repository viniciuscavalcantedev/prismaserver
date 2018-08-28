using Bios.Core;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class RegisterstaffCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_update"; }
        }

        public string Parameters
        {
            get { return "%username% %password%"; }
        }

        public string Description
        {
            get { return "Coloca login staff!"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Algo está faltando!");
                return;
            }

            GameClient Target = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Opa, não foi possível encontrar esse usuário!");
                return;
            }


                DataRow Staffthiago = null;
                using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT cadstaff FROM users WHERE id = '" + Target.GetHabbo().Id + "'");
                    Staffthiago = dbClient.getRow();
                }

                if (Convert.ToBoolean(Staffthiago["cadstaff"]) == false)
                {

                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE users SET cadstaff = 'true' WHERE id = " + Target.GetHabbo().Id + ";");
                        dbClient.SetQuery("INSERT INTO `stafflogin` (`user_id`,`password`) VALUES ('" + Target.GetHabbo().Id + "', '" + Params[2] + "')");
                        dbClient.RunQuery();
                    }

                    Target.SendWhisper("Pronto agora você é um staff de verdade!");
                    Session.SendWhisper("Pronto usuário cadastrado como staff!");
                }


                DataRow Staffsthiago = null;
                using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT cadstaff FROM users WHERE id = '" + Target.GetHabbo().Id + "'");
                    Staffsthiago = dbClient.getRow();
                }

                if (Convert.ToBoolean(Staffsthiago["cadstaff"]) == true)
                {
                    Session.SendWhisper("Esse usuário ja fez o registe staff! , caso queira muda fale com um porgramador ou CEO do hotel.");
                }
        }
    }
}

