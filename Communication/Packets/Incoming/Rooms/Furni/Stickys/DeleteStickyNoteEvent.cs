using Bios.Database.Interfaces;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Items;

namespace Bios.Communication.Packets.Incoming.Rooms.Furni.Stickys
{
    class DeleteStickyNoteEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (!Session.GetHabbo().InRoom)
                return;

            Room Room = null;
            if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room))
                return;

            if (!Room.CheckRights(Session))
                return;

            Item Item = Room.GetRoomItemHandler().GetItem(Packet.PopInt());
            if (Item == null)
                return;

            if (Item.GetBaseItem().InteractionType == InteractionType.POSTIT || Item.GetBaseItem().InteractionType == InteractionType.CAMERA_PICTURE)
            {
                Room.GetRoomItemHandler().RemoveFurniture(Session, Item.Id);
                using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("DELETE FROM `items` WHERE `id` = '" + Item.Id + "' LIMIT 1");
                }
            }
        }
    }
}
