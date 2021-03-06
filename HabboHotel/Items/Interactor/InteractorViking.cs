﻿using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Database.Interfaces;
using Bios.HabboHotel.Rooms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios.HabboHotel.Items.Interactor
{
    class InteractorViking : IFurniInteractor
    {
        public void OnPlace(GameClients.GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClients.GameClient Session, Item Item)
        {
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

            if (Actor.CurrentEffect == 5)
            {
                int count = int.Parse(Item.ExtraData);
                if (count < 5)
                {
                    count++;
                    Item.ExtraData = count + "";
                    Item.UpdateState(true, true);
                }
                if (count == 5)
                {
                        DataRow presothiago = null;
                        using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("SELECT VinkingThiago FROM items WHERE user_id = '" + Session.GetHabbo().Id + "' AND base_item = '900177' AND id = '" + Item.Id + "'");
                            presothiago = dbClient.getRow();
                        }

                        if (Convert.ToBoolean(presothiago["VinkingThiago"]) == true)
                        {
                            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Actor.GetClient(), "ACH_ViciousViking", 1);
                            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                            {
                                dbClient.RunQuery("UPDATE items SET VinkingThiago = 'false' WHERE user_id = '" + Session.GetHabbo().Id + "' AND base_item = '900177' AND id = '" + Item.Id + "'");
                            }
                        }
                    else
                    {
                        Session.SendWhisper("Ops, essa casa dos vinking já foi queimada! (fix By: Thiago Araujo)");
                        return;
                    }
                }
                return;
            }
            else
            {
                Session.SendWhisper("Ops, você não esta com a tocha de fogo! digite o comando[:efeito 5] (fix By: Thiago Araujo)");
                return;
            }

        }

        public void OnWiredTrigger(Item Item)
        {
        }
    }
}
