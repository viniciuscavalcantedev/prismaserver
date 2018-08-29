using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using MySql.Data.MySqlClient;
using Bios.Core;
using Bios.HabboHotel;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.Users.UserData;
using Bios.Communication.RCON;
using Bios.Communication.ConnectionManager;
using Bios.Utilities;
using log4net;
using System.Collections.Concurrent;
using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.Communication.Encryption.Keys;
using Bios.Communication.Encryption;
using Bios.Database.Interfaces;
using Bios.HabboHotel.Cache.Type;
using Bios.Database;

namespace Bios
{
    public static class BiosEmuThiago
    {
        private static readonly ILog log = LogManager.GetLogger("Bios.BiosEmuThiago");
        public static string HotelName;
        public static string Licenseto;
        public static bool IsLive;
		public static string CurrentTime = DateTime.Now.ToString("hh:mm:ss tt" + "- [BIOS] ");
		public const string PrettyVersion = "BIOS EMULADOR ";
        public const string PrettyBuild = " 1.1.2 ";
        public const string ServerVersion = " 1.1.1 ";
        public const string VersionBios = "ThiagoPremium";
        public const string LastUpdate = " 02/09/2017 ";

        private static Encoding _defaultEncoding;
        public static CultureInfo CultureInfo;

        internal static object UnixTimeStampToDateTime(double timestamp)
        {
            throw new NotImplementedException();
        }

        private static Game _game;
        private static ConfigurationData _configuration;
        private static ConnectionHandling _connectionManager;
        private static DatabaseManager _manager;
        private static RCONSocket _rcon;

        // TODO: Get rid?
        public static bool Event = false;
        public static DateTime lastEvent;
        public static DateTime ServerStarted;

        private static readonly List<char> Allowedchars = new List<char>(new[]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x',
                'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '-', '.'
            });

        private static ConcurrentDictionary<int, Habbo> _usersCached = new ConcurrentDictionary<int, Habbo>();
        public static string SWFRevision = "";
        public static int Quartovip;
        public static int Prisao;

