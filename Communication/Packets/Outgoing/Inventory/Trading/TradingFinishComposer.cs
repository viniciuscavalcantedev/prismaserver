namespace Bios.Communication.Packets.Outgoing.Inventory.Trading
{
	class TradingFinishComposer : ServerPacket
    {
        public TradingFinishComposer()
            : base(ServerPacketHeader.TradingFinishMessageComposer)
        {
        }
    }
}
