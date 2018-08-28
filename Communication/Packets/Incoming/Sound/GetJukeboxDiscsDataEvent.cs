using System.Collections.Generic;
using Bios.Communication.Packets.Outgoing.Sound;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Rooms.TraxMachine;

namespace Bios.Communication.Packets.Incoming.Sound
{
    class GetJukeboxDiscsDataEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var songslen = Packet.PopInt();
            var Songs = new List<TraxMusicData>();
            while (songslen-- > 0)
            {
                var id = Packet.PopInt();
                var music = TraxSoundManager.GetMusic(id);
                if (music != null)
                    Songs.Add(music);
            }
            if (Session.GetHabbo().CurrentRoom != null)
                Session.SendMessage(new SetJukeboxSongMusicDataComposer(Songs));
        }
    }
}
