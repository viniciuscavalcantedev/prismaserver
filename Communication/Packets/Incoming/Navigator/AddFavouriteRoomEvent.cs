﻿using Bios.Communication.Packets.Outgoing.Navigator;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;

namespace Bios.Communication.Packets.Incoming.Navigator
{
    public class AddFavouriteRoomEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null)
                return;

            int RoomId = Packet.PopInt();

            if (Session.GetHabbo().FavoriteRooms.Count >= 30 || Session.GetHabbo().FavoriteRooms.Contains(RoomId))
            {
                // send packet that favourites is full.
                return;
            }

            Session.GetHabbo().FavoriteRooms.Add(RoomId);
            Session.SendMessage(new UpdateFavouriteRoomComposer(RoomId, true));

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("INSERT INTO user_favorites (user_id,room_id) VALUES (" + Session.GetHabbo().Id + "," + RoomId + ")");
            }
        }
    }
}