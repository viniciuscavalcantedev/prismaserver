using System;
using Bios.HabboHotel.Rooms;
using Bios.Communication.Packets.Outgoing.Rooms.Engine;
using Bios.Database.Interfaces;


namespace Bios.Communication.Packets.Incoming.Navigator
{
    class EditRoomEventEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int RoomId = Packet.PopInt();
            string word;
            string Name = Packet.PopString();
            Name = BiosEmuThiago.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Name, out word) ? "Spam" : Name;
            string Desc = Packet.PopString();
            Desc = BiosEmuThiago.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Desc, out word) ? "Spam" : Desc;

            RoomData Data = BiosEmuThiago.GetGame().GetRoomManager().GenerateRoomData(RoomId);
            if (Data == null)
                return;

            if (Data.OwnerId != Session.GetHabbo().Id)
                return;//HAX

            if (Data.Promotion == null)
            {
                Session.SendNotification("Ops, parece que não há uma promoção nesta sala! ");
                return;
            }

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `room_promotions` SET `title` = @title, `description` = @desc WHERE `room_id` = " + RoomId + " LIMIT 1");
                dbClient.AddParameter("title", Name);
                dbClient.AddParameter("desc", Desc);
                dbClient.RunQuery();
            }

            Room Room;
            if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(Convert.ToInt32(RoomId), out Room))
                return;

            Data.Promotion.Name = Name;
            Data.Promotion.Description = Desc;
            Room.SendMessage(new RoomEventComposer(Data, Data.Promotion));
        }
    }
}
