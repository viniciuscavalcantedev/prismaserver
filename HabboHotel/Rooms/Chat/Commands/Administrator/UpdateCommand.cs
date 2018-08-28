using System.Linq;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Communication.Packets.Outgoing.Catalog;
using Bios.Core;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Notifications;
using System.Text;
using Bios.Communication.Packets.Incoming.LandingView;
using Bios.HabboHotel.LandingView;
using Bios.HabboHotel.Rooms.TraxMachine;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class UpdateCommand : IChatCommand
    {
        public string PermissionRequired => "command_update";
        public string Parameters => "[VARIABLE]";
        public string Description => "Atualizar catalago.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {
            if (ExtraSettings.STAFF_EFFECT_ENABLED_ROOM)
            {
                if (Session.GetHabbo().isLoggedIn && Session.GetHabbo().Rank > Convert.ToInt32(BiosEmuThiago.GetConfig().data["MineRankStaff"]))
                {
                }
                else
                {
                    Session.SendWhisper("Você precisa estar logado como staff para usar este comando.");
                    return;
                }
            }
            if (Params.Length == 1)
            {
                StringBuilder List = new StringBuilder();
                List.Append("- LISTA DE COMANDOS STAFF -\n\n");
                List.Append(":update catalogo - Atualiza o catalogo.\n········································································\n");
                List.Append(":update items - Atualiza os items.\n········································································\n");
                List.Append(":update jukebox - Atualiza as musicas.\n········································································\n");
                List.Append(":update wordfilter - Atualiza o filtro do hotel.\n········································································\n");
                List.Append(":update models - Atualiza o filtro del hotel.\n········································································\n");
                List.Append(":update promotions - Atualiza as promoções.\n········································································\n");
                List.Append(":update halloffame - Atualiza pontos de fama.\n········································································\n");
                List.Append(":update youtube - Atualiza os videos TV's.\n········································································\n");
                List.Append(":update permissions - Atualiza as permissões de rank.\n········································································\n");
                List.Append(":update settings - Atualiza las configurações do hotel.\n········································································\n");
                List.Append(":update bans - Atualiza os banidos do hotel.\n········································································\n");
                List.Append(":update quests - Atualiza os Quests do hotel.\n········································································\n");
                List.Append(":update achievements - Atualiza is logs de usuarios.\n········································································\n");
                List.Append(":update bots - Atualiza os bots do hotel.\n········································································\n");
                List.Append(":update achievements - Atualiza os logros de usuarios.\n········································································\n");
                Session.SendMessage(new MOTDNotificationComposer(List.ToString()));
                return;
            }

            string UpdateVariable = Params[1];
            switch (UpdateVariable.ToLower())
            {
                case "cata":
                case "catalog":
                case "catalogo":
                case "catalogue":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_catalog"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão  'command_update_catalog' permiso", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetCatalog().Init(BiosEmuThiago.GetGame().GetItemManager());
                    BiosEmuThiago.GetGame().GetClientManager().SendMessage(new CatalogUpdatedComposer());
                    Session.LogsNotif("Catalogo atualizado corretamente", "catalogue");
                    break;

                case "discos":
                case "songs":
                case "jukebox":
                case "canciones":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_songsdata"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_songsdata", "advice");
                        break;
                    }
                    int count = TraxSoundManager.Songs.Count;
                    TraxSoundManager.Init();
                    Session.LogsNotif("Música recarregadas com sucesso, diferença de comprimento: " + checked(count - TraxSoundManager.Songs.Count), "advice");
                    break;

                case "wordfilter":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_filter"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_filter' permiso", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetChatManager().GetFilter().InitWords();
                    BiosEmuThiago.GetGame().GetChatManager().GetFilter().InitCharacters();
                    Session.LogsNotif("Filtro atualizado corretamente", "advice");
                    break;

                case "items":
                case "furni":
                case "furniture":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_furni"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_furni' permiso", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetItemManager().Init();
                    Session.LogsNotif("Items acualizados corretamente", "advice");
                    break;

                case "models":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_models"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_models' permiso", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetRoomManager().LoadModels();
                    Session.LogsNotif("Salas atualizadas corretamente.", "advice");
                    break;

                case "promotions":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_promotions"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_promotions' permiso.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetLandingManager().LoadPromotions();
                    Session.LogsNotif("Promoçoes atualizadas corretamente.", "advice");
                    break;

                case "halloffame":
                case "salondelafama":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_halloffame"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_halloffame' permiso.", "advice");
                        break;
                    }

                    GetHallOfFame.GetInstance().Load();
                    Session.LogsNotif("Hall of Fame atualizado com sucesso.", "advice");
                    break;

                case "youtube":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_youtube"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_youtube' permiso.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetTelevisionManager().Init();
                    Session.LogsNotif("TV's atualizados.", "advice");
                    break;

                case "navigator":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_navigator"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_navigator'.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetNavigator().Init();
                    Session.LogsNotif("Navegador de salas atualizado.", "advice");
                    break;

                case "ranks":
                case "rights":
                case "permissions":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_rights"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_rights'.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetPermissionManager().Init();

                    foreach (GameClient Client in BiosEmuThiago.GetGame().GetClientManager().GetClients.ToList())
                    {
                        if (Client == null || Client.GetHabbo() == null || Client.GetHabbo().GetPermissions() == null)
                            continue;

                        Client.GetHabbo().GetPermissions().Init(Client.GetHabbo());
                    }

                    Session.LogsNotif("Permissoes atualizadas.", "advice");
                    break;
                case "pinatas":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_catalog"))
                    {
                        Session.SendWhisper("Oops, Você não tem permissão para atualizar prêmios pinatas.");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetPinataManager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                    BiosEmuThiago.GetGame().GetClientManager().SendMessage(RoomNotificationComposer.SendBubble("catalogue", "Premios Actualizados", ""));
                    break;
                case "crafting":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_furni"))
                    {
                        Session.SendWhisper("Oops, Você não tem permissão para atualizar .");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetCraftingManager().Init();
                    Session.SendWhisper("Crafting actualizado correctamente.");
                    break;
                case "crackable":
                case "ecotron":
                case "pinata":
                case "piñata":
                    BiosEmuThiago.GetGame().GetPinataManager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                    BiosEmuThiago.GetGame().GetFurniMaticRewardsMnager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                    BiosEmuThiago.GetGame().GetTargetedOffersManager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                    break;

                case "relampago":
                case "targeted":
                case "targetedoffers":
                    BiosEmuThiago.GetGame().GetTargetedOffersManager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                    break;

                case "config":
                case "settings":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_configuration"))
                    {
                        Session.LogsNotif("Ups,Você não tem permissão 'command_update_configuration'.", "advice"); ;
                        break;
                    }

                    BiosEmuThiago.GetGame().GetSettingsManager().Init();
                    ExtraSettings.RunExtraSettings();
                    CatalogSettings.RunCatalogSettings();
                    NotificationSettings.RunNotiSettings();
                    BiosEmuThiago.GetGame().GetTargetedOffersManager().Initialize(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                    Session.LogsNotif("Configuraçoes atualizadas.", "advice");
                    break;

                case "bans":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_bans"))
                    {
                        Session.LogsNotif("Ups, Você não tem'command_update_bans' permiso.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetModerationManager().ReCacheBans();
                    Session.LogsNotif("Cache Ban re-PRONTO - BY: Thiago Araujo.", "advice");
                    break;

                case "quests":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_quests"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_quests' permiso.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetQuestManager().Init();
                    Session.LogsNotif("Quests atualizadas.", "advice");
                    break;

                case "achievements":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_achievements"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_achievements' permiso.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetAchievementManager().LoadAchievements();
                    Session.LogsNotif("Achievements atualizados.", "advice");
                    break;

                case "moderation":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_moderation"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_moderation' permiso.", "advice"); ;
                        break;
                    }

                    BiosEmuThiago.GetGame().GetModerationManager().Init();
                    BiosEmuThiago.GetGame().GetClientManager().ModAlert("Presets de moderación se han actualizado.Por favor, vuelva a cargar el cliente para ver los nuevos presets.");

                    Session.LogsNotif("Configurações dos moderadores atualizadas.", "advice");
                    break;

                case "vouchers":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_vouchers"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão'command_update_vouchers.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetCatalog().GetVoucherManager().Init();
                    Session.LogsNotif("O catálogo cache atualizado.", "advice");
                    break;

                case "gc":
                case "games":
                case "gamecenter":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_game_center"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_game_center'.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetGameDataManager().Init();
                    Session.LogsNotif("Cache Game Center foi atualizado com sucesso.", "advice");
                    break;

                case "pet_locale":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_pet_locale"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_pet_locale'.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetChatManager().GetPetLocale().Init();
                    BiosEmuThiago.GetGame().GetChatManager().GetPetCommands().Init();
                    Session.LogsNotif("Cache local Animais atualizado.", "advice");
                    break;

                case "locale":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_locale"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_locale'.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetLanguageManager().Init();
                    Session.LogsNotif("Locale caché acualizado corretamente.", "advice");
                    break;

                case "mutant":

                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_anti_mutant"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_anti_mutant'.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetFigureManager().Init();
                    Session.LogsNotif("FigureData manager recarregado com sucesso!", "advice");
                    break;

                case "bots":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_bots"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_bots'.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetBotManager().Init();
                    Session.LogsNotif("Bots actualizados.", "advice");
                    break;

                case "rewards":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_rewards"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_rewards'.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetRewardManager().Reload();
                    Session.LogsNotif("Gestor De Recompensas voltou a carregar com sucesso!", "advice");
                    break;

                case "chat_styles":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_chat_styles"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_chat_styles'.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetChatManager().GetChatStyles().Init();
                    Session.LogsNotif("estilos de chat recarregado com sucesso!", "advice");
                    break;

                case "bonus":
                    {
                        if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_bonus"))
                        {
                            Session.LogsNotif("Ups, Você não tem permissão 'command_update_bonus'.", "advice");
                            break;
                        }

                        BiosEmuThiago.GetGame().GetLandingManager().LoadBonusRare(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());
                        Session.SendWhisper("Os bônus de vista do hotel foram atualizados corretamente.");
                        break;
                    }

                case "definitions":
                case "badge_definitions":
                    if (!Session.GetHabbo().GetPermissions().HasCommand("command_update_badge_definitions"))
                    {
                        Session.LogsNotif("Ups, Você não tem permissão 'command_update_badge_definitions'.", "advice");
                        break;
                    }

                    BiosEmuThiago.GetGame().GetBadgeManager().Init();
                    Session.LogsNotif("Definições placas recarregado com sucesso!", "advice");
                    break;

                default:
                    Session.LogsNotif("'" + UpdateVariable + "' não é uma coisa válida para recarregar.", "advice");
                    break;
            }
        }
    }
}
