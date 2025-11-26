// By Neon
// Improved By Dddie

using Server.Network;

namespace Server.Items
{
	public class MagicManaCrystalBall : Item
	{
		private Bittiez.Aura.Aura m_ManaRegenAura;

		[Constructable]
		public MagicManaCrystalBall() : base(0xE2E)
		{
			AuraSetup();

			Name = "a mana aura crystal ball";
			Weight = 10;
			Stackable = false;
			LootType = LootType.Blessed;
			Light = LightType.Circle150;
		}

		private void AuraSetup()
		{
			m_ManaRegenAura = new Bittiez.Aura.Aura(this, 8, Bittiez.Aura.AURATYPE.MANA_REGEN); //Set up the initial aura
			m_ManaRegenAura.HealingAmount = 1;
			m_ManaRegenAura.AffectsSelf = true;
		}

		public override void OnSectorActivate()
		{
			m_ManaRegenAura.EnableAura();
			base.OnSectorActivate();
		}

		public override void OnSectorDeactivate()
		{
			m_ManaRegenAura.DisableAura();
			base.OnSectorDeactivate();
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (m_ManaRegenAura.ToggleAura()) { from.SendMessage("Aura on!"); }
			else from.SendMessage("Aura off!");
		}

		public MagicManaCrystalBall(Serial serial) : base(serial)
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
			AuraSetup();
		}
	}
}