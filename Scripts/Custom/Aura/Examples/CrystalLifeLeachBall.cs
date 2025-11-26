// By Neon
// Improved By Dddie

using Server.Network;

namespace Server.Items
{
	public class MagicLifeLeachCrystalBall : Item
	{
		private Bittiez.Aura.Aura m_LifeLeachAura;

		[Constructable]
		public MagicLifeLeachCrystalBall() : base(0xE2E)
		{
			AuraSetup();

			Name = "a life leach aura crystal ball";
			Weight = 10;
			Stackable = false;
			LootType = LootType.Blessed;
			Light = LightType.Circle150;
		}

		private void AuraSetup()
		{
			m_LifeLeachAura = new Bittiez.Aura.Aura(this, 8, Bittiez.Aura.AURATYPE.LIFE_LEACH); //Set up the initial aura
			m_LifeLeachAura.DamageAmount = 1; //The aura will deal 1 damage to the target
			m_LifeLeachAura.HealingAmount = 1; //The aura will heal for 1 for each target damaged
		}

		public override void OnSectorActivate()
		{
			m_LifeLeachAura.EnableAura();
			base.OnSectorActivate();
		}

		public override void OnSectorDeactivate()
		{
			m_LifeLeachAura.DisableAura();
			base.OnSectorDeactivate();
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (m_LifeLeachAura.ToggleAura()) { from.SendMessage("Aura on!"); }
			else from.SendMessage("Aura off!");
		}

		public MagicLifeLeachCrystalBall(Serial serial) : base(serial)
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