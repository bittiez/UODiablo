// By Neon
// Improved By Dddie

using Server.Network;

namespace Server.Items
{
	public class MagicHealingCrystalBall : Item
	{
		private Bittiez.Aura.Aura m_HealingAura;

		[Constructable]
		public MagicHealingCrystalBall() : base(0xE2E)
		{
			m_HealingAura = new Bittiez.Aura.Aura(this, 8, Bittiez.Aura.AURATYPE.HEALING); //Set up the initial aura
			m_HealingAura.HealingAmount = 20; //Set the aura to heal for 20 hits :O

			Name = "a healing crystal ball";
			Weight = 10;
			Stackable = false;
			LootType = LootType.Blessed;
			Light = LightType.Circle150;
		}

		public override void OnSectorActivate()
		{
			m_HealingAura.EnableAura();
			base.OnSectorActivate();
		}

		public override void OnSectorDeactivate()
		{
			m_HealingAura.DisableAura();
			base.OnSectorDeactivate();
		}

		public override void OnDoubleClick(Mobile from)
		{
			PublicOverheadMessage(MessageType.Regular, 0x3B2, 1007000 + Utility.Random(28));
		}

		public MagicHealingCrystalBall(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version 
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
			m_HealingAura = new Bittiez.Aura.Aura(this, 8, Bittiez.Aura.AURATYPE.HEALING); //Set up the initial aura
			m_HealingAura.HealingAmount = 20; //Set the aura to heal for 20 hits :O
		}
	}
}