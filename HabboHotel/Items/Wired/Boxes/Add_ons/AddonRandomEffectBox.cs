using System;
using System.Collections.Concurrent;
using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Users;
using Bios.Communication.Packets.Incoming;

namespace Bios.HabboHotel.Items.Wired.Boxes.Add_ons
{
    class AddonRandomEffectBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.AddonRandomEffect; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public AddonRandomEffectBox(Room Instance, Item Item)
        {
            this.Instance = Instance;
            this.Item = Item;
            this.SetItems = new ConcurrentDictionary<int, Item>();

            if (this.SetItems.Count > 0)
                this.SetItems.Clear();
        }

        public void HandleSave(ClientPacket Packet)
        {

        }

        public bool Execute(params object[] Params)
        {
            return true;
        }
    }
}