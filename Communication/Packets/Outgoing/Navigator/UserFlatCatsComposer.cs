using System.Collections.Generic;

using Bios.HabboHotel.Navigator;

namespace Bios.Communication.Packets.Outgoing.Navigator
{
	class UserFlatCatsComposer : ServerPacket
    {
        public UserFlatCatsComposer(ICollection<SearchResultList> Categories, int Rank)
            : base(ServerPacketHeader.UserFlatCatsMessageComposer)
        {
			WriteInteger(Categories.Count);
            foreach (SearchResultList Cat in Categories)
            {
				WriteInteger(Cat.Id);
				WriteString(Cat.PublicName);
				WriteBoolean(Cat.RequiredRank <= Rank);
				WriteBoolean(false);
				WriteString("");
				WriteString("");
				WriteBoolean(false);
            }
        }
    }
}