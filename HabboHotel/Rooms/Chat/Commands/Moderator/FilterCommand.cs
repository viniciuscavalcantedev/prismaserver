using Bios.Core;
using Bios.Database.Interfaces;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class FilterCommand : IChatCommand
    {

        public string PermissionRequired => "command_filter";
        public string Parameters => "[PALAVRA]";
        public string Description => "Adicione palavras ao filtro.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
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

            if (Params.Length == 1)
            {
                Session.SendWhisper("Digite uma palavra.");
                return;
            }

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("INSERT INTO `wordfilter` (id, word, replacement, strict, addedby, bannable) VALUES (NULL, '" + Params[1] + "', '" + BiosEmuThiago.HotelName + "', '1', '" + Session.GetHabbo().Username + "', '0')");
            }

            BiosEmuThiago.GetGame().GetChatManager().GetFilter().InitWords();
            BiosEmuThiago.GetGame().GetChatManager().GetFilter().InitCharacters();
            Session.SendWhisper("Sucesso, continue lutando contra spammers");
        }
    }
}