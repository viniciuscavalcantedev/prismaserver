using Bios.HabboHotel.Items;
using System;

namespace Bios.HabboHotel.Catalog.FurniMatic
{
    public class FurniMaticRewards
    {
        public Int32 DisplayId;
        public Int32 BaseId;
        public Int32 Level;

        public FurniMaticRewards(int displayId, int baseId, int level)
        {
            DisplayId = displayId;
            BaseId = baseId;
            Level = level;
        }

        public ItemData GetBaseItem()
        {
            ItemData data = null;
            if (BiosEmuThiago.GetGame().GetItemManager().GetItem(BaseId, out data)) return data;
            return null;
        }
    }
}