using System.Linq;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class MassBadgeCommand : IChatCommand
    {
        public string PermissionRequired => "command_mass_badge";
        public string Parameters => "[CODIGO]";
        public string Description => "Dê emblema para todo o hotel.";

        public void Execute(GameClient Session, Room Room, string[] Params)
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
                Session.SendWhisper("Por favor, insira o código do emblema que você gostaria de dar a todo o hotel.");
                return;
            }

            foreach (GameClient Client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
            {
                if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().Username == Session.GetHabbo().Username)
                    return;

                if (!Client.GetHabbo().GetBadgeComponent().HasBadge(Params[1]))
                {
                    Client.GetHabbo().GetBadgeComponent().GiveBadge(Params[1], true, Client);
                    Client.SendMessage(RoomNotificationComposer.SendBubble("badge/" + Params[1], "Você acabou de receber um emblema!", "/inventory/open/badge"));
                }
                else
                    Client.SendWhisper(Session.GetHabbo().Username + " Eu tento dar-lhe um emblema, mas você já o tem!");
            }

            Session.SendWhisper("Você deu com êxito a cada usuário neste hotel o emblema: " + Params[1] + "!");
        }
    }
}
