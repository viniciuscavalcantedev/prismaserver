using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Groups.Forums;

namespace Bios.Communication.Packets.Outgoing.Groups
{
	class ThreadUpdatedComposer : ServerPacket
    {
        public ThreadUpdatedComposer(GameClient Session, GroupForumThread Thread)
            : base(ServerPacketHeader.ThreadUpdatedMessageComposer)
        {
			WriteInteger(Thread.ParentForum.Id);

            Thread.SerializeData(Session, this);
        }
    }
}
