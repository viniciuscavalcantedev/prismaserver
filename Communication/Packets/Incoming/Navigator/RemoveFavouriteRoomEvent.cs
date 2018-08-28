using Bios.Communication.Packets.Outgoing.Navigator;

using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Users;
using Bios.Communication.Packets.Incoming;

namespace Bios.Communication.Packets.Incoming.Navigator
{
    public class RemoveFavouriteRoomEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int Id = Packet.PopInt();

            Session.GetHabbo().FavoriteRooms.Remove(Id);
            Session.SendMessage(new UpdateFavouriteRoomComposer(Id, false));

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("DELETE FROM user_favorites WHERE user_id = " + Session.GetHabbo().Id + " AND room_id = " + Id + " LIMIT 1");
            }
        }
    }
}