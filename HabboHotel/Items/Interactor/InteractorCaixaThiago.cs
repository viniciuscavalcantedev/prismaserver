using Bios.Communication.Packets.Outgoing.Catalog;
using Bios.Communication.Packets.Outgoing.Inventory.Furni;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Communication.Packets.Outgoing.Users;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Rooms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace Bios.HabboHotel.Items.Interactor
{
    class InteractorCaixaThiago : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
            Item.ExtraData = "0";
            Item.UpdateNeeded = true;

        }

        public void OnRemove(GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            RoomUser User = null;
            if (Session != null)
                User = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            //  +-+-+-+-+-+ +-+-+-+-+-+
            //  |Arca Grega Fixado by: Thiago Araujo
            //   +-+-+-+-+-+ +-+-+-+-+-+

            if (Item.BaseItem == 94593)
            {
                if (Session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id).CurrentEffect == 186)
                {
                    Random random = new Random();
                    int mobi1 = 94594; // Chimera Grega
                    int mobi2 = 94595; // Hydra Grega
                    int mobi3 = 1100001496; // Minotauro Grego
                    int mobi4 = 94592; // Centauro Grego
                    string mobi01 = "Chimera Grega"; //Certo
                    string imagem01 = "santorini_r17_chimera"; //Certo
                    string mobi02 = "Hydra Grega"; //Certo
                    string imagem02 = "santorini_r17_hydra"; //Certo
                    string mobi03 = "Minotauro Grego"; //Certo
                    string imagem03 = "santorini_r17_minotaur"; //Certo
                    string mobi04 = "Centauro Grego"; //Certo
                    string imagem04 = "santorini_r17_centaur"; //Certo
                    int randomNumber = random.Next(1, 4);
                    if (Item.UserID != Session.GetHabbo().Id)
                    {
                        Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Esta caixa grega não te pertence."));
                        return;
                    }
                    Room Room = Session.GetHabbo().CurrentRoom;
                    Room.GetRoomItemHandler().RemoveFurniture(null, Item.Id);
                    if (randomNumber == 1)
                    {
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi1 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                        Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi1, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.SendMessage(new RoomNotificationComposer("icons/" + imagem01 + "_icon", 3, "Você acaba de ganha o raro grego : " + mobi01 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                        if (Session.GetHabbo().Rank == 1)
                        {
                            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem01 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na caixa grega o raro: " + mobi01, " !"));
                        }
                        return;
                    }
                    if (randomNumber == 2)
                    {
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi2 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                        Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi2, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.SendMessage(new RoomNotificationComposer("icons/" + imagem02 + "_icon", 3, "Você acaba de ganha o raro grego : " + mobi02 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                        if (Session.GetHabbo().Rank == 1)
                        {
                            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem02 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na caixa grega o raro: " + mobi02, " !"));
                        }
                        return;
                    }
                    if (randomNumber == 3)
                    {
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi3 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                        Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi3, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.SendMessage(new RoomNotificationComposer("icons/" + imagem03 + "_icon", 3, "Você acaba de ganha o raro grego : " + mobi03 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                        if (Session.GetHabbo().Rank == 1)
                        {
                            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem03 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na caixa grega o raro: " + mobi03, " !"));
                        }
                        return;
                    }
                    if (randomNumber == 4)
                    {
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi4 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                        }
                        Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi4, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                        Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                        Session.SendMessage(new RoomNotificationComposer("icons/" + imagem04 + "_icon", 3, "Você acaba de ganha o raro grego : " + mobi04 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                        if (Session.GetHabbo().Rank == 1)
                        {
                            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem04 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na caixa grega o raro: " + mobi04, " !"));
                        }
                        return;
                    }
                }
                else
                {
                    Session.SendWhisper("Ops, você não esta com a varinha! digite o comando[:efeito 186] (fix By: Thiago Araujo)");
                    return;
                }
            }

                //  +-+-+-+-+-+ +-+-+-+-+-+
                //  |Caixa HC Fixado by: Thiago Araujo
                //   +-+-+-+-+-+ +-+-+-+-+-+

                if (Item.BaseItem == 9324 || Item.BaseItem == 1346781567)
            {
                if (Item.UserID != Session.GetHabbo().Id)
                {
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Esta caixa HC não te pertence."));
                    return;
                }
                Room Room = Session.GetHabbo().CurrentRoom;
                Room.GetRoomItemHandler().RemoveFurniture(null, Item.Id);

                if (Item.BaseItem == 9324)
                {
                    int mobi1 = 756; // 1C
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi1 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi1, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    int num = num = 31 * 1;
                    Session.GetHabbo().GetClubManager().AddOrExtendSubscription("habbo_vip", num * 24 * 3600, Session);
                    Session.GetHabbo().GetBadgeComponent().GiveBadge("HC1", true, Session);
                    BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_BasicClub", 1);
                    BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_VipHC", 1);
                    Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
                    Session.SendMessage(new FurniListUpdateComposer());
                    return;
                }

                if (Item.BaseItem == 1346781567)
                {
                    int mobi1 = 756; // 1C
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi1 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi1, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    int num = num = 31 * 1;
                    Session.GetHabbo().GetClubManager().AddOrExtendSubscription("habbo_vip", num * 24 * 3600, Session);
                    Session.GetHabbo().GetBadgeComponent().GiveBadge("HC1", true, Session);
                    BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_BasicClub", 1);
                    BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_VipHC", 1);
                    Session.SendMessage(new ScrSendUserInfoComposer(Session.GetHabbo()));
                    Session.SendMessage(new FurniListUpdateComposer());
                    return;
                }

            }
            //  +-+-+-+-+-+ +-+-+-+-+-+
            //  |Arca Roxa Fixado by: Thiago Araujo
            //   +-+-+-+-+-+ +-+-+-+-+-+

            if (Item.BaseItem == 2334197)
            {
                Random random = new Random();
                int mobi1 = 8212; // Fonte Roxa
                int mobi2 = 8213; // Dragon Roxo
                int mobi3 = 8214; // Ventilador Roxo
                int mobi4 = 8215; // Sorveteira Roxa
                int mobi5 = 8216; // Ventilador Safira
                int mobi6 = 8216; // Alombar Roxo
                int mobi7 = 8218; // Bonnier Roxo
                int mobi8 = 8219; // Porta espacial Roxa
                int mobi9 = 8220; // Elefante Roxo
                string mobi01 = "Fonte roxa"; //Certo
                string imagem01 = "rare_fountain_4"; //Certo
                string mobi02 = "Dragão Roxo"; //Certo
                string imagem02 = "rare_dragonlamp_10"; //Certo
                string mobi03 = "Ventilador Roxo"; //Certo
                string imagem03 = "rare_fan_10"; //Certo
                string mobi04 = "Sorveteira Roxa"; //Certo
                string imagem04 = "rare_icecream_11"; //Certo
                string mobi05 = "Ventilador Safira"; //Certo
                string imagem05 = "rare_colourable_fan_1"; //Certo
                string mobi06 = "Alombar Roxo"; //Certo
                string imagem06 = "wooden_screen_10"; //Certo
                string mobi07 = "Bonier Roxo"; //Certo
                string imagem07 = "pillow_10"; //Certo
                string mobi08 = "Porta Espacial Roxa"; //Certo
                string imagem08 = "scifiport_10"; //Certo
                string mobi09 = "Elefante Roxo"; //Certo
                string imagem09 = "rare_elephant_statue_3"; //Certo
                int randomNumber = random.Next(1, 9);
                if (Item.UserID != Session.GetHabbo().Id)
                {
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Esta arca de raros perdidos azuis não te pertence."));
                    return;
                }
                Room Room = Session.GetHabbo().CurrentRoom;
                Room.GetRoomItemHandler().RemoveFurniture(null, Item.Id);
                if (randomNumber == 1)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi1 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi1, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem01 + "_icon", 3, "Você acaba de ganha o raro perdido roxo : " + mobi01 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem01 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos roxo, o raro: " + mobi01, " !"));
                    }
                    return;
                }
                if (randomNumber == 2)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi2 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi2, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem02 + "_icon", 3, "Você acaba de ganha o raro perdido roxo : " + mobi02 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem02 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos roxo, o raro: " + mobi02, " !"));
                    }
                    return;
                }
                if (randomNumber == 3)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi3 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi3, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem03 + "_icon", 3, "Você acaba de ganha o raro perdido roxo : " + mobi03 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem03 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos roxo, o raro: " + mobi03, " !"));
                    }
                    return;
                }
                if (randomNumber == 4)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi4 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi4, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem04 + "_icon", 3, "Você acaba de ganha o raro perdido roxo : " + mobi04 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem04 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos roxo, o raro: " + mobi04, " !"));
                    }
                    return;
                }
                if (randomNumber == 5)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi5 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi5, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem05 + "_icon", 3, "Você acaba de ganha o raro perdido roxo : " + mobi05 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem05 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos roxo, o raro: " + mobi05, " !"));
                    }
                    return;
                }
                if (randomNumber == 6)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi6 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi6, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem06 + "_icon", 3, "Você acaba de ganha o raro perdido roxo : " + mobi06 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem06 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos roxo, o raro: " + mobi06, " !"));
                    }
                    return;
                }
                if (randomNumber == 7)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi7 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi7, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem07 + "_icon", 3, "Você acaba de ganha o raro perdido roxo : " + mobi07 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem07 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos roxo, o raro: " + mobi07, " !"));
                    }
                    return;
                }
                if (randomNumber == 8)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi8 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi8, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem08 + "_icon", 3, "Você acaba de ganha o raro perdido roxo : " + mobi08 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem08 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos roxo, o raro: " + mobi08, " !"));
                    }
                    return;
                }
                if (randomNumber == 9)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi9 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi9, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem09 + "_icon", 3, "Você acaba de ganha o raro perdido roxo : " + mobi09 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem09 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos roxo, o raro: " + mobi09, " !"));
                    }
                    return;
                }
            }
            ///Fim da Arca Roxa by: Thiago Araujo

            //  +-+-+-+-+-+ +-+-+-+-+-+
            //  |Arca Azul Fixado by: Thiago Araujo
            //   +-+-+-+-+-+ +-+-+-+-+-+

            if (Item.BaseItem == 9503)
            {
                Random random = new Random();
                int mobi1 = 8836; // Dragão de Fogo Safira
                int mobi2 = 8846; // Barreira Safira
                int mobi3 = 8839; // Âmbar Safira
                int mobi4 = 8841; // Mamute Safira
                int mobi5 = 8843; // Ventilador Safira
                int mobi6 = 8833; // Fonte Safira
                int mobi7 = 8840; // Sorveteira Safira
                int mobi8 = 8831; // Toldo Safira
                int mobi9 = 8837; // Parasol Safira
                int mobi10 = 8838; // Pilar Dórico Safira
                int mobi11 = 8844; // Bonnie Safira
                int mobi12 = 8842; // Porta Espacial Safira
                int mobi13 = 8845; // Porta Laser Safira
                int mobi14 = 8832; // Máquina de Fumaça Safira
                int mobi15 = 8835; // Bimbo Oriental Safira
                string mobi01 = "Dragão de Fogo Safira"; //Certo
                string imagem01 = "rare_colourable_dragonlamp_1"; //Certo
                string mobi02 = "Barreira Safira"; //Certo
                string imagem02 = "rare_colourable_barrier_1"; //Certo
                string mobi03 = "Âmbar Safira"; //Certo
                string imagem03 = "rare_colourable_beehive_bulb_1"; //Certo
                string mobi04 = "Mamute Safira"; //Certo
                string imagem04 = "rare_colourable_elephant_statue_1"; //Certo
                string mobi05 = "Ventilador Safira"; //Certo
                string imagem05 = "rare_colourable_fan_1"; //Certo
                string mobi06 = "Fonte Safira"; //Certo
                string imagem06 = "rare_colourable_fountain_1"; //Certo
                string mobi07 = "Sorveteira Safira"; //Certo
                string imagem07 = "rare_colourable_icecream_1"; //Certo
                string mobi08 = "Toldo Safira"; //Certo
                string imagem08 = "rare_colourable_marquee_1"; //Certo
                string mobi09 = "Parasol Safira"; //Certo
                string imagem09 = "rare_colourable_parasol_1"; //Certo
                string mobi010 = "Pilar Dórico Safira"; //Certo
                string imagem010 = "rare_colourable_pillar_1"; //Certo
                string mobi011 = "Bonnie Safira"; //Certo
                string imagem011 = "rare_colourable_pillow_1"; //Certo
                string mobi012 = "Porta Espacial Safira"; //Certo
                string imagem012 = "rare_colourable_scifidoor_1"; //Certo
                string mobi013 = "Porta Laser Safira"; //Certo
                string imagem013 = "rare_colourable_scifiport_1"; //Certo
                string mobi014 = "Máquina de Fumaça Safira"; //Certo
                string imagem014 = "rare_colourable_scifirocket_1"; //Certo
                string mobi015 = "Bimbo Oriental Safira"; //Certo
                string imagem015 = "rare_colourable_wooden_screen_1"; //Certo
                int randomNumber = random.Next(1, 15);
                if (Item.UserID != Session.GetHabbo().Id)
                {
                    Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Esta arca de raros perdidos azuis não te pertence."));
                    return;
                }
                Room Room = Session.GetHabbo().CurrentRoom;
                Room.GetRoomItemHandler().RemoveFurniture(null, Item.Id);
                if (randomNumber == 1)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi1 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi1, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem01 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi01 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem01 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi01, " !"));
                    }
                    return;
                }
                if (randomNumber == 2)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi2 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi2, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem02 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi02 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem02 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi02, " !"));
                    }
                    return;
                }
                if (randomNumber == 3)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi3 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi3, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem03 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi03 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem03 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi03, " !"));
                    }
                    return;
                }
                if (randomNumber == 4)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi4 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi4, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem04 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi04 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem04 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi04, " !"));
                    }
                    return;
                }
                if (randomNumber == 5)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi5 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi5, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem05 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi05 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem05 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi05, " !"));
                    }
                    return;
                }
                if (randomNumber == 6)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi6 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi6, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem06 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi06 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem06 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi06, " !"));
                    }
                    return;
                }
                if (randomNumber == 7)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi7 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi7, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem07 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi07 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem07 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi07, " !"));
                    }
                    return;
                }
                if (randomNumber == 8)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi8 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi8, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem08 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi08 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem08 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi08, " !"));
                    }
                    return;
                }
                if (randomNumber == 9)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi9 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi9, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem09 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi09 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem09 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi09, " !"));
                    }
                    return;
                }
                if (randomNumber == 10)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi10 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi10, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem09 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi010 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem010 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi010, " !"));
                    }
                    return;
                }
                if (randomNumber == 11)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi11 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi11, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem011 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi011 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem011 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi011, " !"));
                    }
                    return;
                }
                if (randomNumber == 12)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi12 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi12, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem012 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi012 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem012 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi012, " !"));
                    }
                    return;
                }
                if (randomNumber == 13)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi13 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi13, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem013 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi013 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem013 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi013, " !"));
                    }
                    return;
                }
                if (randomNumber == 14)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi14 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi14, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem014 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi014 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem014 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi014, " !"));
                    }
                    return;
                }
                if (randomNumber == 15)
                {
                    using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE `items` SET `room_id` = '0', `base_item` = '" + mobi15 + "' WHERE `id` = '" + Item.Id + "' LIMIT 1");
                    }
                    Session.GetHabbo().GetInventoryComponent().AddNewItem(Item.Id, mobi15, Item.ExtraData, Item.GroupId, true, true, Item.LimitedNo, Item.LimitedTot);
                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    Session.SendMessage(new RoomNotificationComposer("icons/" + imagem015 + "_icon", 3, "Você acaba de ganha o raro perdido azul celestial : " + mobi015 + " #Click aqui para ver no seu inventario#", "inventory/open/furni"));
                    if (Session.GetHabbo().Rank == 1)
                    {
                        BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("icons/" + imagem015 + "_icon", 3, "O usuário " + Session.GetHabbo().Username + " ganhou na arca de raros perdidos o raro: " + mobi015, " !"));
                    }
                    return;
                }
            }
            ///Da Arca Azul by: Thiago Araujo
        }

        public void OnWiredTrigger(Item Item)
        {
            Item.ExtraData = "-1";
            Item.UpdateState(false, true);
            Item.RequestUpdate(4, true);
        }

    }
}
