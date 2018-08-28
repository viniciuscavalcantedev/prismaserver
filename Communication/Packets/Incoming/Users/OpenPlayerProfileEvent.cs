using System.Collections.Generic;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.Groups;
using Bios.Communication.Packets.Outgoing.Users;
using Bios.Database.Interfaces;


namespace Bios.Communication.Packets.Incoming.Users
{
    class OpenPlayerProfileEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int userID = Packet.PopInt();

            Habbo targetData = BiosEmuThiago.GetHabboById(userID);
            if (targetData == null)
            {
                Session.SendNotification("Ocorreu um erro ao encontrar o perfil do usuário.");
                return;
            }

            List<Group> groups = BiosEmuThiago.GetGame().GetGroupManager().GetGroupsForUser(targetData.Id);

            int friendCount = 0;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT COUNT(0) FROM `messenger_friendships` WHERE (`user_one_id` = @userid OR `user_two_id` = @userid)");
                dbClient.AddParameter("userid", userID);
                friendCount = dbClient.getInteger();
            }

            Session.SendMessage(new ProfileInformationComposer(targetData, Session, groups, friendCount));
        }
    }
}
