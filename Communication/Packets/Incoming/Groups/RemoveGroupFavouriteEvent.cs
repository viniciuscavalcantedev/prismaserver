﻿using Bios.Communication.Packets.Outgoing.Groups;
using Bios.HabboHotel.Rooms;

namespace Bios.Communication.Packets.Incoming.Groups
{
    class RemoveGroupFavouriteEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.GetHabbo().GetStats().FavouriteGroupId = 0;

            if (Session.GetHabbo().InRoom)
            {
                RoomUser User = Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (User != null)
                    Session.GetHabbo().CurrentRoom.SendMessage(new UpdateFavouriteGroupComposer(Session.GetHabbo().Id, null, User.VirtualId));
                Session.GetHabbo().CurrentRoom.SendMessage(new RefreshFavouriteGroupComposer(Session.GetHabbo().Id));
            }
            else
                Session.SendMessage(new RefreshFavouriteGroupComposer(Session.GetHabbo().Id));
        }
    }
}
