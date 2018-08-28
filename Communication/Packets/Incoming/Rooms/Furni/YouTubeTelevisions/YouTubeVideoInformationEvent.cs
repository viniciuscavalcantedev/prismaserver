using System.Linq;

using Bios.HabboHotel.Items.Televisions;
using Bios.Communication.Packets.Outgoing.Rooms.Furni.YouTubeTelevisions;

namespace Bios.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions
{
	class YouTubeVideoInformationEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int ItemId = Packet.PopInt();
            string VideoId = Packet.PopString();

            foreach (TelevisionItem Tele in BiosEmuThiago.GetGame().GetTelevisionManager().TelevisionList.ToList())
            {
                if (Tele.YouTubeId != VideoId)
                    continue;

                Session.SendMessage(new GetYouTubeVideoComposer(ItemId, Tele.YouTubeId));
            }
        }
    }
}