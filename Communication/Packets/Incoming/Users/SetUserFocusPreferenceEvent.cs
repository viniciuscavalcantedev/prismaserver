using Bios.HabboHotel.GameClients;
using Bios.Database.Interfaces;


namespace Bios.Communication.Packets.Incoming.Users
{
    class SetUserFocusPreferenceEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            bool FocusPreference = Packet.PopBoolean();

            Session.GetHabbo().FocusPreference = FocusPreference;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `focus_preference` = @focusPreference WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                dbClient.AddParameter("focusPreference", BiosEmuThiago.BoolToEnum(FocusPreference));
                dbClient.RunQuery();
            }
        }
    }
}
