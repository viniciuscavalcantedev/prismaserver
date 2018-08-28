using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Groups.Forums;

namespace Bios.Communication.Packets.Outgoing.Groups
{
	class PostUpdatedComposer : ServerPacket
    {
        public PostUpdatedComposer(GameClient Session, GroupForumThreadPost Post)
            : base(ServerPacketHeader.PostUpdatedMessageComposer)
        {
			WriteInteger(Post.ParentThread.ParentForum.Id);
			WriteInteger(Post.ParentThread.Id);

            Post.SerializeData(this);
        }
    }
}
