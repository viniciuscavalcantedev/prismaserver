using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.HabboHotel.GameClients;
using Bios.Core;

namespace Bios.Communication.Packets.Incoming.Moderation
{
    class AmbassadorAlert : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().Rank < ExtraSettings.AmbassadorMinRank) return;
            int userId = Packet.PopInt();
            GameClient user = BiosEmuThiago.GetGame().GetClientManager().GetClientByUserID(userId);
            if (user == null) return;
            user.SendMessage(new SuperNotificationComposer("", "${notification.ambassador.alert.warning.title}", "${notification.ambassador.alert.warning.message}", "", ""));
        }
    }
}