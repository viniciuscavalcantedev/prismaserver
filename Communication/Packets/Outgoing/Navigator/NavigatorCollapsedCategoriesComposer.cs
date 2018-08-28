namespace Bios.Communication.Packets.Outgoing.Navigator
{
    class NavigatorCollapsedCategoriesComposer : ServerPacket
    {
        public NavigatorCollapsedCategoriesComposer()
            : base(ServerPacketHeader.NavigatorCollapsedCategoriesMessageComposer)
        {
			WriteInteger(0);
        }
    }
}
