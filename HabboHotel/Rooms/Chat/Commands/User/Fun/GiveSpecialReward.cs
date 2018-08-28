using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using System.Data;
using System;
using Bios.Communication.Packets.Outgoing.Rooms.Nux;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class GiveSpecialReward : IChatCommand
    {
        public string PermissionRequired => "";
        public string Parameters => "";
        public string Description => "Abrir menu de compras [Custa 30 diamantes]";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            int AmountThiago;
            if (int.TryParse("30", out AmountThiago))
            {
                if (Session.GetHabbo().Diamonds < AmountThiago)
                {
                    Session.SendMessage(RoomNotificationComposer.SendBubble("erro", "Ops você não tem " + AmountThiago.ToString() + " diamante(s)!"));
                }
            }

            int Amount;
            if (int.TryParse("30", out Amount))
            {
                if (Session.GetHabbo().Diamonds > AmountThiago)
                {
                    Session.GetHabbo().Diamonds -= Amount;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, 0, 5));
                    Session.SendMessage(RoomNotificationComposer.SendBubble("diamonds", "Você acabou de gasta " + Amount + " diamantes.", ""));
                    Session.SendMessage(new NuxItemListComposer());
                    Session.SendMessage(RoomNotificationComposer.SendBubble("catalogue", "Você abriu o menu de compras com exito!"));
                }
            }
        }
    }
}