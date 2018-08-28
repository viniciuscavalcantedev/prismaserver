using System.Collections.Generic;

using Bios.Utilities;
using Bios.HabboHotel.Users;
using Bios.Database.Interfaces;
using Bios.HabboHotel.Moderation;
using Bios.Communication.Packets.Outgoing.Moderation;

namespace Bios.Communication.Packets.Incoming.Moderation
{
    class SubmitNewTicketEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            // Run a quick check to see if we have any existing tickets.
            if (BiosEmuThiago.GetGame().GetModerationManager().UserHasTickets(Session.GetHabbo().Id))
            {
                ModerationTicket PendingTicket = BiosEmuThiago.GetGame().GetModerationManager().GetTicketBySenderId(Session.GetHabbo().Id);
                if (PendingTicket != null)
                {
                    Session.SendMessage(new CallForHelpPendingCallsComposer(PendingTicket));
                    return;
                }
            }

            List<string> Chats = new List<string>();

            string Message = StringCharFilter.Escape(Packet.PopString().Trim());
            int Category = Packet.PopInt();
            int ReportedUserId = Packet.PopInt();
            int Type = Packet.PopInt();// Unsure on what this actually is.

            Habbo ReportedUser = BiosEmuThiago.GetHabboById(ReportedUserId);
            if (ReportedUser == null)
            {
                // User doesn't exist.
                return;
            }

            int Messagecount = Packet.PopInt();
            for (int i = 0; i < Messagecount; i++)
            {
                Packet.PopInt();
                Chats.Add(Packet.PopString());
            }

            ModerationTicket Ticket = new ModerationTicket(1, Type, Category, UnixTimestamp.GetNow(), 1, Session.GetHabbo(), ReportedUser, Message, Session.GetHabbo().CurrentRoom, Chats);
            if (!BiosEmuThiago.GetGame().GetModerationManager().TryAddTicket(Ticket))
                return;

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                // TODO: Come back to this.
                /*dbClient.SetQuery("INSERT INTO `moderation_tickets` (`score`,`type`,`status`,`sender_id`,`reported_id`,`moderator_id`,`message`,`room_id`,`room_name`,`timestamp`) VALUES (1, '" + Category + "', 'open', '" + Session.GetHabbo().Id + "', '" + ReportedUserId + "', '0', @message, '0', '', '" + BiosEmuThiago.GetUnixTimestamp() + "')");
                dbClient.AddParameter("message", Message);
                dbClient.RunQuery();*/

                dbClient.runFastQuery("UPDATE `user_info` SET `cfhs` = `cfhs` + '1' WHERE `user_id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
            }

            BiosEmuThiago.GetGame().GetClientManager().ModAlert("Um novo ticket de suporte foi enviado!");
            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new ModeratorSupportTicketComposer(Session.GetHabbo().Id, Ticket), "mod_tool");
        }
    }
}
