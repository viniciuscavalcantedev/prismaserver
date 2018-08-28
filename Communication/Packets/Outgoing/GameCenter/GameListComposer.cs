using System.Collections.Generic;

using Bios.HabboHotel.Games;

namespace Bios.Communication.Packets.Outgoing.GameCenter
{
	class GameListComposer : ServerPacket
    {
        public GameListComposer(ICollection<GameData> Games)
            : base(ServerPacketHeader.GameListMessageComposer)
        {
			WriteInteger(BiosEmuThiago.GetGame().GetGameDataManager().GetCount());//Game count
            foreach (GameData Game in Games)
            {
				WriteInteger(Game.GameId);
				WriteString(Game.GameName);
				WriteString(Game.ColourOne);
				WriteString(Game.ColourTwo);
				WriteString(Game.ResourcePath);
				WriteString(Game.StringThree);
            }
        }
    }
}
