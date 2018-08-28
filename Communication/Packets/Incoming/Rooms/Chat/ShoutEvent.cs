using System;
using Bios.Communication.Packets.Outgoing.Rooms.Chat;
using Bios.Core;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Quests;
using Bios.HabboHotel.Rooms;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Utilities;
using Bios.Communication.Packets.Outgoing.Moderation;
using Bios.HabboHotel.Rooms.Chat.Styles;
using Bios.HabboHotel.Rooms.Chat.Logs;

namespace Bios.Communication.Packets.Incoming.Rooms.Chat
{
    public class ShoutEvent : IPacketEvent
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

            ChatStyle Style = null;
            if (!BiosEmuThiago.GetGame().GetChatManager().GetChatStyles().TryGetStyle(Colour, out Style) || (Style.RequiredRight.Length > 0 && !Session.GetHabbo().GetPermissions().HasRight(Style.RequiredRight)))
                Colour = 0;

            User.LastBubble = Session.GetHabbo().CustomBubbleId == 0 ? Colour : Session.GetHabbo().CustomBubbleId;

            if (BiosEmuThiago.GetUnixTimestamp() < Session.GetHabbo().FloodTime && Session.GetHabbo().FloodTime != 0)
                return;

            if (Session.GetHabbo().TimeMuted > 0)
            {
                Session.SendMessage(new MutedComposer(Session.GetHabbo().TimeMuted));
                return;
            }

            if (!Room.CheckRights(Session, false) && Room.muteSignalEnabled == true)
            {
                Session.SendWhisper("O quarto esta silenciado, você não pode falar sobre isso até que o proprietário ou alguém com permissões permita.");
                return;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("room_ignore_mute") && Room.CheckMute(Session))
            {
                Session.SendWhisper("Bem, agora você está silenciado.");
                return;
            }

            if (!Session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            {
                int MuteTime;
                if (User.IncrementAndCheckFlood(out MuteTime))
                {
                    Session.SendMessage(new FloodControlComposer(MuteTime));
                    return;
                }
            }

            BiosEmuThiago.GetGame().GetChatManager().GetLogs().StoreChatlog(new ChatlogEntry(Session.GetHabbo().Id, Room.Id, Message, UnixTimestamp.GetNow(), Session.GetHabbo(), Room));

            if (Message.StartsWith(":", StringComparison.CurrentCulture) && BiosEmuThiago.GetGame().GetChatManager().GetCommands().Parse(Session, Message))
                return;

            BiosEmuThiago.GetGame().GetChatManager().GetLogs().StoreChatlog(new ChatlogEntry(Session.GetHabbo().Id, Room.Id, Message, UnixTimestamp.GetNow(), Session.GetHabbo(), Room));

            string word;
            if (!Session.GetHabbo().GetPermissions().HasRight("word_filter_override") &&
                BiosEmuThiago.GetGame().GetChatManager().GetFilter().IsUnnaceptableWord(Message, out word))
            {
                Session.GetHabbo().BannedPhraseCount++;
                if (Session.GetHabbo().BannedPhraseCount >= 1)
                {

					User.MoveTo(Room.GetGameMap().Model.DoorX, Room.GetGameMap().Model.DoorY);
					Session.GetHabbo().TimeMuted = 25;
					Session.SendNotification("Você foi silenciad@ um moderador vai ver seu caso, aparentemente, você nomeou um hotel! <b>Aviso: " + Session.GetHabbo().BannedPhraseCount + "/5</b>");
					BiosEmuThiago.GetGame().GetClientManager().StaffAlert(new RoomNotificationComposer("Alerta de divulgador:",
						"Atenção mencionou a palavra <b>" + word.ToUpper() + "</b> na frase <i>" + Message +
						"</i> dentro de uma sala\r\n" + "- Este usuario: <b>" +
						Session.GetHabbo().Username + "</b>", NotificationSettings.NOTIFICATION_FILTER_IMG, "Ir a Sala", "event:navigator/goto/" +
						Session.GetHabbo().CurrentRoomId));
				}
                if (Session.GetHabbo().BannedPhraseCount >= 5)
                {
                    BiosEmuThiago.GetGame().GetModerationManager().BanUser("System", HabboHotel.Moderation.ModerationBanType.USERNAME, Session.GetHabbo().Username, "Banido por spam (" + Message + ")", (BiosEmuThiago.GetUnixTimestamp() + 78892200));
                    Session.Disconnect();
                    return;
                }
                Session.SendMessage(new ShoutComposer(User.VirtualId, "Palavra Inapropriada", 0, Colour));
                return;
            }
            BiosEmuThiago.GetGame().GetQuestManager().ProgressUserQuest(Session, QuestType.SOCIAL_CHAT);

            User.UnIdle();
            User.OnChat(User.LastBubble, Message, true);
        }
    }
}