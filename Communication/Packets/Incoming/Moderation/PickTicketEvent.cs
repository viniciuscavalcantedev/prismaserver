using Bios.HabboHotel.Moderation;
using Bios.Communication.Packets.Outgoing.Moderation;

namespace Bios.Communication.Packets.Incoming.Moderation
{
    class PickTicketEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int Junk = Packet.PopInt();//??
            int TicketId = Packet.PopInt();

            ModerationTicket Ticket = null;
            if (!BiosEmuThiago.GetGame().GetModerationManager().TryGetTicket(TicketId, out Ticket))
                return;

            Ticket.Moderator = Session.GetHabbo();
            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(Session.GetHabbo().Id, Ticket), "mod_tool");
        }
    }
}
