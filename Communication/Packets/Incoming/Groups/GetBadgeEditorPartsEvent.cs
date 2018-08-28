using Bios.Communication.Packets.Outgoing.Groups;

namespace Bios.Communication.Packets.Incoming.Groups
{
    class GetBadgeEditorPartsEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            Session.SendMessage(new BadgeEditorPartsComposer(
                BiosEmuThiago.GetGame().GetGroupManager().BadgeBases,
                BiosEmuThiago.GetGame().GetGroupManager().BadgeSymbols,
                BiosEmuThiago.GetGame().GetGroupManager().BadgeBaseColours,
                BiosEmuThiago.GetGame().GetGroupManager().BadgeSymbolColours,
                BiosEmuThiago.GetGame().GetGroupManager().BadgeBackColours));
        }
    }
}
