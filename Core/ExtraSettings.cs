#region

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

#endregion

namespace Bios.Core
{
    class ExtraSettings
    {

        public static bool WELCOME_MESSAGE_ENABLED = true;
        public static bool TARGETED_OFFERS_ENABLED = true;
        public static bool WELCOME_NEW_MESSAGE_ENABLED = true;
        public static string WELCOME_MESSAGE_URL = "habbopages/bienvenidax.txt";
        public static string COMMAND_USER_URL = "";
        public static string COMMAND_STAFF_URL = "";
        public static bool STAFF_EFFECT_ENABLED_ROOM = true;
        public static bool STAFF_MENSG_ENTERTHIAGO = true;
        public static bool DEBUG_ENABLED = true;
        public static string YOUTUBE_THUMBNAIL_SUBURL = "youtubethumbnail.php?Video";
        public static string LICENSE = "© 2016 - 2017 SaoDev Corporation Ltd. Todos os direitos reservados ao Thiago Araujo.";
        public static bool CAMERA_ENABLE = true;
        public static string CAMERA_API = "";
        public static string CAMERA_OUTPUT_PICTURES = "http://localhost/camera/pictures/";
        public static int CAMERA_PRICECOINS = 20;
        public static bool CAMERA_ALERT = true;
        public static int CAMERA_PRICEDUCKETS = 10;
        public static int CAMERA_PUBLISHPRICE = 10;
        public static string CAMERA_ITEMID = "202030";
        public static string CAMERA_MAXCACHE = "1000";
        public static int AmbassadorMinRank;
        public static string PTOS_COINS = "";
        public static string WelcomeMessage = "";
        public static readonly ILog log = LogManager.GetLogger("Bios.Core");

        public static bool RunExtraSettings()
        {
            if (File.Exists("BiosConfingThiago/MensagensBiosEmuThiago/Bem-vindoBiosEmuThiago.txt"))
                WelcomeMessage = File.ReadAllText("BiosConfingThiago/MensagensBiosEmuThiago/Bem-vindoBiosEmuThiago.txt");
            if (!File.Exists("BiosConfingThiago/ExtraBiosEmuThiago.ini"))
                return false;
            foreach (var @params in from line in File.ReadAllLines("BiosConfingThiago/ExtraBiosEmuThiago.ini", Encoding.Default) where !String.IsNullOrWhiteSpace(line) && line.Contains("=") select line.Split('='))
            {
                switch (@params[0])
                {
                    case "welcome.message.enabled":
                        WELCOME_MESSAGE_ENABLED = @params[1] == "true";
                        break;
                    case "targeted.offers.enabled":
                        TARGETED_OFFERS_ENABLED = @params[1] == "true";
                        break;
                    case "welcome.new.message.enabled":
                        WELCOME_NEW_MESSAGE_ENABLED = @params[1] == "true";
                        break;
                    case "welcome.message.url":
                        WELCOME_MESSAGE_URL = @params[1];
                        break;
                    case "youtube.thumbnail.suburl":
                        YOUTUBE_THUMBNAIL_SUBURL = @params[1];
                        break;
                    case "licensethiago":
                        LICENSE = @params[1];
                        break;
                    case "camera.photo.purchase.price.coins":
                        CAMERA_PRICECOINS = int.Parse(@params[1]);
                        break;
                    case "camera.photo.purchase.price.duckets":
                        CAMERA_PRICEDUCKETS = int.Parse(@params[1]);
                        break;
                    case "camera.photo.publish.price.duckets":
                        CAMERA_PUBLISHPRICE = int.Parse(@params[1]);
                        break;
                    case "camera.photo.purchase.item_id":
                        CAMERA_ITEMID = @params[1];
                        break;
                    case "camera.api.http":
                        CAMERA_API = @params[1];
                        break;
                    case "camera.output.pictures":
                        CAMERA_OUTPUT_PICTURES = @params[1];
                        break;
                    case "camera.picture.purchase.alert.id":
                        CAMERA_ALERT = @params[1] == "true";
                        break;
                    case "camera.enable":
                        CAMERA_ENABLE = @params[1] == "true";
                        break;
                    case "staff.effect.inroom":
                        STAFF_EFFECT_ENABLED_ROOM = @params[1] == "true";
                        break;
                    case "staff.mensg.inroom":
                        STAFF_MENSG_ENTERTHIAGO = @params[1] == "true";
                        break;
                    case "debug.enabled":
                        DEBUG_ENABLED = @params[1] == "true";
                        break;
                    case "coin.points.name":
                        PTOS_COINS = @params[1];
                        break;
                    case "ambassador.minrank":
                        AmbassadorMinRank = int.Parse(@params[1]);
                        break;
                    case "command.users.url":
                        COMMAND_USER_URL = @params[1];
                        break;
                    case "command.staff.url":
                        COMMAND_STAFF_URL = @params[1];
                        break;
                }
            }
            log.Info("» Configurações BiosConfingThiago -> PRONTO - BY: Thiago Araujo!");
            return true;
        }
    }
}