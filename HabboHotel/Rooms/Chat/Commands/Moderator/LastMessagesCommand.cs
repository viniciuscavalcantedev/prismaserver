using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;

using Bios.HabboHotel.Users;
using Bios.HabboHotel.GameClients;

using Bios.Database.Interfaces;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class LastMessagesCommand : IChatCommand
    {
        public string PermissionRequired => "command_user_info";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Verifique as últimas mensagens de usuário.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (ExtraSettings.STAFF_EFFECT_ENABLED_ROOM)
            {
                if (Session.GetHabbo().isLoggedIn && Session.GetHabbo().Rank > Convert.ToInt32(BiosEmuThiago.GetConfig().data["MineRankStaff"]))
                {
                }
                else
                {
                    Session.SendWhisper("Você precisa estar logado como staff para usar este comando.");
                    return;
                }
            }
            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite o nome do usuário que deseja ver, revise suas informações.");
                return;
            }

            DataRow UserData = null;
            string Username = Params[1];

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `username` FROM users WHERE `username` = @Username LIMIT 1");
                dbClient.AddParameter("Username", Username);
                UserData = dbClient.getRow();
            }

            if (UserData == null)
            {
                Session.SendNotification("Não há nenhum usuário com o nome " + Username + ".");
                return;
            }

            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Username);

            DataTable GetLogs = null;
            StringBuilder HabboInfo = new StringBuilder();

            HabboInfo.Append("Estas são as últimas mensagens do usuário suspeito, lembre-se sempre de verificar esses casos antes de prosseguir a proibição, a menos que seja um caso óbvio de spam.\n\n");

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `message` FROM `chatlogs` WHERE `user_id` = '" + TargetClient.GetHabbo().Id + "' ORDER BY `id` DESC LIMIT 10");
                GetLogs = dbClient.getTable();

                if (GetLogs != null)
                {
                    int Number = 11;
                    foreach (DataRow Log in GetLogs.Rows)
                    {
                        Number -= 1;
                        HabboInfo.Append("<font size ='8' color='#B40404'><b>[" + Number + "]</b></font>" + " " + Convert.ToString(Log["message"]) + "\r");
                        Session.SendMessage(new RoomNotificationComposer("usuário: " + Username + " - " + Number + ":", Convert.ToString(Log["message"]) + "", "", ""));
                    }
                }
                Session.SendMessage(new RoomNotificationComposer("Últimos mensagem de " + Username + ":", (HabboInfo.ToString()), "fig/" + TargetClient.GetHabbo().Look + "", "", ""));
            }
        }
    }
}
    
