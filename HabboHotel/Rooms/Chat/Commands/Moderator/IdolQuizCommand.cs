using Bios.Communication.Packets.Outgoing.Rooms.Poll;
using Bios.Core;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class IdolQuizCommand : IChatCommand
    {

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (ExtraSettings.STAFF_EFFECT_ENABLED_ROOM)
            {
                if (Session.GetHabbo().isLoggedIn && Session.GetHabbo().Rank > Convert.ToInt32(BiosEmuThiago.GetConfig().data["MineRankStaff"]))
                {
                }
                else
                {
                    Session.SendWhisper("Você precisa estar logado como staff para usar este comando.");
                    return;
                }
            }
            if (Params.Length == 0)
            {
                Session.SendWhisper("Digite a pergunta.");
            }
            else
            {

                string question = "< Voto negativo [ " + Params[1] + " ] Voto positivo >";
                if (Session.GetHabbo().Rank > 0)
                {
                    Item[] ReloadItems = Room.GetRoomItemHandler().GetFloor.ToArray();
                    foreach (Item Chair in ReloadItems.ToList())
                    {
                        if (Chair.GetBaseItem().InteractionType == InteractionType.idol_chair)
                        {
                            Chair.ExtraData = "0";
                            Chair.UpdateState();
                        }
                        if (Chair.GetBaseItem().InteractionType == InteractionType.idol_counter)
                        {
                            Chair.ExtraData = "0";
                            Chair.UpdateState();
                        }

                        Room.EndQuestion();
                    }
                }
                else
                {

                    Item[] Items = Room.GetRoomItemHandler().GetFloor.ToArray();
                    foreach (Item Chair in Items.ToList())
                    {
                        if (Chair.GetBaseItem().InteractionType == InteractionType.idol_chair)
                        {

                            bool HasUsers = false;

                            if (Room.GetGameMap().SquareHasUsers(Chair.GetX, Chair.GetY))
                                HasUsers = true;

                            if (!HasUsers)
                            {
                                Session.SendWhisper("Não há juiz na presidência do tribunal.");
                                return;
                            }
                            BiosEmuThiago.GetGame().GetClientManager().QuizzAlert(new QuickPollMessageComposer(question), Chair, Room);
                            Room.SetPoolQuestion(question);
                            Room.ClearPoolAnswers();
                            Console.WriteLine(question);
                        }
                    }
                }
            }
        }



        public string Description =>
			"Uma pesquisa rápida.";

        public string Parameters =>
            "[USUARIO]";

        public string PermissionRequired =>
            "command_give_badge";
    }
}