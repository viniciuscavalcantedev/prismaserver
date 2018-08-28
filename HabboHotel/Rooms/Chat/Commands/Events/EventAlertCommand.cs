using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Engine;
using System.Linq;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Events
{
    internal class EventAlertCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get
            {
                return "command_event_alert";
            }
        }
        public string Parameters
        {
            get
            {
                return "%message%";
            }
        }
        public string Description
        {
            get
            {
                return "Enviar um alerta de evento";
            }
        }
        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Session != null)
            {
                if (Room != null)
                {
                    if (Params.Length == 1)
                    {
                        Session.SendWhisper("Por favor, digite uma mensagem para enviar.");
                        return;
                    }
                    foreach (GameClient client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
                        if (client.GetHabbo().AllowEvents == true)
                        {
                            string Message = CommandManager.MergeParams(Params, 1);

                            client.SendMessage(new RoomNotificationComposer("Está acontecendo um evento!",
                                 "Está acontecendo um novo jogo realizado pela equipe Staff! <br><br>Este, tem o intuito de proporcionar um entretenimento a mais para os usuários!<br><br>Evento:<b>  " + Message +
                                 "</b><br>Por:<b>  " + Session.GetHabbo().Username +
                                 "</b> <br><br>Caso deseje participar, clique no botão abaixo! <br><br>Caso não queira ser notificado sobre os eventos digite esse comando<b>  :alertas</b> !",
                                 "events", "Participar do Evento", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
                        }
                        else
                            client.SendWhisper("Parece que está havendo um novo evento em nosso hotel. Para reativar as mensagens de eventos digite ;alertas", 1);

                }
            }
        }
    }
}