using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;
using System.Collections.Generic;
using System.Linq;
using Bios.Communication.Packets.Outgoing.Rooms.Session;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class BuyRoomCommand : IChatCommand
    {
        public string Description => "Compre um quarto a venda do usuário.";
        public string Parameters => "";
        public string PermissionRequired => "";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            RoomUser Owner = Room.GetRoomUserManager().GetRoomUserByHabbo(Room.RoomData.OwnerId);

            if (User == null)
                return;

            if (!Room.RoomForSale)
            {
                Session.SendWhisper("Este quarto não está à venda, contacte o proprietario se estiver interessado:" + Room.OwnerName);
                return;
            }

            if (Room.OwnerId == User.HabboId)
            {
                Session.SendWhisper("Você não pode comprar seu próprio quarto.");
                return;
            }

            if (User.GetClient().GetHabbo().Duckets >= Room.ForSaleAmount)
            {
                Room.AssignNewOwner(Room, User, Owner);
            }
            else
            {
                User.GetClient().SendWhisper("Você não tem duckets suficientes!");
                return;
            }

        }
    }
}
