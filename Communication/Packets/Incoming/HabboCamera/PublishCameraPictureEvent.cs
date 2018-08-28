using Bios.Communication.Packets.Outgoing.HabboCamera;
using Bios.HabboHotel.GameClients;
using Bios.HabboHotel.Rooms.Camera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bios.Communication.Packets.Incoming.HabboCamera
{
    class PublishCameraPictureEvent : IPacketEvent
    {
        public void Parse(GameClient Session, ClientPacket Packet)
        {
            var pic = HabboCameraManager.GetUserPurchasePic(Session);
            if (pic == null)
                return;

            int InsetId;
            using (var Adap = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
            {
                Adap.SetQuery("INSERT INTO server_pictures_publish (picture_id, usuariothiago) VALUES (@pic, @thiago)");
                Adap.AddParameter("pic", pic.Id);
                Adap.AddParameter("thiago", Session.GetHabbo().Username);
                InsetId = (int)Adap.InsertQuery();
            }

            Session.SendMessage(new CameraFinishPublishComposer(InsetId));

        }
    }
}
