// By Neon
// Improved By Dddie

using Server.Network;

namespace Server.Items
{
	public class MagicDamageCrystalBall : Item
	{
		private Bittiez.Aura.Aura m_DamageingAura;

		[Constructable]
		public MagicDamageCrystalBall() : base(0xE2E)
		{
			AuraSetup();

			Name = "a damaging aura crystal ball";
			Weight = 10;
			Stackable = false;
			LootType = LootType.Blessed;
			Light = LightType.Circle150;
		}

		private void AuraSetup()
		{
			m_DamageingAura = new Bittiez.Aura.Aura(this, 8, Bittiez.Aura.AURATYPE.DAMAGE); //Set up the initial aura
			m_DamageingAura.DamageAmount = 1; //Set the aura to do 1 damage
		}

		public override void OnSectorActivate()
		{
			m_DamageingAura.EnableAura();
			base.OnSectorActivate();
		}

		public override void OnSectorDeactivate()
		{
			m_DamageingAura.DisableAura();
			base.OnSectorDeactivate();
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (m_DamageingAura.ToggleAura()) { from.SendMessage("Aura on!"); }
			else from.SendMessage("Aura off!");
		}

		public MagicDamageCrystalBall(Serial serial) : base(serial)
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