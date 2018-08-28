using System;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Moderation;
using Bios.Database.Interfaces;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class BanCommand : IChatCommand
    {

        public string PermissionRequired => "command_ban";
        public string Parameters => "[USUÁRIO] [TEMPO] [RAZÂO]";
        public string Description => "Banir um usuário.";

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
                Session.SendWhisper("Digite o nome do usuário que deseja Ban IP e banar conta.");
                return;
            }

            Habbo Habbo = BiosEmuThiago.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário no banco de dados.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_soft_ban") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Uau, você não pode banir o usuário.");
                return;
            }

            Double Expire = 0;
            string Hours = Params[2];
            if (String.IsNullOrEmpty(Hours) || Hours == "perm")
                Expire = BiosEmuThiago.GetUnixTimestamp() + 78892200;
            else
                Expire = (BiosEmuThiago.GetUnixTimestamp() + (Convert.ToDouble(Hours) * 3600));

            string Reason = null;
            if (Params.Length >= 4)
                Reason = CommandManager.MergeParams(Params, 3);
            else
                Reason = "Nenhuma razão especificada.";

            string Username = Habbo.Username;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
            }

            BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Reason, Expire);

            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Username);
            if (TargetClient != null)
                TargetClient.Disconnect();

            Session.SendWhisper("Sucesso, você proibiu o usuário da conta '" + Username + "' por " + Hours + " hora(s) com razão: '" + Reason + "'!");
        }
    }
}