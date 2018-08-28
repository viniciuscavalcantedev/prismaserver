
using Bios.Core;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
	class UnmuteCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_unmute"; }
		}

		public string Parameters
		{
			get { return "%username%"; }
		}

		public string Description
		{
			get { return "Desative um usuário atualmente silenciado."; }
		}

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
				Session.SendWhisper("Digite o nome de usuário do usuário que deseja desmutar.");
				return;
			}

			GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
			if (TargetClient == null || TargetClient.GetHabbo() == null)
			{
				Session.SendWhisper("Ocorreu um erro ao encontrar esse usuário, talvez eles não estejam online.");
				return;
			}

			using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
			{
				dbClient.RunQuery("UPDATE `users` SET `time_muted` = '0' WHERE `id` = '" + TargetClient.GetHabbo().Id + "' LIMIT 1");
			}

			TargetClient.GetHabbo().TimeMuted = 0;
			TargetClient.SendNotification("YVocê foi desmutado por " + Session.GetHabbo().Username + "!");
			Session.SendWhisper("Você desmuto o usuário(a) " + TargetClient.GetHabbo().Username + "!");
		}
	}
}