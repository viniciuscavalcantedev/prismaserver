﻿using log4net;
using Bios.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Bios.HabboHotel.Rooms.Chat.Styles
{
    public sealed class ChatStyleManager
    {
        private static readonly ILog log = LogManager.GetLogger("Bios.HabboHotel.Rooms.Chat.Styles.ChatStyleManager");

        private readonly Dictionary<int, ChatStyle> _styles;

        public ChatStyleManager()
        {
            this._styles = new Dictionary<int, ChatStyle>();
        }

        public void Init()
        {
            if (this._styles.Count > 0)
                this._styles.Clear();

            DataTable Table = null;
            using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("SELECT * FROM `room_chat_styles`;");
                Table = dbClient.getTable();

                if (Table != null)
                {
                    foreach (DataRow Row in Table.Rows)
                    {
                        try
                        {
                            if (!this._styles.ContainsKey(Convert.ToInt32(Row["id"])))
                                this._styles.Add(Convert.ToInt32(Row["id"]), new ChatStyle(Convert.ToInt32(Row["id"]), Convert.ToString(Row["name"]), Convert.ToString(Row["required_right"])));
                        }
                        catch (Exception ex)
                        {
                            log.Error("Não foi possível carregar ChatBubble para ID [" + Convert.ToInt32(Row["id"]) + "]", ex);
                        }
                    }
                }
            }

            log.Info("» Carrengado: " + this._styles.Count + " estilos de chat.");
        }

        public bool TryGetStyle(int Id, out ChatStyle Style)
        {
            return this._styles.TryGetValue(Id, out Style);
        }
    }
}
