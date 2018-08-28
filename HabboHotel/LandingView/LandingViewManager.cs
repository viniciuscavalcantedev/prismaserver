using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using Bios.Database.Interfaces;
using Bios.HabboHotel.LandingView.Promotions;
using log4net;
using Bios.Communication.Packets.Incoming.LandingView;

namespace Bios.HabboHotel.LandingView
{
    public class LandingViewManager
    {
        private static readonly ILog log = LogManager.GetLogger("Bios.HabboHotel.LandingView.LandingViewManager");

        internal BonusRareList BonusRareLists;

        private Dictionary<int, Promotion> _promotionItems;

        public Dictionary<uint, UserRank> ranks;
        public List<UserCompetition> usersWithRank;

        public LandingViewManager()
        {
            this._promotionItems = new Dictionary<int, Promotion>();

            this.LoadPromotions();

            this.LoadBonusRare(BiosEmuThiago.GetDatabaseManager().GetQueryReactor());

            this.LoadHallOfFame();
        }

        public void LoadHallOfFame()
        {

            ranks = new Dictionary<uint, UserRank>();
            usersWithRank = new List<UserCompetition>();

            usersWithRank = new List<UserCompetition>();
            usersWithRank.Clear();
            using (IQueryAdapter queryReactor = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                ranks = new Dictionary<uint, UserRank>();
                usersWithRank = new List<UserCompetition>();

                queryReactor.SetQuery("SELECT * FROM `users` WHERE `gotw_points` >= '1' AND `rank` = '1' ORDER BY `gotw_points` DESC LIMIT 16");
                DataTable gUsersTable = queryReactor.getTable();

                foreach (DataRow Row in gUsersTable.Rows)
                {
                    var staff = new UserCompetition(Row);
                    if (!usersWithRank.Contains(staff))
                        usersWithRank.Add(staff);
                }
            }
        }

        public void LoadPromotions()
        {
            if (this._promotionItems.Count > 0)
                this._promotionItems.Clear();

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `server_landing` ORDER BY `id` DESC");
                DataTable GetData = dbClient.getTable();

                if (GetData != null)
                {
                    foreach (DataRow Row in GetData.Rows)
                    {
                        this._promotionItems.Add(Convert.ToInt32(Row[0]), new Promotion((int)Row[0], Row[1].ToString(), Row[2].ToString(), Row[3].ToString(), Convert.ToInt32(Row[4]), Row[5].ToString(), Row[6].ToString()));
                    }
                }
            }


            log.Info("» Vista de pouso -> PRONTO - BY: Thiago Araujo! ");
        }

        public ICollection<Promotion> GetPromotionItems()
        {
            return this._promotionItems.Values;
        }
        public void LoadBonusRare(IQueryAdapter dbClient)
        {

            BonusRareLists = null;

            dbClient.SetQuery("SELECT * FROM landing_bonus WHERE enable = 'true' LIMIT 1");
            var row = dbClient.getRow();

            if (row == null)
                return;

            BonusRareLists = new BonusRareList((string)row["item_name"], (int)row["base_item"], (int)row["bonus_score"]);
            log.Info("» Vista do hotel -> PRONTO - BY: Thiago Araujo");
        }


        public class BonusRareList
        {
            internal string Item;
            internal int Baseid, Score;
            internal BonusRareList(string item, int baseid, int score)
            {
                Item = item;
                Baseid = baseid;
                Score = score;
            }
        }
    }
}