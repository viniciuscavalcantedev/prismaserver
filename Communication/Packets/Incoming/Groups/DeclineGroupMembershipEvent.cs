﻿using Bios.HabboHotel.Groups;
using Bios.Communication.Packets.Outgoing.Groups;

namespace Bios.Communication.Packets.Incoming.Groups
{
    class DeclineGroupMembershipEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GroupId = Packet.PopInt();
            int UserId = Packet.PopInt();

            Group Group = null;
            if (!BiosEmuThiago.GetGame().GetGroupManager().TryGetGroup(GroupId, out Group))
                return;

            if (Session.GetHabbo().Id != Group.CreatorId && !Group.IsAdmin(Session.GetHabbo().Id))
                return;

            if (!Group.HasRequest(UserId))
                return;

            Group.HandleRequest(UserId, false);
            Session.SendMessage(new UnknownGroupComposer(Group.Id, UserId));
        }
    }
}