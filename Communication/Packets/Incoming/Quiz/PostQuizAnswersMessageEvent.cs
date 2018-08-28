using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Quiz;

namespace Bios.Communication.Packets.Incoming.Quiz
{
    class PostQuizAnswersMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new PostQuizAnswersMessageComposer(Session));
        }
    }
}
