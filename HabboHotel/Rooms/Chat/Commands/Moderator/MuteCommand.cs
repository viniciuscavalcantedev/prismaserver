using Bios.Core;
using Bios.Database.Interfaces;
using Bios.HabboHotel.Users;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class MuteCommand : IChatCommand
    {
        public string PermissionRequired => "command_mute";
        public string Parameters => "[USUÁRIO] [TEMPO]";
        public string Description => "Silenciar o usuário por um tempo.";

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
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
                Session.SendWhisper("Digite um nome de usuário e um tempo válido em segundos (máximo 600, nada mais é reiniciado para 600).");
                return;
            }

            Habbo Habbo = BiosEmuThiago.GetHabboByUsername(Params[1]);
            if (Habbo == null)
            {
                Session.SendWhisper("Ocorreu um erro ao procurar o usuário no banco de dados.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_tool") && !Session.GetHabbo().GetPermissions().HasRight("mod_mute_any"))
            {
                Session.SendWhisper("Uau, você não pode silenciar esse usuário.");
                return;
            }

			if (double.TryParse(Params[2], out double Time))
			{
				if (Time > 600 && !Session.GetHabbo().GetPermissions().HasRight("mod_mute_limit_override"))
					Time = 600;

				using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
				{
					dbClient.runFastQuery("UPDATE `users` SET `time_muted` = '" + Time + "' WHERE `id` = '" + Habbo.Id + "' LIMIT 1");
				}

				if (Habbo.GetClient() != null)
				{
					Habbo.TimeMuted = Time;
					Habbo.GetClient().SendNotification("Você foi silenciado por um moderador para " + Time + " segundos!");
				}

				Session.SendWhisper("Você muda para: " + Habbo.Username + " por " + Time + " segundos.");
			}
			else
				Session.SendWhisper("Insira um número inteiro válido.");
		}
    }
}