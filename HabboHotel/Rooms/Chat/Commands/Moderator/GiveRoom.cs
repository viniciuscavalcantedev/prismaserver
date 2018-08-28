using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Core;
using Bios.HabboHotel.GameClients;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class GiveRoom : IChatCommand
    {
        public string PermissionRequired => "command_give_room";
        public string Parameters => "[QUANTIADE]";
        public string Description => "Dar créditos a todos.";

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
                Session.SendWhisper("Digite a quantidade que você gostaria de dar à sala.");
                return;
            }
			if (int.TryParse(Params[1], out int Amount))

				foreach (RoomUser RoomUser in Room.GetRoomUserManager().GetRoomUsers())
				{
					if (RoomUser == null || RoomUser.GetClient() == null || Session.GetHabbo().Id == RoomUser.UserId)
						continue;
					RoomUser.GetClient().GetHabbo().Credits += Amount;
					RoomUser.GetClient().SendMessage(new CreditBalanceComposer(RoomUser.GetClient().GetHabbo().Credits));
				}
		}
}
}
  