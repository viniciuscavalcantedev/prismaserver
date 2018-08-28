using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class LogMeInCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_alert_user"; }
        }
        public string Parameters
        {
            get { return "%username% %password%"; }
        }
        public string Description
        {
            get { return "Efetuar login como staff"; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (ExtraSettings.STAFF_EFFECT_ENABLED_ROOM)
            {
                if (Session.GetHabbo().Rank > Convert.ToInt32(BiosEmuThiago.GetConfig().data["MineRankStaff"]))
            {

                if (Params.Length == 1)
                {
                    Session.SendWhisper("Algo está faltando!");
                    return;
                }

                if (Session.GetHabbo().isLoggedIn == true)
                {
                    Session.SendWhisper("Você já entrou!");
                    return;
                }

                if (Params[1] != Session.GetHabbo().Username)
                {
                    Session.SendWhisper("Você só pode fazer login em sua própria conta.");
                    return;
                }

                if (Session.GetHabbo().Username == Params[1])
                {
                    string passw = Params[2];
                    string password;

                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.SetQuery("SELECT `password` FROM stafflogin WHERE `user_id` = " + Session.GetHabbo().Id + " LIMIT 1");
                        dbClient.AddParameter("password", passw);
                        password = dbClient.getString();
                    }

                    if (password == Params[2])
                    {
                        Session.GetHabbo().isLoggedIn = true;
                        Session.SendWhisper("Aviso do BiosEmulador: " + Params[1] + ", Você está agora logado como staff!");

                            if (Session.GetHabbo().Rank != ExtraSettings.AmbassadorMinRank)
                            {
                                using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("SELECT * FROM `ranks` WHERE id = '" + Session.GetHabbo().Rank + "'");
                                DataRow Table = dbClient.getRow();

                                if (Session.GetHabbo().Rank < Convert.ToInt32(BiosEmuThiago.GetConfig().data["MineRankStaff"]))
                                {
                                    // Thiago é muito lindo ser é doido
                                }
                                else
                                {
                                    using (IQueryAdapter dbClients = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                                    {
                                        dbClients.RunQuery("UPDATE users SET prefix_name = '" + Convert.ToString(Table["TAGSTAFF"]) + "' WHERE id = '" + Session.GetHabbo().Id + "'");
                                        dbClients.RunQuery("UPDATE users SET prefix_name_color = '#" + Convert.ToString(Table["CORSTAFF"]) + "' WHERE id = '" + Session.GetHabbo().Id + "'");
                                    }
                                    Session.GetHabbo()._NamePrefixColor = "#" + Convert.ToString(Table["CORSTAFF"]);
                                    Session.GetHabbo()._NamePrefix = Convert.ToString(Table["TAGSTAFF"]);
                                    Session.SendWhisper("Tag " + Convert.ToString(Table["TAGSTAFF"]) + " foi ativada!");
                                    Session.GetHabbo().Effects().ApplyEffect(102);

                                    string figure = Session.GetHabbo().Look;

                                    BiosEmuThiago.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + figure, 3, "O " + Convert.ToString(Table["name"]) + " " + Params[1] + " fez o login staff!", ""));

                                }
                            }
                            }

                            if (Session.GetHabbo().Rank == ExtraSettings.AmbassadorMinRank)
                            {
                                using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.SetQuery("SELECT * FROM `ranks` WHERE id = '" + Session.GetHabbo().Rank + "'");
                                DataRow Table = dbClient.getRow();

                                if (Session.GetHabbo().Rank < Convert.ToInt32(BiosEmuThiago.GetConfig().data["MineRankStaff"]))
                                {
                                    // Thiago é muito lindo ser é doido
                                }
                                else
                                {
                                    using (IQueryAdapter dbClients = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                                    {
                                        dbClients.RunQuery("UPDATE users SET prefix_name = '" + Convert.ToString(Table["TAGSTAFF"]) + "' WHERE id = '" + Session.GetHabbo().Id + "'");
                                        dbClients.RunQuery("UPDATE users SET prefix_name_color = '#" + Convert.ToString(Table["CORSTAFF"]) + "' WHERE id = '" + Session.GetHabbo().Id + "'");
                                    }
                                    Session.GetHabbo()._NamePrefixColor = "#" + Convert.ToString(Table["CORSTAFF"]);
                                    Session.GetHabbo()._NamePrefix = Convert.ToString(Table["TAGSTAFF"]);
                                    Session.SendWhisper("Tag " + Convert.ToString(Table["TAGSTAFF"]) + " foi ativada!");
                                    Session.GetHabbo().Effects().ApplyEffect(178);
                                    Session.SendWhisper("Não esqueça de ligar sua ferramenta de embaixador!");

                                    string figure = Session.GetHabbo().Look;

                                    BiosEmuThiago.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + figure, 3, "O " + Convert.ToString(Table["name"]) + " " + Params[1] + " fez o login staff!", ""));

                                }
                            }
                         }
                    }

                    else if (password != Params[2])
                    {
                        Session.SendWhisper("Senha incorreta.");
                    }
                }
            }
        }
            else
            {
                Session.SendWhisper("Comando esta desativado nas configuração do emulador!");
                return;
            }
        }
    }

}
