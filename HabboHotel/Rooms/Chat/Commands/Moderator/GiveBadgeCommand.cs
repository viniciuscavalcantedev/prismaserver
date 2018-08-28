using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;
using Bios.HabboHotel.GameClients;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class GiveBadgeCommand : IChatCommand
    {
        public string PermissionRequired => "command_give_badge";
        public string Parameters => "[USUÁRIO] [IDEMBLEMA]";
        public string Description => "Dê um emblema a um usuário.";

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
            if (Params.Length != 3)
            {
                Session.SendWhisper("Digite um nome de usuário e um código de emblema que você gostaria de dar!");
                return;
            }

            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (TargetClient != null)
            {
                if (!TargetClient.GetHabbo().GetBadgeComponent().HasBadge(Params[2]))
                {
                    TargetClient.GetHabbo().GetBadgeComponent().GiveBadge(Params[2], true, TargetClient);
                    if (TargetClient.GetHabbo().Id != Session.GetHabbo().Id)
                        TargetClient.SendMessage(RoomNotificationComposer.SendBubble("badge/" + Params[2], "Você acabou de receber um emblema!", "/inventory/open/badge"));
                    else
                        Session.SendMessage(RoomNotificationComposer.SendBubble("badge/" + Params[2], "Você acabou de dar o emblema: " + Params[2], " /inventory/open/badge"));
                }
                else
                    Session.SendWhisper("Uau, esse usuário já possui este emblema(" + Params[2] + ") !");
                return;
            }
            else
            {
                Session.SendWhisper("Nossa, não conseguimos encontrar o usuário!");
                return;
            }
        }
    }
}
