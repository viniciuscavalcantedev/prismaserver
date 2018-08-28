using System.Collections.Generic;
using Bios.HabboHotel.Groups;

namespace Bios.Communication.Packets.Outgoing.Catalog
{
    class GroupFurniConfigComposer : ServerPacket
    {
        public GroupFurniConfigComposer(ICollection<Group> Groups)
            : base(ServerPacketHeader.GroupFurniConfigMessageComposer)
        {
			WriteInteger(Groups.Count);
            foreach (Group Group in Groups)
            {
				WriteInteger(Group.Id);
				WriteString(Group.Name);
				WriteString(Group.Badge);
				WriteString(BiosEmuThiago.GetGame().GetGroupManager().GetColourCode(Group.Colour1, true));
				WriteString(BiosEmuThiago.GetGame().GetGroupManager().GetColourCode(Group.Colour2, false));
				WriteBoolean(false);
				WriteInteger(Group.CreatorId);
				WriteBoolean(Group.ForumEnabled);
            }
        }
    }
}
