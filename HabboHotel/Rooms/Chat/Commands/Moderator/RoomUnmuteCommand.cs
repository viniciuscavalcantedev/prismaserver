
using System.Collections.Generic;

using Bios.HabboHotel.Rooms;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
	class RoomUnmuteCommand : IChatCommand
	{
		public string PermissionRequired
		{
			get { return "command_give_room"; }
		}

		public string Parameters
		{
			get { return ""; }
		}

		public string Description
		{
			get { return "Desmutar a sala"; }
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
            if (!Room.RoomMuted)
			{
				Session.SendWhisper("Este quarto não está mutado.");
				return;
			}

			Room.RoomMuted = false;

			List<RoomUser> RoomUsers = Room.GetRoomUserManager().GetRoomUsers();
			if (RoomUsers.Count > 0)
			{
				foreach (RoomUser User in RoomUsers)
				{
					if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null || User.GetClient().GetHabbo().Username == Session.GetHabbo().Username)
						continue;

					User.GetClient().SendWhisper("Esta sala foi desmutada .");
				}
			}
		}
	}
}