using Bios.Communication.Packets.Outgoing.Sound;
using Bios.HabboHotel.GameClients;


namespace Bios.Communication.Packets.Incoming.Sound
{
    class GetJukeboxPlayListEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session.GetHabbo().CurrentRoom != null)
                Session.SendMessage(new SetJukeboxPlayListComposer(Session.GetHabbo().CurrentRoom));
            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_MusicPlayer", 1);
        }
    }
}
