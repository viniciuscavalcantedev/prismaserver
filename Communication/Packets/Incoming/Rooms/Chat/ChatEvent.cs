using System;

using Bios.Core;
using Bios.Utilities;
using Bios.HabboHotel.Quests;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Rooms.Chat.Logs;
using Bios.Communication.Packets.Outgoing.Rooms.Chat;
using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.HabboHotel.Rooms.Chat.Styles;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Bios.Communication.Packets.Incoming.Rooms.Chat
{
	public class ChatEvent : IPacketEvent
	{
		public void Parse(GameClient Session, ClientPacket Packet)
		{
			if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().InRoom)
				return;

			Room Room = Session.GetHabbo().CurrentRoom;
			if (Room == null)
				return;

			RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
			if (User == null)
				return;

			string Message = StringCharFilter.Escape(Packet.PopString());
			if (Message.Length > 100)
				Message = Message.Substring(0, 100);

			int Colour = Packet.PopInt();

			if (!BiosEmuThiago.GetGame().GetChatManager().GetChatStyles().TryGetStyle(Colour, out ChatStyle Style) || (Style.RequiredRight.Length > 0 && !Session.GetHabbo().GetPermissions().HasRight(Style.RequiredRight)))
				Colour = 0;

			User.UnIdle();

			if (BiosEmuThiago.GetUnixTimestamp() < Session.GetHabbo().FloodTime && Session.GetHabbo().FloodTime != 0)
				return;

			if (Session.GetHabbo().TimeMuted > 0)
			{
				Session.SendMessage(new MutedComposer(Session.GetHabbo().TimeMuted));
				return;
			}

			if (!Session.GetHabbo().GetPermissions().HasRight("room_ignore_mute") && Room.CheckMute(Session))
			{
				Session.SendWhisper("Ops, você está mutado!");
				return;
			}

			User.LastBubble = Session.GetHabbo().CustomBubbleId == 0 ? Colour : Session.GetHabbo().CustomBubbleId;

			if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
			{
				if (User.IncrementAndCheckFlood(out int MuteTime))
				{
					Session.SendMessage(new FloodControlComposer(MuteTime));
					return;
				}
			}

			if (Message.StartsWith(":", StringComparison.CurrentCulture) && BiosEmuThiago.GetGame().GetChatManager().GetCommands().Parse(Session, Message))
				return;

			BiosEmuThiago.GetGame().GetChatManager().GetLogs().StoreChatlog(new ChatlogEntry(Session.GetHabbo().Id, Room.Id, Message, UnixTimestamp.GetNow(), Session.GetHabbo(), Room));

            if (Session.GetHabbo().Rank < Convert.ToInt32(BiosEmuThiago.GetConfig().data["MineRankStaff"]))
            {
                if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override") &&
                BiosEmuThiago.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Message, out string word))
			{

                // Comando editaveu abaixo mais cuidado pra não faze merda
                Session.GetHabbo().BannedPhraseCount++;
                if (Session.GetHabbo().BannedPhraseCount >= 1)
                {

                    User.MoveTo(Room.GetGameMap().Model.DoorX, Room.GetGameMap().Model.DoorY);
                    Session.GetHabbo().TimeMuted = 10;
                    Session.SendNotification("Você foi silenciado, um moderador vai rever o seu caso, aparentemente, você nomeou um hotel! Não continue divulgando ser for um hotel pois temos ante divulgação - Aviso<font size =\"11\" color=\"#fc0a3a\">  <b>" + Session.GetHabbo().BannedPhraseCount + "/5</b></font> Ser chega ao numero 5/5 você sera banido automaticamente");
                    BiosEmuThiago.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("Alerta publicitário:",
                        "Atenção colaboradores do " + BiosEmuThiago.HotelName + ", o usuário <b>" + Session.GetHabbo().Username + "</b> divulgor um link de um site ou hotel na frase, você poderia investigar? so click no botão abaixo *Ir ao Quarto*. <i> a palavra dita:<font size =\"11\" color=\"#f40909\">  <b>  " + Message +
                        "</b></font></i>   dentro de um das salas do jogo\r\n" + "- Nome do usuário: <font size =\"11\" color=\"#0b82c6\">  <b>" +
                        Session.GetHabbo().Username + "</b>", "", "Ir ao Quarto", "event:navigator/goto/" +
                        Session.GetHabbo().CurrentRoomId));
                }

                if (Session.GetHabbo().BannedPhraseCount >= 5)
                {
                    BiosEmuThiago.GetGame().GetModerationManager().BanUser("BiosEmulador ante divulgação", HabboHotel.Moderation.ModerationBanType.USERNAME, Session.GetHabbo().Username, "Banido por spam com a frase (" + Message + ")", (BiosEmuThiago.GetUnixTimestamp() + 78892200));
                    Session.Disconnect();
                    return;
                }
                Session.SendMessage(new RoomNotificationComposer("furni_placement_error", "message", "Mensagem inapropiada no " + BiosEmuThiago.HotelName + ". Estamos investigando o que você falou" + " " + Session.GetHabbo().Username + " " + "na sala!"));
                return;
            }
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("room_ignore_mute") && Room.CheckMute(Session))
            {
                Session.SendWhisper("Ops, você está no momento silenciado.");
                return;
            }

            BiosEmuThiago.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_CHAT);

			User.OnChat(User.LastBubble, Message, false);
		}
	}
}