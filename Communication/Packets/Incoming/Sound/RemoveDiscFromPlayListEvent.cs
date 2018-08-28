using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.Communication.Packets.Incoming.Sound
{
    class RemoveDiscFromPlayListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var room = Session.GetHabbo().CurrentRoom;
            if (!room.CheckRights(Session))
                return;
            var itemindex = Packet.PopInt();

            var trax = room.GetTraxManager();
            if (trax.Playlist.Count < itemindex)
            {
                goto error;
            }

            var item = trax.Playlist[itemindex];
            if (!trax.RemoveDisc(item))
                goto error;

            return;
        error:
            Session.SendMessage(new RoomNotificationComposer("jukeboxBios", "message", "Ops, você não pode remove esse CD do jukebox!"));
        }
    }
}
