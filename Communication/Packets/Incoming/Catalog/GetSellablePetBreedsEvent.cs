using Bios.Communication.Packets.Outgoing.Catalog;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Items;

namespace Bios.Communication.Packets.Incoming.Catalog
{
    public class GetSellablePetBreedsEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            string Type = Packet.PopString();

            ItemData Item = BiosEmuThiago.GetGame().GetItemManager().GetItemByName(Type);
            if (Item == null)
                return;

            int PetId = Item.BehaviourData;

            Session.SendMessage(new SellablePetBreedsComposer(Type, PetId, BiosEmuThiago.GetGame().GetCatalog().GetPetRaceManager().GetRacesForRaceId(PetId)));
        }
    }
}