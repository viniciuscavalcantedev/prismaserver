using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Help.Helpers;
using Bios.HabboHotel.Helpers;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.Communication.Packets.Incoming.Help.Helpers
{
    class HandleHelperToolEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().Rank > 2 || Session.GetHabbo()._guidelevel > 0)
            {

                var onDuty = Packet.PopBoolean();
                var isGuide = Packet.PopBoolean();
                var isHelper = Packet.PopBoolean();
                var isGuardian = Packet.PopBoolean();
                if (onDuty)
                    HelperToolsManager.AddHelper(Session, isHelper, isGuardian, isGuide);
                else
                    HelperToolsManager.RemoveHelper(Session);
                Session.SendMessage(new HandleHelperToolComposer(onDuty));
            }
            else
            {
                Session.SendMessage(new RoomNotificationComposer("Ops, você não pode usar essa ferramenta!", ""));
            }

        }
    }
}
