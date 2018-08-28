using System.Linq;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Cache.Type;

namespace Bios.Communication.Packets.Outgoing.Rooms.Settings
{
	class GetRoomBannedUsersComposer : ServerPacket
    {
        public GetRoomBannedUsersComposer(Room Instance)
            : base(ServerPacketHeader.GetRoomBannedUsersMessageComposer)
        {
			WriteInteger(Instance.Id);

			WriteInteger(Instance.GetBans().BannedUsers().Count);//Count
            foreach (int Id in Instance.GetBans().BannedUsers().ToList())
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
