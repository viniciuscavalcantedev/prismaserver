using Bios.HabboHotel.Groups;
using Bios.Database.Interfaces;
using Bios.Communication.Packets.Outgoing.Groups;

namespace Bios.Communication.Packets.Incoming.Groups
{
    class UpdateGroupIdentityEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            string word;
            string Name = Packet.PopString();
            Name = BiosEmuThiago.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Name, out word) ? "Spam" : Name;
            string Desc = Packet.PopString();
            Desc = BiosEmuThiago.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Desc, out word) ? "Spam" : Desc;

            Group Group = null;
            if (!BiosEmuThiago.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Group.CreatorId != Session.GetHabbo().Id)
                return;

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `groups` SET `name`= @name, `desc` = @desc WHERE `id` = @groupId LIMIT 1");
                dbClient.AddParameter("name", Name);
                dbClient.AddParameter("desc", Desc);
                dbClient.AddParameter("groupId", GroupId);
                dbClient.RunQuery();
            }

            Group.Name = Name;
            Group.Description = Desc;

            Session.SendMessage(new GroupInfoComposer(Group, Session));
        }
    }
}
