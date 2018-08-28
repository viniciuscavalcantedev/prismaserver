using System.Collections.Generic;
using Bios.HabboHotel.LandingView.Promotions;
using Bios.Communication.Packets.Outgoing.LandingView;

namespace Bios.Communication.Packets.Incoming.LandingView
{
    class GetPromoArticlesEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            ICollection<Promotion> LandingPromotions = BiosEmuThiago.GetGame().GetLandingManager().GetPromotionItems();
            Session.SendMessage(new PromoArticlesComposer(LandingPromotions));
            BiosEmuThiago.GetGame().GetLandingManager().LoadHallOfFame();
        }
    }
}
