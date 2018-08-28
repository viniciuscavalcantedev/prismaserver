using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Events
{
    class CatalogUpdateAlert : IChatCommand
    {
        public string PermissionRequired => "command_addpredesigned";
        public string Parameters => "[MENSAJE]";
        public string Description => "Avisar de uma atualização no catálogo do hotel.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            string Message = CommandManager.MergeParams(Params, 1);

            BiosEmuThiago.GetGame().GetClientManager().SendMessage(new RoomNotificationComposer("Atualizamos o Catalago!",
              "O catálogo do <font color=\"#2E9AFE\"><b>" + BiosEmuThiago.HotelName + "</b></font> acaba de ser atualizado! Se quiser observar <b>as novidades</b> Só clicar no botão abaixo.<br>", "cata", "Confira a página", "event:catalog/open/" + Message));

            Session.SendWhisper("Catalogo atualizado com sucesso.");
        }
    }
}

