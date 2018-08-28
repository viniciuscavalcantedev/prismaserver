using Bios.Communication.Packets.Outgoing.Catalog;

namespace Bios.Communication.RCON.Commands.Hotel
{
    class ReloadCatalogCommand : IRCONCommand
    {
        public string Description => "Se utiliza para actualizar Catalogo";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            BiosEmuThiago.GetGame().GetCatalog().Init(BiosEmuThiago.GetGame().GetItemManager());
            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new CatalogUpdatedComposer());
            return true;
        }
    }
}