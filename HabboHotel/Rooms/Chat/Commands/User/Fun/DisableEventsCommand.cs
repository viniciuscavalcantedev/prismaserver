using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Bios.Database.Interfaces;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class DisableEventsCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return ""; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Ativar ou desativar mensagens de eventos."; }
        }

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            Session.GetHabbo().AllowEvents = !Session.GetHabbo().AllowEvents;
            Session.SendWhisper("Você " + (Session.GetHabbo().AllowEvents == true ? "permite" : "não permite") + " receber mensagens de eventos.");

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `allow_events` = @AllowEvents WHERE `id` = '" + Session.GetHabbo().Id + "'");
                dbClient.AddParameter("AllowEvents", BiosEmuThiago.BoolToEnum(Session.GetHabbo().AllowEvents));
                dbClient.RunQuery();
            }
        }
    }
}