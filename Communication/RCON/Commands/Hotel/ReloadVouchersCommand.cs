namespace Bios.Communication.RCON.Commands.Hotel
{
    class ReloadVouchersCommand : IRCONCommand
    {
        public string Description => "Se utiliza para recarregar os Vouchers";
        public string Parameters => "";

        public bool TryExecute(string[] parameters)
        {
            BiosEmuThiago.GetGame().GetCatalog().GetVoucherManager().Init();
            return true;
        }
    }
}