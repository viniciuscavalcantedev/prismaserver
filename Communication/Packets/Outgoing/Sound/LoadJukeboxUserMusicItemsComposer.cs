﻿
using System.Collections.Generic;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Items;

namespace Bios.Communication.Packets.Outgoing.Sound
{
    class LoadJukeboxUserMusicItemsComposer : ServerPacket
    {
        public LoadJukeboxUserMusicItemsComposer(Room room)
            : base(ServerPacketHeader.LoadJukeboxUserMusicItemsMessageComposer)
        {
            var songs = room.GetTraxManager().GetAvaliableSongs();

			WriteInteger(songs.Count);//while

            foreach (var item in songs)
            {
				WriteInteger(item.Id);//item id
				WriteInteger(item.ExtradataInt);//Song id
            }
        }

        public LoadJukeboxUserMusicItemsComposer(ICollection<Item> Items)
            : base(ServerPacketHeader.LoadJukeboxUserMusicItemsMessageComposer)
        {

			WriteInteger(Items.Count);//while

            foreach (var item in Items)
            {
				WriteInteger(item.Id);//item id
				WriteInteger(item.ExtradataInt);//Song id
            }
        }
    }
}