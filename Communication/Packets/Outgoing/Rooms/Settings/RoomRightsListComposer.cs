using System.Linq;

using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Cache.Type;

namespace Bios.Communication.Packets.Outgoing.Rooms.Settings
{
	class RoomRightsListComposer : ServerPacket
    {
        public RoomRightsListComposer(Room Instance)
            : base(ServerPacketHeader.RoomRightsListMessageComposer)
        {
			WriteInteger(Instance.Id);

			WriteInteger(Instance.UsersWithRights.Count);
            foreach (int Id in Instance.UsersWithRights.ToList())
            {
                UserCache Data = BiosEmuThiago.GetGame().GetCacheManager().GenerateUser(Id);
                if (Data == null)
                {
					WriteInteger(0);
					WriteString("Unknown Error");
                }
                else
                {
					WriteInteger(Data.Id);
					WriteString(Data.Username);
                }
            }
        }
    }
}
