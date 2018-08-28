
using Bios.HabboHotel.Groups;
using Bios.Communication.Packets.Outgoing.Groups;
using Bios.Database.Interfaces;
using Bios.Communication.Packets.Outgoing.Users;
using Bios.HabboHotel.Rooms;

namespace Bios.Communication.Packets.Incoming.Groups
{
    class SetGroupFavouriteEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            int GroupId = Packet.PopInt();
            if (GroupId == 0)
                return;

            Group Group = null;
            if (!BiosEmuThiago.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            Session.GetHabbo().GetStats().FavouriteGroupId = Group.Id;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `user_stats` SET `groupid` = @groupId WHERE `id` = @userId LIMIT 1");
                dbClient.AddParameter("groupId", Session.GetHabbo().GetStats().FavouriteGroupId);
                dbClient.AddParameter("userId", Session.GetHabbo().Id);
                dbClient.RunQuery();
            }

            if (Session.GetHabbo().InRoom && Session.GetHabbo().CurrentRoom != null)
            {
                Session.GetHabbo().CurrentRoom.SendMessage(new RefreshFavouriteGroupComposer(Session.GetHabbo().Id));
                if (Group != null)
                {
                    Session.GetHabbo().CurrentRoom.SendMessage(new HabboGroupBadgesComposer(Group));

                    RoomUser User = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                    if (User != null)
                    Session.GetHabbo().CurrentRoom.SendMessage(new UpdateFavouriteGroupComposer(Session.GetHabbo().Id, Group, User.VirtualId));
                }
            }
            else
                Session.SendMessage(new RefreshFavouriteGroupComposer(Session.GetHabbo().Id));
        }
    }
}
