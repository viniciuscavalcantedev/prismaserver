using Bios.Core;
using System;
using System.Linq;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class RoomBadgeCommand : IChatCommand
    {
        public string PermissionRequired => "command_room_badge";
        public string Parameters => "[CODIGO]";
        public string Description => "Dê emblema para toda a sala.";

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
                Session.SendWhisper("Digite o nome do identificador que você gostaria de dar à sala.");
                return;
            }

            foreach (RoomUser User in Room.GetRoomUserManager().GetUserList().ToList())
            {
                if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                    return;

                if (!User.GetClient().GetHabbo().GetBadgeComponent().HasBadge(Params[1]))
                {
                    User.GetClient().GetHabbo().GetBadgeComponent().GiveBadge(Params[1], true, User.GetClient());
                    User.GetClient().SendNotification("Você acabou de receber um emblema!");
                }
                else
                {
                    User.GetClient().SendWhisper(Session.GetHabbo().Username + " Eu tento dar-lhe um emblema, mas você já o tem!");
                    return;
                }
                    
            }

            Session.SendWhisper("Usted ha dado con éxito todos los usuarios en esta sala la placa: " + Params[2] + "!");
        }
    }
}
