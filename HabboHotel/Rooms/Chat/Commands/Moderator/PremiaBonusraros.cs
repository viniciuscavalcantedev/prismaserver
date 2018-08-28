using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Communication.Packets.Outgoing.LandingView;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.Core;
using Bios.HabboHotel.GameClients;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bios.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class PremiaBonusraros : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_alert_user"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Premiar um usuário com bônus."; }
        }

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
                Session.SendWhisper("Por favor, digite o usuário que deseja premiar!");
                return;
            }

            GameClient Target = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Opa, não foi possível encontrar esse usuário!");
                return;
            }

            RoomUser TargetUser = Room.GetRoomUserManager().GetRoomUserByHabbo(Target.GetHabbo().Id);
            if (TargetUser == null)
            {
                Session.SendWhisper("Usuário não encontrado! Talvez ele não esteja online ou nesta sala.");
                return;
            }

            if (Target.GetHabbo().Username == Session.GetHabbo().Username)
            {
                string product = BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("bonus_rare_productdata_name");
                int baseid = int.Parse(BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("bonus_rare_item_baseid"));
                int score = int.Parse(BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("bonus_rare_total_score"));

                Session.GetHabbo().BonusPoints += 1;
                Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().BonusPoints, score, 101));
                Session.SendMessage(new RoomAlertComposer("Parabéns! Você recebeu um ponto bônus! Você tem agora: (" + Session.GetHabbo().BonusPoints + ") bônus"));
                Session.SendMessage(new BonusRareMessageComposer(Session));
                return;
            }
            if (Target.GetHabbo().Username != Session.GetHabbo().Username)
            {
                string product = BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("bonus_rare_productdata_name");
                int baseid = int.Parse(BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("bonus_rare_item_baseid"));
                int score = int.Parse(BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("bonus_rare_total_score"));

                Target.GetHabbo().BonusPoints += 1;
                Target.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().BonusPoints, score, 101));
                Target.SendMessage(new RoomAlertComposer("Parabéns! Você recebeu um ponto bônus! Você tem agora: (" + Target.GetHabbo().BonusPoints + ") bônus"));
                Target.SendMessage(new BonusRareMessageComposer(Target));
                Session.SendMessage(new RoomAlertComposer("Parabéns! Você deu com exito os pontos bônus!"));
            }
        }
    }
}