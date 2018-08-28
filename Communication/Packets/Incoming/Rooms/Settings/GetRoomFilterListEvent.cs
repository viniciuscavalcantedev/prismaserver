using Bios.HabboHotel.Rooms;
using Bios.Communication.Packets.Outgoing.Rooms.Settings;

namespace Bios.Communication.Packets.Incoming.Rooms.Settings
{
	class GetRoomFilterListEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Instance = Session.GetHabbo().CurrentRoom;
            if (Instance == null)
                return;

            if (!Instance.CheckRights(Session))
                return;

            Session.SendMessage(new GetRoomFilterListComposer(Instance));
            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_SelfModRoomFilterSeen", 1);
        }
    }
}
