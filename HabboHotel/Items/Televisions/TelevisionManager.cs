using System;
using System.Data;
using System.Collections.Generic;
using Bios.Database.Interfaces;
using log4net;

namespace Bios.HabboHotel.Items.Televisions
{
    public class TelevisionManager
    {
        private static readonly ILog log = LogManager.GetLogger("Bios.HabboHotel.Items.Televisions.TelevisionManager");
        public Dictionary<int, TelevisionItem> _televisions;

        public TelevisionManager()
        {
            this._televisions =  new Dictionary<int, TelevisionItem>();

            this.Init();
        }

        public void Init()
        {
            if (this._televisions.Count > 0)
                _televisions.Clear();

            DataTable getData = null;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor()) 
            {
                dbClient.SetQuery("SELECT * FROM `items_youtube` ORDER BY `id` DESC");
                getData = dbClient.getTable();

                if (getData != null)
                {
                    foreach (DataRow Row in getData.Rows)
                    {
                        this._televisions.Add(Convert.ToInt32(Row["id"]), new TelevisionItem(Convert.ToInt32(Row["id"]), Row["youtube_id"].ToString(), Row["title"].ToString(), Row["description"].ToString(), BiosEmuThiago.EnumToBool(Row["enabled"].ToString())));
                    }
                }
            }


            log.Info("» YouTube Manager -> PRONTO - BY: Thiago Araujo");
        }


        public ICollection<TelevisionItem> TelevisionList
        {
            get
            {
                return this._televisions.Values;
            }
        }

        public bool TryGet(int ItemId, out TelevisionItem TelevisionItem)
        {
            if (this._televisions.TryGetValue(ItemId, out TelevisionItem))
                return true;
            return false;
        }
    }
}
