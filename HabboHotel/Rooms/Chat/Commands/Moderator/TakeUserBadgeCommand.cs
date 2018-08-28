using System;
using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.HabboHotel.GameClients;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class TakeUserBadgeCommand : IChatCommand
    {
        public string PermissionRequired => "command_give_badge"; 
        public string Parameters => "[USUÁRIO] [CODIGO]";
        public string Description => "Ele é usado para remover o emblema de um usuário.";

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
                Session.SendWhisper("Digite um nome de usuário e um código de emblema que você gostaria de tira!");
                return;
            }

            GameClient client = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (client == null || client.GetHabbo() == null)
                return;

            if (string.IsNullOrEmpty(Convert.ToString(Params[2])))
                return;

            string badge = Convert.ToString(Params[2]);

            if (client.GetHabbo().GetBadgeComponent().HasBadge(badge))
            {
                client.GetHabbo().GetBadgeComponent().RemoveBadge(badge);
                Session.SendMessage(new BroadcastMessageAlertComposer(BiosEmuThiago.GetGame().GetLanguageManager().TryGetValue("server.console.alert") + "\n\n" + "O emblema <b>"+ badge + " foi removido de "+ client.GetHabbo().Username+" com exito!"));
            }
            return;
        }
    }
}