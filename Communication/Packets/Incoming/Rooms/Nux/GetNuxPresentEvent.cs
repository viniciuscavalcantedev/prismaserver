using System;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Utilities;
using Bios.HabboHotel.Items;
using Bios.Communication.Packets.Outgoing.Inventory.Furni;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.Communication.Packets.Incoming.Rooms.Nux
{
    class GetNuxPresentEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int Data1 = Packet.PopInt(); // ELEMENTO 1
            int Data2 = Packet.PopInt(); // ELEMENTO 2
            int Data3 = Packet.PopInt(); // ELEMENTO 3
            int Data4 = Packet.PopInt(); // SELECTOR
            var RewardName = "";

            switch (Data4)
            {
                case 0:
                    int RewardDiamonds = RandomNumber.GenerateRandom(0, 5);
                    Session.GetHabbo().Diamonds += RewardDiamonds;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                    Session.SendMessage(RoomNotificationComposer.SendBubble("diamonds", "Você acabou de ganha " + RewardDiamonds + " diamantes.", ""));
                    break;
                case 1:
                    int RewardGotw = RandomNumber.GenerateRandom(25, 50);
                    Session.GetHabbo().GOTWPoints += RewardGotw;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().GOTWPoints, RewardGotw, 103));
                    break;
                case 2:
                    int RewardItem = RandomNumber.GenerateRandom(1, 10);
                    var RewardItemId = 0;

                    switch (RewardItem)
                    {
                        case 1:
                            RewardItemId = 49231247; // Pacos de Notas
                            RewardName = "Pacos de Notas";
                            break;
                        case 2:
                            RewardItemId = 2607; // CD Antigo
                            RewardName = "CD Antigo";
                            break;
                        case 3:
                            RewardItemId = 179; // Pato de Borracha
                            RewardName = "Pato de Borracha";
                            break;
                        case 4:
                            RewardItemId = 3226; // Gnoma
                            RewardName = "Gnoma";
                            break;
                        case 5:
                            RewardItemId = 3155; // Cadeira Mursh
                            RewardName = "Cadeira Mursh";
                            break;
                        case 6:
                            RewardItemId = 3291; // Mão
                            RewardName = "Mão";
                            break;
                        case 7:
                            RewardItemId = 206; // Abóbora
                            RewardName = "Abóbora";
                            break;
                        case 8:
                            RewardItemId = 9159; // Teia de Aranha
                            RewardName = "Teia de Aranha";
                            break;
                        case 9:
                            RewardItemId = 2064; // Moeda
                            RewardName = "Moeda";
                            break;
                        case 10:
                            RewardItemId = 2064; // Moeda
                            RewardName = "Moeda";
                            break;
                    }
                    ItemData Item = null;
                    if (!BiosEmuThiago.GetGame().GetItemManager().GetItem(RewardItemId, out Item))
                    { return; }

                    Item GiveItem = ItemFactory.CreateSingleItemNullable(Item, Session.GetHabbo(), "", "");
                    if (GiveItem != null)
                    {
                        Session.GetHabbo().GetInventoryComponent().TryAddItem(GiveItem);

                        Session.SendMessage(new FurniListNotificationComposer(GiveItem.Id, 1));
                        Session.SendMessage(new FurniListUpdateComposer());
                        Session.SendMessage(RoomNotificationComposer.SendBubble("voucher", "Você acabou de receber um " + RewardName + ".\n\nCorre, " + Session.GetHabbo().Username + ", Verifique o seu inventário, há algo novo aparentemente!", ""));
                    }

                    Session.GetHabbo().GetInventoryComponent().UpdateItems(true);
                    break;
            }
            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_AnimationRanking", 1);
        }
    }
}