using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class HotelAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_hotel_alert";
        public string Parameters => "[MENSAGE]";
        public string Description => "Enviar alerta para hotel.";

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
                Session.SendWhisper("Por favor, introduzca un mensage para enviar.");
                return;
            }
            int OnlineUsers = BiosEmuThiago.GetGame().GetClientManager().Count;
            int RoomCount = BiosEmuThiago.GetGame().GetRoomManager().Count;
            string Message = CommandManager.MergeParams(Params, 1);
            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(Message + "\n\n<b>Informações do BiosEmulador (By: Thiago Araujo)</b>:\n" +
            "Usuários Online: " + OnlineUsers + "\n" +
            "Salas Logadas: " + RoomCount + "\r\n" + "- " + Session.GetHabbo().Username));
            return;
        }
    }
}
