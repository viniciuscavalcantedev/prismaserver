﻿using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Bios.Core;
using log4net;
using log4net.Config;
using Bios.Utilities;

namespace Bios
{
    public class Program
    {
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_CLOSE = 0xF060;

        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("kernel32.dll", ExactSpelling = true)]
        private static extern IntPtr GetConsoleWindow();

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]

        public static void Main(string[] Args)
        {
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), SC_CLOSE, MF_BYCOMMAND);

            XmlConfigurator.Configure();



            Console.ForegroundColor = ConsoleColor.White;
            Console.CursorVisible = false;
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;

            StartEverything();

            while (BiosEmuThiago.IsLive)
            {
                Console.CursorVisible = true;
                if (ExceptionLogger.DisabledState)
                    Console.Write("BIOS> ");

                ConsoleCommandHandler.InvokeCommand(Console.ReadLine());
                continue;
            }
        }

        private static void StartEverything()
        {
            Console.Clear();
            StartConsoleWindow();
            DeleteMenu(GetSystemMenu(GetConsoleWindow(), false), 61536, 0);
            InitEnvironment();
        }

        /// <summary>
        /// Starts the console window.
        /// </summary>
        public static void StartConsoleWindow()
        {
            Console.Clear();
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Green;

        }

        public static void InitEnvironment()
        {
            if (BiosEmuThiago.IsLive)
                return;

            Console.CursorVisible = false;
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;
            BiosEmuThiago.Initialize();
        }

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            ExceptionLogger.LogCriticalException(e);
            BiosEmuThiago.PerformShutDown(false);
        }

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private delegate bool EventHandler(CtrlType sig);
    }
}