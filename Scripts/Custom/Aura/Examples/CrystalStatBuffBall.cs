// By Neon
// Improved By Dddie

using Server.Network;

namespace Server.Items
{
	public class MagicStatBuffCrystalBall : Item
	{
		private Bittiez.Aura.Aura m_StatBuffAura;

		[Constructable]
		public MagicStatBuffCrystalBall() : base(0xE2E)
		{
			AuraSetup();

			Name = "a stat buff aura crystal ball";
			Weight = 10;
			Stackable = false;
			LootType = LootType.Blessed;
			Light = LightType.Circle150;
		}

		private void AuraSetup()
		{
			m_StatBuffAura = new Bittiez.Aura.Aura(this, 8, Bittiez.Aura.AURATYPE.STAT_BUFF); //Set up the initial aura
			m_StatBuffAura.StatBuff = 3; //Set the aura to add 3 to all stats
			m_StatBuffAura.AffectsSelf = true; //Will affect the aura owner
		}

		public override void OnSectorActivate()
		{
			m_StatBuffAura.EnableAura();
			base.OnSectorActivate();
		}

		public override void OnSectorDeactivate()
		{
			m_StatBuffAura.DisableAura();
			base.OnSectorDeactivate();
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (m_StatBuffAura.ToggleAura()) { from.SendMessage("Aura on!"); }
			else from.SendMessage("Aura off!");
		}

		public MagicStatBuffCrystalBall(Serial serial) : base(serial)
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