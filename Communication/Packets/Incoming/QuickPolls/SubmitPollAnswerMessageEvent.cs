﻿using Bios.HabboHotel.GameClients;
using System;
using Bios.HabboHotel.Rooms;
using Bios.Communication.Packets.Outgoing.Rooms.Poll;

namespace Bios.Communication.Packets.Incoming.QuickPolls
{
    class SubmitPollAnswerMessageEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {

            int pollId = Packet.PopInt();
            int questionId = Packet.PopInt();
            int count = Packet.PopInt();

            String answer = Packet.PopString();
            //DEBUG MADAFAKA
            if (questionId == -1)
            {
                if (Session == null || Session.GetHabbo() == null)
                    return;

                Room room = Session.GetHabbo().CurrentRoom;
                if (room == null)
                    return;

                if (room.poolQuestion == string.Empty)
                {
                    return;
                }

                if (room.yesPoolAnswers.Contains(Session.GetHabbo().Id) || room.noPoolAnswers.Contains(Session.GetHabbo().Id))
                {
                    return;
                }

                if (answer.Equals("1"))
                {
                    room.yesPoolAnswers.Add(Session.GetHabbo().Id);
                }
                else
                {
                    room.noPoolAnswers.Add(Session.GetHabbo().Id);
                }

                room.SendMessage(new QuickPollResultMessageComposer(Session.GetHabbo().Id, answer, room.yesPoolAnswers.Count, room.noPoolAnswers.Count));
                return;
            }
        }
    }
}

       
