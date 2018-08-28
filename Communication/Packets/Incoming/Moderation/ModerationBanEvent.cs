using Bios.Database.Interfaces;
using Bios.HabboHotel.Users;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Moderation;

namespace Bios.Communication.Packets.Incoming.Moderation
{
    class ModerationBanEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            if (Session == null || Session.GetHabbo() == null || !Session.GetHabbo().GetPermissions().HasRight("mod_soft_ban"))
                return;

            int UserId = Packet.PopInt();
            string Message = Packet.PopString();
            double Length = (Packet.PopInt() * 3600) + BiosEmuThiago.GetUnixTimestamp();
            string Unknown1 = Packet.PopString();
            string Unknown2 = Packet.PopString();
            bool IPBan = Packet.PopBoolean();
            bool MachineBan = Packet.PopBoolean();

            if (MachineBan)
                IPBan = false;

            Habbo Habbo = BiosEmuThiago.GetHabboById(UserId);

            if (Habbo == null)
            {
                Session.SendWhisper("Ocorreu um erro em encontrar esse usuário no banco de dados.");
                return;
            }

            if (Habbo.GetPermissions().HasRight("mod_tool") && !Session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                Session.SendWhisper("Ops, você não pode proibir o usuário.");
                return;
            }

            Message = (Message ?? "nenhuma razão foi especificado.");

            string Username = Habbo.Username;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.runFastQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + Habbo.Id + "' LIMIT 1");
            }

            if (IPBan == false && MachineBan == false)
                BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Message, Length);
            else if (IPBan == true)
                BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.IP, Habbo.Username, Message, Length);
            else if (MachineBan == true)
            {
                BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.IP, Habbo.Username, Message, Length);
                BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.USERNAME, Habbo.Username, Message, Length);
                BiosEmuThiago.GetGame().GetModerationManager().BanUser(Session.GetHabbo().Username, ModerationBanType.MACHINE, Habbo.Username, Message, Length);
            }

            GameClient TargetClient = BiosEmuThiago.GetGame().GetClientManager().GetClientByUsername(Habbo.Username);
            if (TargetClient != null)
                TargetClient.Disconnect();
        }
    }
}