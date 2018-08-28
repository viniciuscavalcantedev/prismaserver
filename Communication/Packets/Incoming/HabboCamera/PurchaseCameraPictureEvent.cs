using Bios.Communication.Packets.Outgoing.HabboCamera;
using Bios.Communication.Packets.Outgoing.Rooms.Notifications;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Items;
using Bios.HabboHotel.Rooms.Camera;
using Bios.Core;

namespace Bios.Communication.Packets.Incoming.HabboCamera
{
    class PurchaseCameraPictureEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            int PictureBaseId = 202030;
            var conf = ExtraSettings.CAMERA_ITEMID;
            if (!int.TryParse(conf, out PictureBaseId))
            {
                Session.SendMessage(new RoomNotificationComposer("Por favor, fale com a equipe de desenvolvedores que sua foto não foi identifica na db.\n Desculpe pelo inconveniente!", "error"));
                return;
            }
            BiosEmuThiago.GetGame().GetAchievementManager().ProgressAchievement(Session, "ACH_CameraPhotoCount", 1);
            var pic = HabboCameraManager.GetUserPurchasePic(Session);
            ItemData ibase = null;
            if (pic == null || !BiosEmuThiago.GetGame().GetItemManager().GetItem(PictureBaseId, out ibase))
                return;

            Session.GetHabbo().GetInventoryComponent().AddNewItem(0, ibase.Id, pic.Id.ToString(), 0, true, false, 0, 0);
            Session.GetHabbo().GetInventoryComponent().UpdateItems(false);
            
            Session.SendMessage(new CamereFinishPurchaseComposer());
        }
    }
}
