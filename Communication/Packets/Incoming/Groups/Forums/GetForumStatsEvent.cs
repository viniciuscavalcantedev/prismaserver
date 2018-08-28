using Bios.Communication.Packets.Outgoing.Groups;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Groups.Forums;

namespace Bios.Communication.Packets.Incoming.Groups
{
    class GetForumStatsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var GroupForumId = Packet.PopInt();

            GroupForum Forum;
            if (!BiosEmuThiago.GetGame().GetGroupForumManager().TryGetForum(GroupForumId, out Forum))
            {
                BiosEmuThiago.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("forums_thread_hidden", "O fórum que você está tentando acessar não existe mais.", ""));
                return;
            }

            Session.SendMessage(new ForumDataComposer(Forum, Session));

        }
    }
}
