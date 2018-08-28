using System;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;

namespace Bios.Communication.Packets.Incoming.Moderation
{
    class ModerationCautionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_caution"))
                return;

            int UserId = Packet.PopInt();
            String Message = Packet.PopString();

            GameClient Client = BiosEmuThiago.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client == null || Client.GetHabbo() == null)
                return;

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `user_info` SET `cautions` = `cautions` + '1' WHERE `user_id` = '" + Client.GetHabbo().Id + "' LIMIT 1");
            }

            Client.SendNotification(Message);
        }
    }
}