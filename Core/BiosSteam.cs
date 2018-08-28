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
    class BiosSteam
    {
        public static string LICENSE = "";
        private static readonly ILog log = LogManager.GetLogger("Bios.BiosEmuThiago");

        public static bool RunLicenseKey()
        {
            if (!File.Exists("BiosConfingThiago/license.ini"))
                return false;
            foreach (var @params in from line in File.ReadAllLines("BiosConfingThiago/license.ini", Encoding.Default) where !String.IsNullOrWhiteSpace(line) && line.Contains("=") select line.Split('='))
            {
                switch (@params[0])
                {
                    case "license":
                        LICENSE = @params[1];
                        break;
                }
            }
            return true;
        }
    }
}