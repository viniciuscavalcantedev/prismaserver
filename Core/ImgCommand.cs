﻿#region

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using log4net;

#endregion

namespace Bios.Core
{
    class NotificationSettings
    {
        public static string NOTIFICATION_EVENT_IMG = "";
        public static string NOTIFICATION_ABOUT_IMG = "";
        public static string NOTIFICATION_FILTER_IMG = "";
        public static string NOTIFICATION_OLE_IMG = "";
        public static string NOTIFICATION_PUBLI_IMG = "";
        public static readonly ILog log = LogManager.GetLogger("Bios.Core");

        public static bool RunNotiSettings()
        {
            if (!File.Exists("BiosConfingThiago/NotificaBiosEmuThiago/ConfigBiosThiago.ini"))
                return false;
            foreach (var @params in from line in File.ReadAllLines("BiosConfingThiago/NotificaBiosEmuThiago/ConfigBiosThiago.ini", Encoding.Default) where !String.IsNullOrWhiteSpace(line) && line.Contains("=") select line.Split('='))
            {
                switch (@params[0])
                {
                    case "notification.event.img":
                        NOTIFICATION_EVENT_IMG = @params[1];
                        break;
                    case "notification.about.img":
                        NOTIFICATION_ABOUT_IMG = @params[1];
                        break;
                    case "notification.filter.img":
                        NOTIFICATION_FILTER_IMG = @params[1];
                        break;
                    case "notification.da2.img":
                        NOTIFICATION_OLE_IMG = @params[1];
                        break;
                    case "notification.publi.img":
                        NOTIFICATION_PUBLI_IMG = @params[1];
                        break;
                }

            }
            log.Info("» Notificações BIOS -> PRONTO - BY: Thiago Araujo!");
            return true;
        }
    }
}