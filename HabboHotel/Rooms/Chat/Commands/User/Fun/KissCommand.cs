using System;
using Bios.HabboHotel.GameClients;
using Bios.Communication.Packets.Outgoing.Rooms.Chat;


namespace Bios.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    class KissCommand : IChatCommand
    {
        public string PermissionRequired => "command_kiss";
        public string Parameters => "[USUARIO]";
        public string Description => "Beijar um usuário <3";

        public void Execute(GameClient Session, Room Room, string[] Params)
        {
            if (Params.Length == 1)
            {
                Session.SendWhisper("Por favor, insira o usuário!");
                return;
            }
            GameClient Target = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Params[1]);
            if (Target == null)
            {
                Session.SendWhisper("Sentimos muito, não encontramos este usuário!");
                return;
            }
            else
            {
                RoomUser TargetID = Room.GetRoomUserManager().GetRoomUserByHabbo(Target.GetHabbo().Id);
                RoomUser User = Room.GetRoomUserManager().GetRoomUserByHabbo(Session.GetHabbo().Id);
                if (TargetID == null)
                {
                    Session.LogsNotif("Sentimos muito o usuário não esta na sala!", "command_notification");
                    return;
                }
                else if (Target.GetHabbo().Username == Session.GetHabbo().Username)
                {
                    Session.SendWhisper("Está carente? Não pode se beijar.");
                    Room.SendMessage(new ChatComposer(User.VirtualId, "Alguém ajuda esse virgem não tem vida social!", 0, 34));
                    return;
                }
                else if (TargetID.TeleportEnabled)
                {
                    Session.LogsNotif("Sentimos muito o usuário está com o builder ativado!", "command_notification");
                    return;
                }
                else
                {
                    if (User != null)
                    {
                        if ((Math.Abs((int)(TargetID.X - User.X)) < 2) && (Math.Abs((int)(TargetID.Y - User.Y)) < 2))
                        {
                            Room.SendMessage(new ChatComposer(User.VirtualId, "*Te beijei de lingua " + Params[1] + " dlç*", 0, 16));
                            Room.SendMessage(new ChatComposer(TargetID.VirtualId, "*Vem eu quero mais, vem aqui dlç, vamos fazer bobba? ><*", 0, 16));
                            TargetID.ApplyEffect(9);
                        }
                        else
                        {
                            Session.SendWhisper("Ops! O usuário não está perto de você!");
                            return;
                        }
                    }
                }
            }
        }

    }
}