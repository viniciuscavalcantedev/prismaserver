using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Helpers;

namespace Bios.Communication.Packets.Incoming.Help.Helpers
{
    class CloseHelperChatSessionEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var Element = HelperToolsManager.GetElement(Session);

            if (Element != null)
            {
                Element.End();
                if (Element.OtherElement != null)
                    Element.OtherElement.End();
            }
        }
    }
}
