using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Items.Wired;
using Bios.Communication.Packets.Outgoing.Rooms.Engine;
using Bios.Communication.Packets.Outgoing.Rooms.Chat;
using Bios.Communication.Packets.Outgoing.Rooms.Poll;
using Bios.HabboHotel.Rooms.AI;
using System;
using System.Collections.Generic;
using Bios.HabboHotel.Rooms.AI.Speech;

namespace Bios.Communication.Packets.Incoming.Rooms.Engine
{
    class GetRoomEntryDataEvent : IPacketEvent
    {
        public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null)
                return;

            Room Room = Session.GetHabbo().CurrentRoom;
            if (Room == null)
                return;

            if (Session.GetHabbo().InRoom)
            {

				if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room OldRoom))
					return;

				if (OldRoom.GetRoomUserManager() != null)
                    OldRoom.GetRoomUserManager().RemoveUserFromRoom(Session, false, false);
            }

            if (!Room.GetRoomUserManager().AddAvatarToRoom(Session))
            {
                Room.GetRoomUserManager().RemoveUserFromRoom(Session, false, false);
                return;//TODO: Remove?
            }

            Room.SendObjects(Session);

            //Status updating for messenger, do later as buggy.

            try
            {
                if (Session.GetHabbo().GetMessenger() != null)
                    Session.GetHabbo().GetMessenger().OnStatusChanged(true);
            }
            catch { }

            if (Session.GetHabbo().GetStats().QuestID > 0)
                BiosEmuThiago.GetGame().GetQuestManager().QuestReminder(Session, Session.GetHabbo().GetStats().QuestID);

            Session.SendMessage(new RoomEntryInfoComposer(Room.RoomId, Room.CheckRights(Session, true)));
            Session.SendMessage(new RoomVisualizationSettingsComposer(Room.WallThickness, Room.FloorThickness, BiosEmuThiago.EnumToBool(Room.Hidewall.ToString())));

            RoomUser ThisUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Username);

            if (ThisUser != null && Session.GetHabbo().PetId == 0)
                Room.SendMessage(new UserChangeComposer(ThisUser, false));

            Session.SendMessage(new RoomEventComposer(Room.RoomData, Room.RoomData.Promotion));

            // AQUI EL IF DE SI LA SALA TIENE POLL

            if (Room.poolQuestion != string.Empty)
            {
                Session.SendMessage(new QuickPollMessageComposer(Room.poolQuestion));

                if (Room.yesPoolAnswers.Contains(Session.GetHabbo().Id) || Room.noPoolAnswers.Contains(Session.GetHabbo().Id))
                {
                    Session.SendMessage(new QuickPollResultsMessageComposer(Room.yesPoolAnswers.Count, Room.noPoolAnswers.Count));
                }
            }


            if (Room.GetWired() != null)
                Room.GetWired().TriggerEvent(WiredBoxType.TriggerRoomEnter, Session.GetHabbo());

            if (Room.ForSale && Room.SalePrice > 0 && (Room.GetRoomUserManager().GetRoomUserByHabbo(Room.OwnerName) != null))
            {
                Session.SendWhisper("Esta sala esta a venda, em " + Room.SalePrice + " Duckets. Escreva :buyroom se deseja comprar!");
            }
            else if (Room.ForSale && Room.GetRoomUserManager().GetRoomUserByHabbo(Room.OwnerName) == null)
            {
                foreach (RoomUser _User in Room.GetRoomUserManager().GetRoomUsers())
                {
                    if (_User.GetClient() != null && _User.GetClient().GetHabbo() != null && _User.GetClient().GetHabbo().Id != Session.GetHabbo().Id)
                    {
                        _User.GetClient().SendWhisper("Esta sala não se encontra a venda!");
                    }
                }
                Room.ForSale = false;
                Room.SalePrice = 0;
            }

            if (Session.GetHabbo().Effects().CurrentEffect == 77)
                Session.GetHabbo().Effects().ApplyEffect(0);

            if (BiosEmuThiago.GetUnixTimestamp() < Session.GetHabbo().FloodTime && Session.GetHabbo().FloodTime != 0)
               Session.SendMessage(new FloodControlComposer((int)Session.GetHabbo().FloodTime - (int)BiosEmuThiago.GetUnixTimestamp()));

            if (Room.OwnerId == Session.GetHabbo().Id)
            {
                string dFrank = null;
                using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    dbClient.SetQuery("SELECT bot_frank FROM users WHERE id = '" + Session.GetHabbo().Id + "' LIMIT 1");
                    dFrank = dbClient.getString();
                }
                int dFrankInt = Int32.Parse(dFrank);
                DateTime dateGregorian = new DateTime();
                dateGregorian = DateTime.Today;
                int day = 1;
                if (dFrankInt != day)
                {
                    using (var dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                    {
                        dbClient.RunQuery("UPDATE users SET bot_frank = '1' WHERE id = " + Session.GetHabbo().Id + ";");
                    }
                    List<RandomSpeech> BotSpeechList = new List<RandomSpeech>();
                    RoomUser BotUser = Room.GetRoomUserManager().DeployBot(new RoomBot(0, Session.GetHabbo().CurrentRoomId, "welcome", "freeroam", "Frank", "Gerente do Bios Emulador, criado por Thiago Araujo!", "hr-3194-38-36.hd-180-1.ch-220-1408.lg-285-73.sh-906-90.ha-3129-73.fa-1206-73.cc-3039-73", 0, 0, 0, 4, 0, 0, 0, 0, ref BotSpeechList, "", 0, 0, false, 0, false, 33), null);


                }
                else
                {
                    //O usuário já recebeu o prêmio de hoje!
                }
            }
        }
    }
}