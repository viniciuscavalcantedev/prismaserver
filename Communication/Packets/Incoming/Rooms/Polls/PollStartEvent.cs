using Bios.HabboHotel.Rooms.Polls;
using Bios.Communication.Packets.Outgoing.Rooms.Polls;

namespace Bios.Communication.Packets.Incoming.Rooms.Polls
{
    class PollStartEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient session, ClientPacket packet)
        {
            int pollId = packet.PopInt();

            RoomPoll poll = null;
            if (!BiosEmuThiago.GetGame().GetPollManager().TryGetPoll(pollId, out poll))
                return;

            session.SendMessage(new PollContentsComposer(poll));
        }
    }
}
