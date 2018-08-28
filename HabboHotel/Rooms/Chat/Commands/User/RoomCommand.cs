using System.Text;
using Bios.Communication.Packets.Outgoing.Rooms.Engine;
using Bios.Database.Interfaces;

namespace Bios.HabboHotel.Rooms.Chat.Commands.User
{
    class RoomCommand : IChatCommand
    {
        public string PermissionRequired => "command_room";
        public string Parameters => "list/golpe/push/pull/enables/respect/spush/spull/pets";
        public string Description => "Capacidade de desativar comandos básicos da sala.";

        public void Execute(GameClients.GameClient Session, Rooms.Room Room, string[] Params)
        {


            if (Params.Length == 1)
            {
                Session.SendWhisper("Vá, você deve escolher uma opção para desativar quarto.");
                return;
            }

            if (!Room.CheckRights(Session, true))
            {
                Session.SendWhisper("Bem, somente o proprietário da sala ou a equipe pode usar este comando.");
                return;
            }

            string Option = Params[1];
            switch (Option)
            {
                case "list":
                    {
                        StringBuilder List = new StringBuilder("");
                        List.AppendLine("Lista de comando na sala");
                        List.AppendLine("-------------------------");
                        List.AppendLine("Pet Morphs: " + (Room.PetMorphsAllowed == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Pull: " + (Room.PullEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Push: " + (Room.PushEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Golpes: " + (Room.GolpeEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Super Pull: " + (Room.SPullEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Super Push: " + (Room.SPushEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Respect: " + (Room.RespectNotificationsEnabled == true ? "Habilitado" : "Deshabilitado"));
                        List.AppendLine("Enables: " + (Room.EnablesEnabled == true ? "Habilitado" : "Deshabilitado"));
                        Session.SendNotification(List.ToString());
                        break;
                    }

                case "golpe":
                    {
                        Room.GolpeEnabled = !Room.GolpeEnabled;
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `golpe_enabled` = @GolpeEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("GolpeEnabled", BiosEmuThiago.BoolToEnum(Room.GolpeEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Golpes nesta sala são" + (Room.GolpeEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "push":
                    {
                        Room.PushEnabled = !Room.PushEnabled;
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `push_enabled` = @PushEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PushEnabled", BiosEmuThiago.BoolToEnum(Room.PushEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo Push agora esta " + (Room.PushEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "spush":
                    {
                        Room.SPushEnabled = !Room.SPushEnabled;
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `spush_enabled` = @PushEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PushEnabled", BiosEmuThiago.BoolToEnum(Room.SPushEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo Super Push agora esta " + (Room.SPushEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "spull":
                    {
                        Room.SPullEnabled = !Room.SPullEnabled;
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `spull_enabled` = @PullEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PullEnabled", BiosEmuThiago.BoolToEnum(Room.SPullEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo Super Pull agora esta  " + (Room.SPullEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "pull":
                    {
                        Room.PullEnabled = !Room.PullEnabled;
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `pull_enabled` = @PullEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PullEnabled", BiosEmuThiago.BoolToEnum(Room.PullEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Modo Pull agora esta " + (Room.PullEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "enable":
                case "enables":
                    {
                        Room.EnablesEnabled = !Room.EnablesEnabled;
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `enables_enabled` = @EnablesEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("EnablesEnabled", BiosEmuThiago.BoolToEnum(Room.EnablesEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("os efeitos da sala estão " + (Room.EnablesEnabled == true ? "Habilitados!" : "Deshabilitados!"));
                        break;
                    }

                case "respect":
                    {
                        Room.RespectNotificationsEnabled = !Room.RespectNotificationsEnabled;
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `respect_notifications_enabled` = @RespectNotificationsEnabled WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("RespectNotificationsEnabled", BiosEmuThiago.BoolToEnum(Room.RespectNotificationsEnabled));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("Aviso respeito esta " + (Room.RespectNotificationsEnabled == true ? "Habilitado!" : "Deshabilitado!"));
                        break;
                    }

                case "pets":
                case "morphs":
                    {
                        Room.PetMorphsAllowed = !Room.PetMorphsAllowed;
                        using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                        {
                            dbClient.SetQuery("UPDATE `rooms` SET `pet_morphs_allowed` = @PetMorphsAllowed WHERE `id` = '" + Room.Id + "' LIMIT 1");
                            dbClient.AddParameter("PetMorphsAllowed", BiosEmuThiago.BoolToEnum(Room.PetMorphsAllowed));
                            dbClient.RunQuery();
                        }

                        Session.SendWhisper("pets nesta sala esta " + (Room.PetMorphsAllowed == true ? "Habilitado!" : "Deshabilitado!"));

                        if (!Room.PetMorphsAllowed)
                        {
                            foreach (RoomUser User in Room.GetRoomUserManager().GetRoomUsers())
                            {
                                if (User == null || User.GetClient() == null || User.GetClient().GetHabbo() == null)
                                    continue;

                                User.GetClient().SendWhisper("O quarto proprietário desativou a opção de se tornar um animal de estimação.");
                                if (User.GetClient().GetHabbo().PetId > 0)
                                {
                                    //Tell the user what is going on.
                                    User.GetClient().SendWhisper("Oops, o proprietário da sala só permite que os usuários comuns, sem animais..");

                                    //Change the users Pet Id.
                                    User.GetClient().GetHabbo().PetId = 0;

                                    //Quickly remove the old user instance.
                                    Room.SendMessage(new UserRemoveComposer(User.VirtualId));

                                    //Add the new one, they won't even notice a thing!!11 8-)
                                    Room.SendMessage(new UsersComposer(User));
                                }
                            }
                        }
                        break;
                    }
            }
        }
    }
}
