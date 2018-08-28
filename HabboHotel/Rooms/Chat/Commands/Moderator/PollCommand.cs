using Bios.Core;
using Bios.HabboHotel.GameClients;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    internal class PollCommand : IChatCommand
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
                Session.SendWhisper("Por favor, apresente a pergunta");
            }
            else
            {

                string quest = CommandManager.MergeParams(Params, 1);
                if (quest == "end")
                {
                    Room.EndQuestion();
                }
                else
                {
                    Room.StartQuestion(quest);
                }

            }
        }

        public string Description =>
            "Faça uma busca imediata.";

        public string Parameters =>
            "%question%";

        public string PermissionRequired =>
            "command_give_badge";
    }
}