using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Rooms;

namespace Bios.HabboHotel.Items.Interactor
{
    public class InteractorInformationTerminal : IFurniInteractor
    {
        public void OnPlace(GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClient Session, Item Item)
        {

        }
        public void BeforeRoomLoad(Item Item)
        { }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
            if (Item == null || Item.GetRoom() == null || Session == null || Session.GetHabbo() == null)
                return;

            RoomUser User = Item.GetRoom().GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
            if (User == null)
                return;

            User.LastInteraction = BiosEmuThiago.GetUnixTimestamp();
            Session.SendWhisper("Bios Emulador By: Thiago Araujo");
            Session.SendWhisper("Teste:" + Item.ExtraData);
        }

        public void OnWiredTrigger(Item Item)
        {
        }
    }
}