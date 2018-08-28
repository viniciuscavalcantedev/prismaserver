using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Items;
using Bios.HabboHotel.Items.Wired;
using Bios.Communication.Packets.Outgoing.Rooms.Furni.Wired;

namespace Bios.Communication.Packets.Incoming.Rooms.Furni.Wired
{
    class SaveWiredConfigEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null || !Room.CheckRights(Session, false, true))
                return;

            int ItemId = Packet.PopInt();

            Session.SendMessage(new HideWiredConfigComposer());

            Item SelectedItem = Room.GetRoomItemHandler().GetItem(ItemId);
            if (SelectedItem == null)
                return;

			if (!Session.GetHabbo().CurrentRoom.GetWired().TryGet(ItemId, out IWiredItem Box))
				return;

			if (Box.Type == WiredBoxType.EffectGiveUserBadge && !Session.GetHabbo().GetPermissions().HasRight("room_item_wired_rewards"))
            {
                Session.SendNotification("Não tem premisão para fazer isso.");
                return;
            }

            Box.HandleSave(Packet);
            Session.GetHabbo().CurrentRoom.GetWired().SaveBox(Box);
        }
    }
}
