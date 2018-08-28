
using Bios.HabboHotel.Users.Effects;

namespace Bios.Communication.Packets.Outgoing.Inventory.AvatarEffects
{
	class AvatarEffectActivatedComposer : ServerPacket
    {
        public AvatarEffectActivatedComposer(AvatarEffect Effect)
            : base(ServerPacketHeader.AvatarEffectActivatedMessageComposer)
        {
			WriteInteger(Effect.SpriteId);
			WriteInteger((int)Effect.Duration);
			WriteBoolean(false);//Permanent
        }
    }
}