using System;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Furni;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.HabboHotel.Items.Interactor
{
    class InteractorCrafting : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            Session.SendMessage(new MassEventComposer("inventory/open"));
            Session.SendMessage(new CraftableProductsComposer());
        }

        public void OnWiredTrigger(Item Item)
        {
        }
    }
}