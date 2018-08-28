using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Moderation;

namespace Bios.Communication.Packets.Incoming.Moderation
{
    class CallForHelpPendingCallsDeletedEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (session == null || session.GetHabbo() == null)
                return;
            
            if (BiosEmuThiago.GetGame().GetModerationManager().UserHasTickets(session.GetHabbo().Id))
            {
                ModerationTicket PendingTicket = BiosEmuThiago.GetGame().GetModerationManager().GetTicketBySenderId(session.GetHabbo().Id);
                if (PendingTicket != null)
                {
                    PendingTicket.Answered = true;
                    BiosEmuThiago.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(session.GetHabbo().Id, PendingTicket), "mod_tool");
                }
            }
        }
    }
}
