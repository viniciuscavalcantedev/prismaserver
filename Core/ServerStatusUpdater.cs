using System;
using System.Threading;
using System.Diagnostics;
using log4net;
using Bios.Database.Interfaces;
using Bios.HabboHotel;

namespace Bios.Core
{
    public class ServerStatusUpdater : IDisposable
    {
        private static ILog log = LogManager.GetLogger("Bios.Core.ServerStatusUpdater");
        private const int UPDATE_IN_SECS = 15;
        public static int _userPeak;

        private static string _lastDate;

        private static bool isExecuted;

        private static Stopwatch lowPriorityProcessWatch;
        private static Timer _mTimer;

        public static void Init()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            log.Info("» Atualização do servidor iniciada! - BY: Thiago Araujo");
            Console.ResetColor();
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE server_status SET status = '1'");
            }
            lowPriorityProcessWatch = new Stopwatch();
            lowPriorityProcessWatch.Start();
        }

        public static void StartProcessing()
        {
            _mTimer = new Timer(Process, null, 0, 10000);
        }

        internal static void Process(object caller)
        {
            if (lowPriorityProcessWatch.ElapsedMilliseconds >= 10000 || !isExecuted)
            {
                isExecuted = true;
                lowPriorityProcessWatch.Restart();

                var clientCount = BiosEmuThiago.GetGame().GetClientManager().Count;
                var loadedRoomsCount = BiosEmuThiago.GetGame().GetRoomManager().Count;
                var Uptime = DateTime.Now - BiosEmuThiago.ServerStarted;
                Game.SessionUserRecord = clientCount > Game.SessionUserRecord ? clientCount : Game.SessionUserRecord;
                Console.Title = string.Concat("BIOS EMULADOR [" + BiosEmuThiago.HotelName + "] » [" + clientCount + "] ON » [" + loadedRoomsCount + "] SALAS » [" + Uptime.Days + "] DÍAS » [" + Uptime.Hours + "] HORAS");

                using (var queryReactor = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    if (clientCount > _userPeak) _userPeak = clientCount;

                    _lastDate = DateTime.Now.ToShortDateString();
                    queryReactor.runFastQuery(string.Concat("UPDATE `server_status` SET `status` = '2', `users_online` = '", clientCount, "', `loaded_rooms` = '", loadedRoomsCount, "'"));
                }
            }
        }

        public void Dispose()
        {
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0', `status` = '0'");
            }

            _mTimer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}