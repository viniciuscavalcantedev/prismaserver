using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.HabboHotel.GameClients;
using Bios.Core;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Events
{
   class DiceAlertCommand : IChatCommand
    {
        public string PermissionRequired => "command_da2_alert";
        public string Parameters => "[MENSAJE]";
        public string Description => "Enviar una alerta de hotel para su evento!";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session == null) return;
            if (Room == null) return;
            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new SuperNotificationComposer(NotificationSettings.NOTIFICATION_OLE_IMG, "¡Se han abierto los dados oficiales!", "El inter que abre los dados es: <b><font color='#FF8000'>" + Session.GetHabbo().Username + " </font></b>\nAo contrário de dados comuns, é que estes podem apostar com segurança" + "\r\rO interesse será responsável por supervisionar que tudo é feito corretamente\n\n ¡¿O QUE ESPERAS?! ¡Venha agora e ganhar apostando contra outros usuários!",
                "Ir a la sala", "event:navigator/goto/" + Room.Id));
        }
    }
}