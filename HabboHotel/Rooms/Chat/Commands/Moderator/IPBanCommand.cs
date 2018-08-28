using System;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Moderation;
using Bios.Database.Interfaces;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class IPBanCommand : IChatCommand
    {
        public string PermissionRequired => "command_ip_ban";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Banir usuário por IP.";

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
                Session.SendWhisper("Digite o nome de usuário do usuário que deseja Ban IP e banar conta.");
                return;
            }

            Habbo Habbo = BiosEmuThiago.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Se produjo un error mientras que la búsqueda de usuario en la base de datos.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_tool") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Uau, você não pode proibir o usuário.");
                return;
            }

            String IPAddress = String.Empty;
            Double Expire = BiosEmuThiago.GetUnixTimestamp() + 78892200;
            string Username = Habbo.Username;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");

                dbClient.SetQuery("SELECT `ip_last` FROM `users` WHERE `id` = '" + Habbo.Id + "' LIMIT 1");
                IPAddress = dbClient.getString();
            }

            string Reason = null;
            if (Params.Length >= 3)
                Reason = CommandManager.MergeParams(Params, 2);
            else
                Reason = "Nenhuma razão especificada.";

            if (!string.IsNullOrEmpty(IPAddress))
                BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.IP, IPAddress, Reason, Expire);
            BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Reason, Expire);

            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Username);
            if (TargetClient != null)
                TargetClient.Disconnect();


            Session.SendWhisper("Sucesso, você tem IP e a conta baniu o usuário '" + Username + "' pelo motivo '" + Reason + "'!");
        }
    }
}