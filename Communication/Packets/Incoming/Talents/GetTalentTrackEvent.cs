using System.Collections.Generic;
using Bios.HabboHotel.Achievements;
using Bios.Communication.Packets.Outgoing.Talents;

namespace Bios.Communication.Packets.Incoming.Talents
{
    class GetTalentTrackEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            List<Talent> talents = BiosEmuThiago.GetGame().GetTalentManager().GetTalents(Type, -1);

            if (talents == null)
                return;

            Session.SendMessage(new TalentTrackComposer(Session, Type, talents));
        }
    }
}
