using System;
using log4net;
using Bios.Communication.Packets.Outgoing.Catalog;

using Bios.Communication.Packets.Outgoing.Moderation;

namespace Bios.Core
{
    public class ConsoleCommandHandler
    {
        private static readonly ILog log = LogManager.GetLogger("Bios.Core.ConsoleCommandHandler");

        public static void InvokeCommand(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
                return;

            try
            {
                #region Command parsing
                string[] parameters = inputData.Split(' ');

                switch (parameters[0].ToLower())
                {
                    #region targeted
                    case "relampago":
                    case "targeted":
                        BiosEmuThiago.GetGame().GetTargetedOffersManager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                        break;
                    #endregion targeted
                    #region stop
                    case "stop":
                    case "fechar":
                    case "fecharemulador":
                    case "desligar":
                    case "shutdown":
                        ExceptionLogger.DisablePrimaryWriting(true);
                        log.Warn("O servidor está economizando móveis de usuários, salas, etc. ESPERE QUE O SERVIDOR FECHE, NÃO SAIA DO PROCESSO NO GERADOR DE TAREFAS !!");
                        BiosEmuThiago.PerformShutDown(false);
                        break;
                    #endregion
                    #region actualizarcatalogo
                    case "catalog":
                    case "catalogue":
                    case "actualizarcatalogo":
                        BiosEmuThiago.GetGame().GetCatalog().Init(BiosEmuThiago.GetGame().GetItemManager());
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new CatalogUpdatedComposer());
                        break;
                    #endregion
                    #region actualizaritems
                    case "items":
                    case "furnis":
                    case "furniture":
                        BiosEmuThiago.GetGame().GetItemManager().Init();
                        break;
                    #endregion
                    #region restart
                    case "restart":
                    case "reiniciar":
                        ExceptionLogger.DisablePrimaryWriting(true);
                        log.Warn("O servidor está economizando móveis de usuários, salas, etc. ESPERE QUE O SERVIDOR FECHE, NÃO SAIA DO PROCESSO NO GERADOR DE TAREFAS !!");
                        BiosEmuThiago.PerformShutDown(true);
                        break;
                    #endregion
                    #region Clear Console
                    case "clear":
                        Console.Clear();
                        break;
                    #endregion
                    #region alert
                    case "alert":
                        string Notice = inputData.Substring(6);
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new BroadcastMessageAlertComposer(BiosEmuThiago.GetGame().GetLanguageManager().TryGetValue("server.console.alert") + "\n\n" + Notice));
                        log.Info("Alerta enviada correctamente.");
                        break;
                    #endregion
                    #region navigator
                    case "navi":
                    case "navegador":
                    case "navigator":
                        BiosEmuThiago.GetGame().GetNavigator().Init();
                        break;
                    #endregion
                    #region configs
                    case "config":
                    case "settings":
                        BiosEmuThiago.GetGame().GetSettingsManager().Init();
                        ExtraSettings.RunExtraSettings();
                        CatalogSettings.RunCatalogSettings();
                        NotificationSettings.RunNotiSettings();
                        BiosEmuThiago.GetGame().GetTargetedOffersManager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                        break;
                    #endregion

                    default:
                        log.Error(parameters[0].ToLower() + " is an unknown or unsupported command. Type help for more information");
                        break;
                }
                #endregion
            }
            catch (Exception e)
            {
                log.Error("Error in command [" + inputData + "]: " + e);
            }
        }
    }
}
