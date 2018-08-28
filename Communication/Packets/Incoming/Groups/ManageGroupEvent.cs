using Bios.HabboHotel.Groups;
using Bios.Communication.Packets.Outgoing.Groups;

namespace Bios.Communication.Packets.Incoming.Groups
{
    class ManageGroupEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();

            Group Group = null;
            if (!BiosEmuThiago.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Group.CreatorId != Session.GetHabbo().Id && !Session.GetHabbo().GetPermissions().HasRight("group_management_override"))
                return;

            Session.SendMessage(new ManageGroupComposer(Group, Group.Badge.Replace("b", "").Split('s')));
        }
    }
}
