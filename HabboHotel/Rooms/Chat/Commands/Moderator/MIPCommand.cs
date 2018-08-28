using System;
using Bios.Database.Interfaces;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Moderation;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class MIPCommand : IChatCommand
    {
        public string PermissionRequired => "command_mip";
        public string Parameters => "[USUÁRIO]";
        public string Description => "Proibição de máquina, Banear IP e a conta de outro usuário.";

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
                Session.SendWhisper("Digite o nome de usuário do usuário que deseja Ban IP e banir conta.");
                return;
            }

            Habbo Habbo = BiosEmuThiago.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário no banco de dados.");
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
                Reason = "Não há motivo específico.";

            if (!string.IsNullOrEmpty(IPAddress))
                BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.IP, IPAddress, Reason, Expire);
            BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Reason, Expire);

            if (!string.IsNullOrEmpty(Habbo.MachineId))
                BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.MACHINE, Habbo.MachineId, Reason, Expire);

            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Username);
            if (TargetClient != null)
                TargetClient.Disconnect();
            Session.SendWhisper("Sucesso, a banio machine banio a: '" + Username + "' pelo seguinte motivo: '" + Reason + "'!");
        }
    }
}