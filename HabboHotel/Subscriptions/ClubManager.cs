
using Bios.Communication.Packets.Outgoing.Handshake;
using Bios.Database.Interfaces;
using Bios.HabboHotel.GameClients;
using System.Collections.Generic;
using Bios.HabboHotel.Users.UserData;

namespace Bios.HabboHotel.Subscriptions
{
    internal class ClubManager
    {
        private readonly int UserId;
        private readonly Dictionary<string, Subscription> Subscriptions;

        internal ClubManager(int userID, UserData userData)
        {
           UserId = userID;
           Subscriptions = userData.subscriptions;
        }

        internal void Clear()
        {
            Subscriptions.Clear();
        }

        internal Subscription GetSubscription(string SubscriptionId)
        {
            if (Subscriptions.ContainsKey(SubscriptionId))
            {
                return Subscriptions[SubscriptionId];
            }
            else
            {
                return null;
            }
        }

        internal bool HasSubscription(string SubscriptionId)
        {
            if (!Subscriptions.ContainsKey(SubscriptionId))
            {
                return false;
            }

            Subscription subscription = Subscriptions[SubscriptionId];
            return subscription.IsValid();
        }

        internal void AddOrExtendSubscription(string SubscriptionId, int DurationSeconds, GameClient Session)
        {
            SubscriptionId = SubscriptionId.ToLower();

            var clientByUserId = BiosEmuThiago.GetGame().GetClientManager().GetClientByUserID(UserId);
            if (Subscriptions.ContainsKey(SubscriptionId))
            {
                Subscription subscription = Subscriptions[SubscriptionId];

                if (subscription.IsValid())
                {
                    subscription.ExtendSubscription(DurationSeconds);
                }
                else
                {
                    subscription.SetEndTime((int)BiosEmuThiago.GetUnixTimestamp() + DurationSeconds);
                }

                using (IQueryAdapter adapter = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    adapter.SetQuery(string.Concat(new object[] { "UPDATE user_subscriptions SET timestamp_expire = ", subscription.ExpireTime, " WHERE user_id = ", this.UserId, " AND subscription_id = '", subscription.SubscriptionId, "'" }));
                    adapter.RunQuery();
                }
                BiosEmuThiago.GetGame().GetAchievementManager().TryProgressHabboClubAchievements(clientByUserId);
            }
            else
            {
                int unixTimestamp = (int)BiosEmuThiago.GetUnixTimestamp();
                int timeExpire = (int)BiosEmuThiago.GetUnixTimestamp() + DurationSeconds;
                string SubscriptionType = SubscriptionId;
                Subscription subscription2 = new Subscription(SubscriptionId, timeExpire, unixTimestamp);

                using (IQueryAdapter adapter = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
                {
                    adapter.SetQuery(string.Concat(new object[] { "INSERT INTO user_subscriptions (user_id,subscription_id,timestamp_activated,timestamp_expire) VALUES (", this.UserId, ",'", SubscriptionType, "',", unixTimestamp, ",", timeExpire, ")" }));
                    adapter.RunQuery();
                }

                Subscriptions.Add(subscription2.SubscriptionId.ToLower(), subscription2);
                BiosEmuThiago.GetGame().GetAchievementManager().TryProgressHabboClubAchievements(clientByUserId);
            }
        }


        internal void ReloadSubscription(GameClient Session)
        {
            Session.SendMessage(new UserRightsComposer(Session.GetHabbo()));
        }
    }
}