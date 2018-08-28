
using Bios.HabboHotel.Users.Effects;

namespace Bios.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
	class AvatarEffectExpiredComposer : ServerPacket
    {
        public AvatarEffectExpiredComposer(AvatarEffect Effect)
            : base(ServerPacketHeader.AvatarEffectExpiredMessageComposer)
        {
			WriteInteger(Effect.SpriteId);
        }
    }
}
