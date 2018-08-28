using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class CustomizedHotelAlert : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_update"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Enviar uma mensagem custom para todo o Hotel"; }
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
                    Session.SendWhisper("Por favor, indique a mensagem para enviar.");
                    return;
                }

                string Message = CommandManager.MergeParams(Params, 1);
                BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomCustomizedAlertComposer("\n" + Message + "\n\n - " + Session.GetHabbo().Username + ""));
                BiosEmuThiago.GetGame().GetClientManager().SendMessage(new MassEventComposer(Message));
                Session.SendWhisper("Custom Alerta enviado!");
                return;
        }
    }
}
