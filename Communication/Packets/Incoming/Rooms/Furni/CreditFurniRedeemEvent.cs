using Bios.HabboHotel.Rooms;
using Bios.HabboHotel.Items;
using Bios.Communication.Packets.Outgoing.Inventory.Purse;
using Bios.Communication.Packets.Outgoing.Inventory.Furni;
using Bios.Database.Interfaces;

namespace Bios.Communication.Packets.Incoming.Rooms.Furni
{
	class CreditFurniRedeemEvent : IPacketEvent
	{
		public void Parse(HabboHotel.GameClients.GameClient Session, ClientPacket Packet)
		{
			if (!Session.GetHabbo().InRoom)
				return;


			if (!BiosEmuThiago.GetGame().GetRoomManager().TryGetRoom(Session.GetHabbo().CurrentRoomId, out Room Room))
				return;

			if (!Room.CheckRights(Session, true))
				return;

			if (BiosEmuThiago.GetGame().GetSettingsManager().TryGetValue("room.item.exchangeables.enabled") != "1")
			{
				Session.SendNotification("Desculpe, mas o catálogo foi desativado!");
				return;
			}

			Item Exchange = Room.GetRoomItemHandler().GetItem(Packet.PopInt());
			if (Exchange == null)
				return;

			if (!Exchange.GetBaseItem().ItemName.StartsWith("CF_") && !Exchange.GetBaseItem().ItemName.StartsWith("CFC_") && !Exchange.GetBaseItem().ItemName.StartsWith("DF_") && !Exchange.GetBaseItem().ItemName.StartsWith("DFD_") && !Exchange.GetBaseItem().ItemName.StartsWith("DC_") && !Exchange.GetBaseItem().ItemName.StartsWith("DCK_"))
				return;

			string[] Split = Exchange.GetBaseItem().ItemName.Split('_');
			int Valuee = int.Parse(Split[1]);

			if (Valuee > 0)
			{
                // Os 3 fix das 3 tipos de meodas fixados por Thiago Araujo
				if (Exchange.GetBaseItem().ItemName.StartsWith("CF_") || Exchange.GetBaseItem().ItemName.StartsWith("CFC_"))
				{
					Session.GetHabbo().Credits += Valuee;
					Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
				}
				else if (Exchange.GetBaseItem().ItemName.StartsWith("DF_") || Exchange.GetBaseItem().ItemName.StartsWith("DFD_"))
				{
					Session.GetHabbo().Diamonds += Valuee;
					Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Diamonds, Valuee, 5));
				}
                else if (Exchange.GetBaseItem().ItemName.StartsWith("DC_") || Exchange.GetBaseItem().ItemName.StartsWith("DCK_"))
                {
                    Session.GetHabbo().Duckets += Valuee;
                    Session.SendMessage(new HabboActivityPointNotificationComposer(Session.GetHabbo().Duckets, Valuee, 5));
                }
            }

			if (Exchange.Data.InteractionType != InteractionType.EXCHANGE)
				return;

			int Value = Exchange.Data.BehaviourData;

			if (Value > 0)
			{
				Session.GetHabbo().Credits += Value;
				Session.SendMessage(new CreditBalanceComposer(Session.GetHabbo().Credits));
			}

			using (IQueryAdapter dbClient = BiosEmuThiago.GetDatabaseManager().GetQueryReactor())
			{
				dbClient.SetQuery("DELETE FROM `items` WHERE `id` = @exchangeId LIMIT 1");
				dbClient.AddParameter("exchangeId", Exchange.Id);
				dbClient.RunQuery();
			}

			Session.SendMessage(new FurniListUpdateComposer());
			Room.GetRoomItemHandler().RemoveFurniture(null, Exchange.Id, false);
			Session.GetHabbo().GetInventoryComponent().RemoveItem(Exchange.Id);

		}
	}
}
