using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;
using Bios.HabboHotel.Rooms.Chat.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    class EndPollCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_give_badge"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Termine a pesquisa na sala atual."; }
        }

        public void Execute(GameClients.GameClient Session, Bios.HabboHotel.Rooms.Room Room, string[] Params)
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
            Room.EndQuestion();
            Session.SendMessage(new RoomNotificationComposer("game", 3, "Espere um pouquinho que a pesquisa ja vai ser terminada!", ""));
            return;
        }
    }
}