        public static void Initialize()
        {
            ServerStarted = DateTime.Now;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;


            Console.WriteLine("");
            Console.WriteLine(@"© 2016 - 2017 SaoDev Corporation Ltd. Todos os direitos reservados ao Thiago Araujo.");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine(
                Console.LargestWindowWidth > 30
                ? @"-------------------------------------------------------------------------------------------------------------"
                : @"");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("");
            _defaultEncoding = Encoding.Default;

            Console.Write("VERIFICANDO LICENSIA: ");
            string passthiago = "servidoresdabiosdevCreditosThiagoAraujoSAODevLTDHappy";

            if (ExtraSettings.LICENSE != passthiago)
            {
                Console.Clear();
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(@"             (            (          )            *   )   )                     (           ");
                Console.WriteLine(@"             )\  (     (  )\ ) (  ( /(          ` )  /(( /((     ) (  (         )\   (      )   (  (       ");
                Console.WriteLine(@"           (((_) )(   ))\(()/( )\ )\())(  (      ( )(_))\())\ ( /( )\))( (   ((((_)( )(  ( /(  ))\ )\  (   ");
                Console.WriteLine(@"           )\___(()\ /((_)((_))(_)_))/ )\ )\ _  (_(_())(_)((_))(_))(_))\ )\   )\ _ )(()\ )(_))/((_)(_) )\  ");
                Console.WriteLine(@"          ((/ __|((_)_))  _| | (_) |_ ((_)(_)_) |_   _| |(_)_)(_)_ (()(_)(_)  (_)_\(_)(_)(_)_(_))(  ! ((_) ");
                Console.WriteLine(@"           | (__| '_/ -_) _` | | |  _/ _ (_-<_    | | | ' \| / _` / _` / _ \   / _ \| '_/ _` | || || / _ \ ");
                Console.WriteLine(@"            \___|_| \___\__,_| |_|\__\___/__(_)   |_| |_||_|_\__,_\__, \___/  /_/ \_\_| \__,_|\_,_|/ \___/ ");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Title = ("BIOSEMU - OPS ALGO ESTA ERRADO COM SUA LICENSIA!");
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(@"             (            (          )            *   )   )                     (           ");
                Console.WriteLine(@"             )\  (     (  )\ ) (  ( /(          ` )  /(( /((     ) (  (         )\   (      )   (  (       ");
                Console.WriteLine(@"           (((_) )(   ))\(()/( )\ )\())(  (      ( )(_))\())\ ( /( )\))( (   ((((_)( )(  ( /(  ))\ )\  (   ");
                Console.WriteLine(@"           )\___(()\ /((_)((_))(_)_))/ )\ )\ _  (_(_())(_)((_))(_))(_))\ )\   )\ _ )(()\ )(_))/((_)(_) )\  ");
                Console.WriteLine(@"          ((/ __|((_)_))  _| | (_) |_ ((_)(_)_) |_   _| |(_)_)(_)_ (()(_)(_)  (_)_\(_)(_)(_)_(_))(  ! ((_) ");
                Console.WriteLine(@"           | (__| '_/ -_) _` | | |  _/ _ (_-<_    | | | ' \| / _` / _` / _ \   / _ \| '_/ _` | || || / _ \ ");
                Console.WriteLine(@"            \___|_| \___\__,_| |_|\__\___/__(_)   |_| |_||_|_\__,_\__, \___/  /_/ \_\_| \__,_|\_,_|/ \___/ ");
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("            Licensia do emulador não é valida para o BiosEmulador, Fale com o dono do pack (Thiago Araujo)!");
                Console.WriteLine(" ");
                Console.WriteLine("                                Clique em qualquer tecla para desligar o BIOSEMU");
                Console.ReadKey();
                Environment.Exit(1);
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"           (       )  (            *          (           (        )  (     ");
            Console.WriteLine(@"        (  )\ ) ( /(  )\ )       (  `         )\ )   (    )\ )  ( /(  )\ )  ");
            Console.WriteLine(@"      ( )\(()/( )\())(()/(   (   )\))(     ( (()/(   )\  (()/(  )\())(()/(  ");
            Console.WriteLine(@"      )((_)/(_))(_)\  /(_))  )\ ((_)()\    )\ /(_))(((_)( /(_))((_)\  /(_)) ");
            Console.WriteLine(@"     ((_)_(_))   ((_)(_))   ((_)(_()((_)_ ((_)_))  )\ _ )(_))_   ((_)(_))   ");
            Console.WriteLine(@"      | _ )_ _| / _ \/ __|  | __|  \/  | | | | |   (_)_\(_)   \ / _ \| _ \  ");
            Console.WriteLine(@"      | _ \| | | (_) \__ \  | _|| |\/| | |_| | |__  / _ \ | |) | (_) |   /  ");
            Console.WriteLine(@"      |___/___| \___/|___/  |___|_|  |_|\___/|____|/_/ \_\|___/ \___/|_|_\  ");
            Console.WriteLine("");
            Console.WriteLine(@"© 2016 - 2017 SaoDev Corporation Ltd. Todos os direitos reservados ao Thiago Araujo.");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.WriteLine(
                Console.LargestWindowWidth > 30
                ? @"-------------------------------------------------------------------------------------------------------------"
                : @"");
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Title = "BIOS EMULADOR | Carregando...";
            CultureInfo = CultureInfo.CreateSpecificCulture("en-GB");
            try
            {
                _configuration = new ConfigurationData(Path.Combine(Application.StartupPath, @"BiosConfingThiago/ConfigBiosEmuThiago.ini"));

                var connectionString = new MySqlConnectionStringBuilder
                {
                    ConnectionTimeout = 10,
                    Database = GetConfig().data["db.name"],
                    DefaultCommandTimeout = 30,
                    Logging = false,
                    MaximumPoolSize = uint.Parse(GetConfig().data["db.pool.maxsize"]),
                    MinimumPoolSize = uint.Parse(GetConfig().data["db.pool.minsize"]),
                    Password = GetConfig().data["db.password"],
                    Pooling = true,
                    Port = uint.Parse(GetConfig().data["db.port"]),
                    Server = GetConfig().data["db.hostname"],
                    UserID = GetConfig().data["db.username"],
                    AllowZeroDateTime = true,
                    ConvertZeroDateTime = true,
                };

                _manager = new DatabaseManager(connectionString.ToString());

                if (!_manager.IsConnected())
                {
                    log.Warn("» Existe uma conexão com o banco de dados já existente ou há um problema para se conectar a ele.");
                    Console.ReadKey(true);
                    Environment.Exit(1);
                    return;
                }

                log.Info("» Conectado ao banco de dados!");

                #region Add 2016
                HotelName = Convert.ToString(GetConfig().data["hotel.name"]);
                Licenseto = Convert.ToString(GetConfig().data["license"]);
                #endregion Add 2016

                //Reset our statistics first.
                using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.runFastQuery("TRUNCATE `catalog_marketplace_data`");
                    dbClient.runFastQuery("UPDATE `rooms` SET `users_now` = '0' WHERE `users_now` > '0'");
                    dbClient.runFastQuery("UPDATE `users` SET `online` = '0' WHERE `online` = '1'");
                    dbClient.runFastQuery("UPDATE `server_status` SET `users_online` = '0', `loaded_rooms` = '0', `status` = '1'");
                }

                _game = new Game();
                _game.ContinueLoading();

                //Have our encryption ready.
                HabboEncryptionV2.Initialize(new RSAKeys());

                //Make sure MUS is working.
                _rcon = new RCONSocket(GetConfig().data["mus.tcp.bindip"], int.Parse(GetConfig().data["mus.tcp.port"]), GetConfig().data["mus.tcp.allowedaddr"].Split(Convert.ToChar(";")));

                //Accept connections.
                _connectionManager = new ConnectionHandling(int.Parse(GetConfig().data["game.tcp.port"]), int.Parse(GetConfig().data["game.tcp.conlimit"]), int.Parse(GetConfig().data["game.tcp.conperip"]), GetConfig().data["game.tcp.enablenagles"].ToLower() == "true");
                _connectionManager.Init();

                //_game.StartGameLoop();
                TimeSpan TimeUsed = DateTime.Now - ServerStarted;

                Quartovip = int.Parse(GetConfig().data["Quartovip"]);
                Prisao = int.Parse(GetConfig().data["Prisao"]);

                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;
                log.Info("» BIOSEMULADOR -> LISTO!! (" + TimeUsed.Seconds + " s, " + TimeUsed.Milliseconds + " ms)");
                Console.ResetColor();
                IsLive = true;

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("");
                Console.WriteLine(
                    Console.LargestWindowWidth > 30
                    ? @"-------------------------------------------------------------------------------------------------------------"
                    : @"");
                Console.ForegroundColor = ConsoleColor.Gray;

            }
            catch (KeyNotFoundException e)
            {
                log.ErrorFormat("Verifique o seu arquivo de configuração - alguns valores parecem estar faltando.", ConsoleColor.Red);
                log.Error("Pressione qualquer tecla para desligar ...");
                ExceptionLogger.LogException(e);
                Console.ReadKey(true);
                Environment.Exit(1);
                return;
            }
            catch (InvalidOperationException e)
            {
                log.Error("Falha ao inicializar BIOS EMULADOR:" + e.Message);
                log.Error("Pressione qualquer tecla para desligar ...");
                Console.ReadKey(true);
                Environment.Exit(1);
                return;
            }
            catch (Exception e)
            {
                log.Error("Erro fatal durante a inicialização:" + e);
                log.Error("Pressione uma tecla para sair");

                Console.ReadKey();
                Environment.Exit(1);
            }
        }

        public static bool EnumToBool(string Enum)
        {
            return (Enum == "1");
        }

        public static string BoolToEnum(bool Bool)
        {
            return (Bool == true ? "1" : "0");
        }

        public static int GetRandomNumber(int Min, int Max)
        {
            return RandomNumber.GenerateNewRandom(Min, Max);
        }

        public static string Rainbow()
        {
            int numColors = 1000;
            var colors = new List<string>();
            var random = new Random();
            for (int i = 0; i < numColors; i++)
            {
                colors.Add(String.Format("#{0:X2}{1:X2}00", i, random.Next(0x1000000) - i));
            }

            int index = 0;
            string rainbow = colors[index];

            if (index > numColors)
                index = 0;
             else
                index++;

            return rainbow;
        }

        public static string RainbowT()
        {
            int numColorst = 1000;
            var colorst = new List<string>();
            var randomt = new Random();
            for (int i = 0; i < numColorst; i++)
            {
                colorst.Add(String.Format("#{0:X6}", randomt.Next(0x1000000)));
            }

            int indext = 0;
            string rainbowt = colorst[indext];

            if (indext > numColorst)
                indext = 0;
            else
                indext++;

            return rainbowt;
        }

        public static double GetUnixTimestamp()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return ts.TotalSeconds;
        }

        internal static int GetIUnixTimestamp()
        {
            var ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            var unixTime = ts.TotalSeconds;
            return Convert.ToInt32(unixTime);
        }

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long CurrentTimeMillis()
        {
            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }


        public static long Now()
        {
            TimeSpan ts = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            double unixTime = ts.TotalMilliseconds;
            return (long)unixTime;
        }

        public static string FilterFigure(string figure)
        {
            foreach (char character in figure)
            {
                if (!IsValid(character))
                    return "sh-3338-93.ea-1406-62.hr-831-49.ha-3331-92.hd-180-7.ch-3334-93-1408.lg-3337-92.ca-1813-62";
            }

            return figure;
        }

        private static bool IsValid(char character)
        {
            return Allowedchars.Contains(character);
        }

        public static bool IsValidAlphaNumeric(string inputStr)
        {
            inputStr = inputStr.ToLower();
            if (string.IsNullOrEmpty(inputStr))
            {
                return false;
            }

            for (int i = 0; i < inputStr.Length; i++)
            {
                if (!IsValid(inputStr[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public static string GetUsernameById(int UserId)
        {
            string Name = "Unknown User";

            GameClient Client = GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Client != null && Client.GetHabbo() != null)
                return Client.GetHabbo().Username;

            UserCache User = GetGame().GetCacheManager().GenerateUser(UserId);
            if (User != null)
                return User.Username;

            using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT `username` FROM `users` WHERE id = @id LIMIT 1");
                dbClient.AddParameter("id", UserId);
                Name = dbClient.getString();
            }

            if (string.IsNullOrEmpty(Name))
                Name = "Unknown User";

            return Name;
        }

        public static bool ShutdownStarted { get; set; }
		
        public static Habbo GetHabboById(int UserId)
        {
            try
            {
                GameClient Client = GetGame().GetClientManager().GetClientByUserID(UserId);
                if (Client != null)
                {
                    Habbo User = Client.GetHabbo();
                    if (User != null && User.Id > 0)
                    {
                        if (_usersCached.ContainsKey(UserId))
                            _usersCached.TryRemove(UserId, out User);
                        return User;
                    }
                }
                else
                {
                    try
                    {
                        if (_usersCached.ContainsKey(UserId))
                            return _usersCached[UserId];
                        else
                        {
                            UserData data = UserDataFactory.GetUserData(UserId);
                            if (data != null)
                            {
                                Habbo Generated = data.user;
                                if (Generated != null)
                                {
                                    Generated.InitInformation(data);
                                    _usersCached.TryAdd(UserId, Generated);
                                    return Generated;
                                }
                            }
                        }
                    }
                    catch { return null; }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static Habbo GetHabboByUsername(String UserName)
        {
            try
            {
                using (IQueryAdapter dbClient = GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT `id` FROM `users` WHERE `username` = @user LIMIT 1");
                    dbClient.AddParameter("user", UserName);
                    int id = dbClient.getInteger();
                    if (id > 0)
                        return GetHabboById(Convert.ToInt32(id));
                }
                return null;
            }
            catch { return null; }
        }



        public static void PerformShutDown()
        {
            PerformShutDown(false);
        }

        public static void PerformRestart()
        {
            PerformShutDown(true);
            using (IQueryAdapter dbClient = _manager.GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `server_status` SET `status` = '1'");
            }
        }
		
        public static void PerformShutDown(bool restart)
        {
            Console.Clear();
            log.Info("Desligando o servidor...");
            Console.Title = "BIOSEMULADOR: DESLIGANDO!";

            ShutdownStarted = true;

            GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(GetGame().GetLanguageManager().TryGetValue("server.shutdown.message")));
            GetGame().StopGameLoop();
            System.Threading.Thread.Sleep(2500);
            GetConnectionManager().Destroy();//Stop listening.
            GetGame().GetPacketManager().UnregisterAll();//Unregister the packets.
            GetGame().GetPacketManager().WaitForAllToComplete();
            GetGame().GetClientManager().CloseAll();//Close all connections
            GetGame().GetRoomManager().Dispose();//Stop the game loop.

            GetConnectionManager().Destroy();

            using (IQueryAdapter dbClient = _manager.GetQueryReactor())
            {
                dbClient.runFastQuery("TRUNCATE `catalog_marketplace_data`");
                dbClient.runFastQuery("UPDATE `users` SET online = '0', `auth_ticket` = NULL");
                dbClient.runFastQuery("UPDATE `rooms` SET `users_now` = '0'");
            }

            _connectionManager.Destroy();
            _game.Destroy();

            log.Info("BIOS EMULADOR foi desligado com exito.");

            if (!restart)
                log.WarnFormat("Desligado Completo. Presione una tecla para continuar...", ConsoleColor.Blue);

            if (!restart)
                Console.ReadKey();

            IsLive = false;

            if (restart)
                Process.Start(Assembly.GetEntryAssembly().Location);

            if (restart)
                Console.WriteLine("Reiniciando...");
            else
                Console.WriteLine("Finalizando...");

            System.Threading.Thread.Sleep(1000);
            Environment.Exit(0);
        }

        public static ConfigurationData GetConfig()
        {
            return _configuration;
        }

        public static Encoding GetDefaultEncoding()
        {
            return _defaultEncoding;
        }

        public static ConnectionHandling GetConnectionManager()
        {
            return _connectionManager;
        }

        public static Game GetGame()
        {
            return _game;
        }

        public static RCONSocket GetRCONSocket()
        {
            return _rcon;
        }

        public static DatabaseManager GetDatabaseManager()
        {
            return _manager;
        }

        public static ICollection<Habbo> GetUsersCached()
        {
            return _usersCached.Values;
        }

        public static bool RemoveFromCache(int Id, out Habbo Data)
        {
            return _usersCached.TryRemove(Id, out Data);
        }
    }
}