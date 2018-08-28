using log4net;
using System.Collections.Generic;
using System.Data;

namespace Bios.HabboHotel.Rooms.TraxMachine
{
    public class TraxSoundManager
    {
        public static List<TraxMusicData> Songs = new List<TraxMusicData>();

        private static ILog Log = LogManager.GetLogger("Bios.HabboHotel.Rooms.TraxMachine");
        public static void Init()
        {
            Songs.Clear();

            DataTable table;
            using (var adap = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                adap.RunQuery("SELECT * FROM jukebox_songs_data");
                table = adap.getTable();
            }

            foreach (DataRow row in table.Rows)
            {
                Songs.Add(TraxMusicData.Parse(row));
            }

            Log.Info("» Jukebox -> Músicas PRONTO - BY: Thiago Araujo: [" + Songs.Count + "]");
        }

        public static TraxMusicData GetMusic(int id)
        {
            foreach (var item in Songs)
                if (item.Id == id)
                    return item;

            return null;
        }
    }
}
