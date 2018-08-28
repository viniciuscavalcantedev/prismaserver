using System;
using Bios.HabboHotel.GameClients;
using Bios.Database.Interfaces;


namespace Bios.Communication.Packets.Incoming.Users
{
    class SetChatPreferenceEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Boolean ChatPreference = Packet.PopBoolean();

            Session.GetHabbo().ChatPreference = ChatPreference;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `chat_preference` = @chatPreference WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                dbClient.AddParameter("chatPreference", BiosEmuThiago.BoolToEnum(ChatPreference));
                dbClient.RunQuery();
            }
        }
    }
}
