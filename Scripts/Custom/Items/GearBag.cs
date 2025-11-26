using Server.ContextMenus;
using Server.Items;
using System.Collections.Generic;

namespace Server.Custom.Items
{
    public class GearBag : Bag
	{
		[Constructable]
		public GearBag()
		{
			Hue = 293;
		}
		public GearBag(Serial serial) : base(serial) { }

		public override void GetProperties(ObjectPropertyList list)
		{
			base.GetProperties(list);
			list.Add("This bag can store all your currently equiped items.");
		}

		public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
		{
			base.GetContextMenuEntries(from, list);
			list.Add(new GearBagUseContext(from, this));
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
	}

	public class GearBagUseContext : ContextMenuEntry
	{
		private readonly GearBag m_Item;
		private readonly Mobile m_Mobile;
		public GearBagUseContext(Mobile from, Item item)
			: base(6190)
		{
			this.m_Item = (GearBag)item;
			this.m_Mobile = from;
		}

		public override void OnClick()
		{
			if (!m_Item.IsAccessibleTo(m_Mobile)) return;

			List<Item> itemsInBag = new List<Item>(m_Item.Items);
			List<Item> currentEquipment = CurrentEquipedGear();

			foreach (Item item in currentEquipment)
			{
				if (item != null)
				{
					m_Item.AddItem(item);
				}
			}

			foreach (Item item in itemsInBag)
			{
				m_Mobile.EquipItem(item);
			}
		}

		private List<Item> CurrentEquipedGear()
		{
			List<Item> items = new List<Item>();

			items.Add(m_Mobile.FindItemOnLayer(Layer.Arms));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Bracelet));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Cloak));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Earrings));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Gloves));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Helm));
			items.Add(m_Mobile.FindItemOnLayer(Layer.InnerLegs));
			items.Add(m_Mobile.FindItemOnLayer(Layer.InnerTorso));
			items.Add(m_Mobile.FindItemOnLayer(Layer.MiddleTorso));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Neck));
			items.Add(m_Mobile.FindItemOnLayer(Layer.OneHanded));
			items.Add(m_Mobile.FindItemOnLayer(Layer.OuterLegs));
			items.Add(m_Mobile.FindItemOnLayer(Layer.OuterTorso));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Pants));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Ring));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Shirt));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Shoes));
			items.Add(m_Mobile.FindItemOnLayer(Layer.TwoHanded));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Talisman));
			items.Add(m_Mobile.FindItemOnLayer(Layer.Waist));

			return items;
		}

	}
}
