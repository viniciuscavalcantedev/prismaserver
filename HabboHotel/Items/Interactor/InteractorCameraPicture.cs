using Bios.HabboHotel.Rooms.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bios.HabboHotel.GameClients;

namespace Bios.HabboHotel.Items.Interactor
{
    class InteractorCameraPicture : IFurniInteractor
    {
        public string GetJsonData(Item item)
        {
            var defaultData = "{\"t\":\"0\",\"u\":\"1\", \"n\":\"Lucas\",\"s\":\"1\",\"url\":\"http://habbocamera.dev/pictures/10.png\", \"w\": \"http://habbocamera.dev/pictures/10.png\", \"m\": \"lalalala\"}";
            int picid;
            if (!int.TryParse(item.ExtraData, out picid))
                return defaultData;

            var picdata = HabboCameraManager.GetPicture(picid);
            if (picdata == null)
                return defaultData;

            var Onwer = BiosEmuThiago.GetHabboById(picdata.UserId);

            if (Onwer == null)
                return defaultData;

            var str = string.Concat(
                "{\"t\":\"", picdata.Timestamp, "\",", // Time
                "\"u\":\"", item.Id, "\",",// Image Unique ID
                "\"n\":\"", Onwer.Username, "\",", //Owner Name
                "\"s\":\"", Onwer.Id, "\",", //Owner ID
                "\"url\":\"", picdata.Url, "\",", //Owner Name
                "\"m\":\"aaaaaaa\",", //image desc
                "\"w\":\"", picdata.Url, "\",", //image desc
                "}");

            return str;

        }
        public void OnPlace(GameClient Session, Item Item)
        {
        }

        public void OnRemove(GameClient Session, Item Item)
        {
        }

        public void OnTrigger(GameClient Session, Item Item, int Request, bool HasRights)
        {
        }

        public void OnWiredTrigger(Item Item)
        {
        }
    }
}
