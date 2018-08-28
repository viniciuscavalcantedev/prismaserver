using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.Rooms;


namespace Bios.Communication.Packets.Incoming.Rooms.Action
{
    class AmbassadorWarningMessageEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {

            int UserId = Packet.PopInt();
            int RoomId = Packet.PopInt();
            int Time = Packet.PopInt();
            string HotelName = BiosEmuThiago.HotelName;

            Room Room = Session.GetHabbo().CurrentRoom;
            RoomUser Target = Room.GetRoomUserManager().GetRoomUserByHabbo(BiosEmuThiago.GetUsernameById(UserId));
            if (Target == null)
                return;

            long nowTime = BiosEmuThiago.CurrentTimeMillis();
            long timeBetween = nowTime - Session.GetHabbo()._lastTimeUsedHelpCommand;
            if (timeBetween < 60000)
            {
                Session.SendMessage(RoomNotificationComposer.SendBubble("Abuso", "Espere pelo menos 1 minuto para enviar um alerta de novo.", ""));
                return;
            }

            else
                BiosEmuThiago.GetGame().GetClientManager().StaffAlert(RoomNotificationComposer.SendBubble("advice", "" + Session.GetHabbo().Username + " acaba de mandar um alerta embaixador a " + Target.GetClient().GetHabbo().Username + ", clique aqui para ir.", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));
            Target.GetClient().SendMessage(new BroadcastMessageAlertComposer("<b><font size='15px' color='#c40101'>Mensagem de embaixadores<br></font></b>embaixadores de " + HotelName + " considerar que o seu comportamento não é o melhor. Por favor, reconsidere a sua atitude, antes de um moderador tomar medidas."));

            Session.GetHabbo()._lastTimeUsedHelpCommand = nowTime;
        }
    }
}
