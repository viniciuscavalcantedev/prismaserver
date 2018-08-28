using System;
using System.Data;

namespace Bios.HabboHotel.Users.Authenticator
{
    public static class HabboFactory
    {
        public static Habbo GenerateHabbo(DataRow Row, DataRow UserInfo)
        {
            return new Habbo(Convert.ToInt32(Row["id"]), Convert.ToString(Row["username"]), Convert.ToInt32(Row["rank"]), Convert.ToString(Row["motto"]), Convert.ToString(Row["look"]),
                Convert.ToString(Row["gender"]), Convert.ToInt32(Row["credits"]), Convert.ToInt32(Row["activity_points"]), 
                Convert.ToInt32(Row["home_room"]), BiosEmuThiago.EnumToBool(Row["block_newfriends"].ToString()), Convert.ToInt32(Row["last_online"]),
                BiosEmuThiago.EnumToBool(Row["hide_online"].ToString()), BiosEmuThiago.EnumToBool(Row["hide_inroom"].ToString()),
                Convert.ToDouble(Row["account_created"]), Convert.ToInt32(Row["vip_points"]),Convert.ToString(Row["machine_id"]), (Row["nux_user"].ToString() == "true"), BiosEmuThiago.EnumToBool(Row["is_nuevo"].ToString()), Convert.ToString(Row["volume"]),
                BiosEmuThiago.EnumToBool(Row["chat_preference"].ToString()), BiosEmuThiago.EnumToBool(Row["focus_preference"].ToString()), BiosEmuThiago.EnumToBool(Row["pets_muted"].ToString()), BiosEmuThiago.EnumToBool(Row["bots_muted"].ToString()),
                BiosEmuThiago.EnumToBool(Row["advertising_report_blocked"].ToString()), Convert.ToDouble(Row["last_change"].ToString()), Convert.ToInt32(Row["gotw_points"]), Convert.ToInt32(Row["user_points"]),
                BiosEmuThiago.EnumToBool(Convert.ToString(Row["ignore_invites"])), Convert.ToDouble(Row["time_muted"]), Convert.ToDouble(UserInfo["trading_locked"]),
                BiosEmuThiago.EnumToBool(Row["allow_gifts"].ToString()), Convert.ToInt32(Row["friend_bar_state"]),  BiosEmuThiago.EnumToBool(Row["disable_forced_effects"].ToString()),
                BiosEmuThiago.EnumToBool(Row["allow_mimic"].ToString()), BiosEmuThiago.EnumToBool(Row["allow_events"].ToString()), Convert.ToInt32(Row["rank_vip"]), Convert.ToString(Row["name_color"]), Convert.ToString(Row["bubble_color"]), Convert.ToString(Row["prefix_name"]), Convert.ToString(Row["prefix_name_color"]), Convert.ToInt32(Row["bubble_id"]), Convert.ToByte(Row["publi"]), Convert.ToByte(Row["guia"].ToString()), Convert.ToByte(Row["builder"].ToString()), Convert.ToByte(Row["croupier"].ToString()), Row["talent_status"].ToString(), Convert.ToByte(Row["targeted_buy"]), Convert.ToInt32(Row["bonus_points"]));
        }
    }
}