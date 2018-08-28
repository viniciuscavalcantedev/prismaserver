using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Bios.Communication.Packets.Outgoing.Users;
using Bios.Communication.Packets.Outgoing.Notifications;
using Bios.Communication.Packets.Outgoing.Handshake;
using Bios.Communication.Packets.Outgoing.Quests;
using Bios.HabboHotel.Items;
using Bios.Communication.Packets.Outgoing.Inventory.Furni;
using Bios.Communication.Packets.Outgoing.Catalog;
using Bios.HabboHotel.Quests;
using Bios.HabboHotel.Rooms;
using System.Threading;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Avatar;
using Bios.Communication.Packets.Outgoing.Pets;
using Bios.Communication.Packets.Outgoing.Messenger;
using Bios.HabboHotel.Users.Messenger;
using Bios.Communication.Packets.Outgoing.Rooms.Polls;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Communication.Packets.Outgoing.Availability;
using Bios.Communication.Packets.Outgoing;


namespace Bios.HabboHotel.Rooms.Chat.Commands.Events
{
    class HelpCommand : IChatCommand
    {
        public string PermissionRequired => "command_info";
        public string Parameters => "[MENSAJE]";
        public string Description => "Enviar um pedido de ajuda, descrevendo brevemente o seu problema.";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            long nowTime = BiosEmuThiago.CurrentTimeMillis();
            long timeBetween = nowTime - Session.GetHabbo()._lastTimeUsedHelpCommand;
            if (timeBetween < 60000)
            {
                Session.SendMessage(RoomNotificationComposer.SendBubble("abuse", "Espere pelo menos 1 minuto para reutilizar o sistema de apoio", ""));
                return;
            }

            Session.GetHabbo()._lastTimeUsedHelpCommand = nowTime;
            string Request = CommandManager.MergeParams(Params, 1);

            if (Params.Length == 1)
            {
                Session.SendMessage(new RoomNotificationComposer("Sistema de suporte:", "<font color='#B40404'><b>Atenção, " + Session.GetHabbo().Username + "!</b></font>\n\n<font size=\"11\" color=\"#1C1C1C\">O sistema de suporte foi criado para fazer solicitações detalhadas de ajuda. Então você não pode enviar uma mensagem vazia porque é inútil.\n\n" +
                 "Se você quiser pedir ajuda, descrever <font color='#B40404'> <b> detalhadamente o seu problema</b></font>. \n\nO sistema detecta se você abusar estes pedidos, então não enviar mais do que um ou você será bloqueado.\n\n" +
                 "Lembre-se que você também tem ajuda central para resolver seus problemas.", "help_user", ""));
                return;
            }
            else

                BiosEmuThiago.GetGame().GetClientManager().GuideAlert(new RoomNotificationComposer("Novo caso de atenção!",
                 "O usuario " + Session.GetHabbo().Username + " Ele requer a ajuda de um guia, o embaixador ou moderador.<br></font></b><br>Sua pergunta ou problema é este:<br><b>s"
                 + Request + "</b></font><br><br>Atender ao usuário mais rapidamente possível para resolver a sua pergunta, lembre-se que em breve sua ajuda vai ser marcado e que serão considerados para a promoção.", "Ajude-me", "Seguir a " + Session.GetHabbo().Username + "", "event:navigator/goto/" + Session.GetHabbo().CurrentRoomId));

            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_GuideEnrollmentLifetime", 1);
            Session.SendMessage(RoomNotificationComposer.SendBubble("ambassador", "Seu pedido de ajuda foi enviada com sucesso, aguarde.", ""));
        }
    }
}



