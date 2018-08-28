using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class CarryCommand : IChatCommand
    {
        public string PermissionRequired => "command_carry";
        public string Parameters => "[ITEMID]";
        public string Description => "Ele permite que você carregue um item na mão.";

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
            int ItemId = 0;
            if (!int.TryParse(Convert.ToString(Params[1]), out ItemId))
            {
                Session.SendWhisper("Por favor, introduza um número válido.");
                return;
            }

            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.CarryItem(ItemId);
        }
    }
}
