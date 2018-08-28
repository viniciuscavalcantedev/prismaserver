using Bios.Core;
using Bios.Database.Interfaces;
using System;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class DisableForcedFXCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_forced_effects"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Capacidade de ignorar ou permitir efeitos forçados."; }
        }

        public void Execute(GameClients.GameClient Session, Room Room, string[] Params)
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
            Session.GetHabbo().DisableForcedEffects = !Session.GetHabbo().DisableForcedEffects;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `users` SET `disable_forced_effects` = @DisableForcedEffects WHERE `id` = '" + Session.GetHabbo().Id + "' LIMIT 1");
                dbClient.AddParameter("DisableForcedEffects", (Session.GetHabbo().DisableForcedEffects == true ? 1 : 0).ToString());
                dbClient.RunQuery();
            }

            Session.SendWhisper("O modo forçado FX é " + (Session.GetHabbo().DisableForcedEffects == true ? "desabilitado!" : "ativado!"));
        }
    }
}
