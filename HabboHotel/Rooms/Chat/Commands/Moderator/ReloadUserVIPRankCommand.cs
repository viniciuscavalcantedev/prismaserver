using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Communication.Packets.Outgoing.Users;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Rooms.Chat.Commands;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class ReloadUserrVIPRankCommand : IChatCommand
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
            get { return "Dar rank vip para uma pessoa."; }
        }

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `users` SET `rank` = '2' WHERE `id` = '" + TargetClient.GetHabbo().Id + "'");
                dbClient.runFastQuery("UPDATE `users` SET `rank_vip` = '1' WHERE `id` = '" + TargetClient.GetHabbo().Id + "'");
                TargetClient.GetHabbo().Rank = 2;
                TargetClient.GetHabbo().VIPRank = 1;
            }

            TargetClient.GetHabbo().GetClubManager().AddOrExtendSubscription("club_vip", 1 * 24 * 3600, Session);
            TargetClient.GetHabbo().GetBadgeComponent().GiveBadge("DVIP", true, Session);

            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_VipClub", 1);
            TargetClient.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));

            string figure = TargetClient.GetHabbo().Look;
            BiosEmuThiago.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("fig/" + figure, 3, "O " + Params[1] + " agora é um usuário VIP!", ""));
            Session.SendWhisper("VIP dado com exito!");
        }
    }
}