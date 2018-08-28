

using Bios.HabboHotel.Games;


namespace Bios.Communication.Packets.Incoming.GameCenter
{
    class Game2GetWeeklyLeaderboardEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            int GameId = Packet.PopInt();

            GameData GameData = null;
            if (BiosEmuThiago.GetGame().GetGameDataManager().TryGetGame(GameId, out GameData))
            {
                //Code
            }
        }
    }
}
