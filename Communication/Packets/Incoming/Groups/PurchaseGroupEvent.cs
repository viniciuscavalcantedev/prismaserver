﻿using System;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Groups;
using Bios.Communication.Packets.Outgoing.Groups;
using Bios.Communication.Packets.Outgoing.Catalog;
using Bios.Communication.Packets.Outgoing.Rooms.Session;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Communication.Packets.Outgoing.Moderation;

namespace Bios.Communication.Packets.Incoming.Groups
{
    class PurchaseGroupEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket packet)
        {
            string word;
            string Name = packet.PopString();
            Name = BiosEmuThiago.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Name, out word) ? "Spam" : Name;
            string Description = packet.PopString();
            Description = BiosEmuThiago.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Description, out word) ? "Spam" : Description;
            int RoomId = packet.PopInt();
            int Colour1 = packet.PopInt();
            int Colour2 = packet.PopInt();
            int Unknown = packet.PopInt();

            int groupCost = Convert.ToInt32(BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("catalog.group.purchase.cost"));

            if (Session.GetHabbo().Credits < groupCost)
            {
                Session.SendMessage(new BroadcastMessageAlertComposer("Um grupo custa " + groupCost + " creditos! E você tem " + Session.GetHabbo().Credits + "!"));
                return;
            }
            else
            {
                Session.GetHabbo().Credits -= groupCost;
                Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
            }

            RoomData Room = BiosEmuThiago.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            if (Room == null || Room.OwnerId != Session.GetHabbo().Id || Room.Group != null)
                return;

            string Badge = string.Empty;

            for (int i = 0; i < 5; i++)
            {
                Badge += BadgePartUtility.WorkBadgeParts(i == 0, packet.PopInt().ToString(), packet.PopInt().ToString(), packet.PopInt().ToString());
            }

            Group Group = null;
            if (!BiosEmuThiago.GetGame().GetGroupManager().TryCreateGroup(Session.GetHabbo(), Name, Description, RoomId, Badge, Colour1, Colour2, out Group))
            {
                Session.SendNotification("Houve um erro ao tentar criar este grupo.\n\nTenta de novo.Se você receber esta mensagem mais de uma vez, fale com o moderador.\r\r");
                return;
            }

            Session.SendMessage(new PurchaseOKComposer());

            Room.Group = Group;

            if (Session.GetHabbo().CurrentRoomId != Room.Id)
                Session.SendMessage(new RoomForwardComposer(Room.Id));

            Session.SendMessage(new NewGroupInfoComposer(RoomId, Group.Id));
        }
    }
}