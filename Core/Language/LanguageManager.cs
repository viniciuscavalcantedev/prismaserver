using System.Data;
using System.Collections.Generic;
using log4net;
using Bios.Database.Interfaces;

namespace Bios.Core.Language
{
    public class LanguageManager
    {
        private Dictionary<string, string> _values = new Dictionary<string, string>();

        private static readonly ILog log = LogManager.GetLogger("Bios.Core.Language.LanguageManager");

        public LanguageManager()
        {
            this._values = new Dictionary<string, string>();
        }

        public void Init()
        {
            if (this._values.Count > 0)
                this._values.Clear();

            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `server_locale`");
                DataTable Table = dbClient.getTable();

                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                    {
                        this._values.Add(Row["key"].ToString(), Row["value"].ToString());
                    }
                }
            }

            log.Info("» PRONTO - BY: Thiago Araujo(s): " + this._values.Count + " línguas locais.");
        }

        public string TryGetValue(string value)
        {
            return this._values.ContainsKey(value) ? this._values[value] : "Nenhum idioma encontrado para [" + value + "]";
        }
    }
}
