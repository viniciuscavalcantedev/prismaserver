﻿using System;

using Bios.HabboHotel.Items;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Rooms.Games;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Rooms.Games.Teams;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.HabboHotel.Items.Interactor
{
    class InteractorPressurePad : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClient Session, Item Item)
        {
            if (Session == null || Session.GetHabbo() == null || Item == null)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser Actor = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (Actor == null)
                return;

            if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) > 2)
                return;

            int count = int.Parse(Item.ExtraData);
            count++;
            Item.ExtraData = count + "";
            Item.UpdateState(true, true);
            Session.SendMessage(new RoomNotificationComposer("Piso mudo de cor! dsadsa"));
        }

        public void OnTrigger(GameClients.GameClient Session, Item Item, int Request, bool HasRights)
        {
            if (Session == null || Session.GetHabbo() == null || Item == null)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            RoomUser Actor = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (Actor == null)
                return;

            if (Gamemap.TileDistance(Actor.X, Actor.Y, Item.GetX, Item.GetY) > 2)
                return;

            int count = int.Parse(Item.ExtraData);
                count++;
                Item.ExtraData = count + "";
                Item.UpdateState(true, true);
                Session.SendMessage(new RoomNotificationComposer("Piso mudo de cor!"));
        }

        public void OnWiredTrigger(Item Item)
        {

        }
    }
}