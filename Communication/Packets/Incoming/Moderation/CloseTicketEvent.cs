using Bios.HabboHotel.Moderation;
using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.HabboHotel.GameClients;
using Bios.Database.Interfaces;

namespace Bios.Communication.Packets.Incoming.Moderation
{
    class CloseTicketEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
                return;

            int Result = Packet.PopInt(); // 1 = useless, 2 = abusive, 3 = resolved
            int Junk = Packet.PopInt();
            int TicketId = Packet.PopInt();

            ModerationTicket Ticket = null;
            if (!BiosEmuThiago.GetGame().GetModerationManager().TryGetTicket(TicketId, out Ticket))
                return;

            if (Ticket.Moderator.Id != Session.GetHabbo().Id)
                return;

            GameClient Client = BiosEmuThiago.GetGame().GetClientManager().GetClientByUserID(Ticket.Sender.Id);
            if (Client != null)
            {
                Client.SendMessage(new ModeratorSupportTicketResponseComposer(Result));
            }

            if (Result == 2)
            {
                using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("UPDATE `user_info` SET `cfhs_abusive` = `cfhs_abusive` + 1 WHERE `user_id` = '" + Ticket.Sender.Id + "' LIMIT 1");
                }
            }

            Ticket.Answered = true;
            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(Session.GetHabbo().Id, Ticket), "mod_tool");
        }
    }
}