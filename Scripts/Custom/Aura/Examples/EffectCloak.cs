using Server.Engines.Craft;
using Server.Engines.VeteranRewards;

namespace Server.Items
{
	[Flipable]
	public class EffectCloak : BaseCloak
	{
		private Bittiez.Aura.Aura EffectAura;

		[Constructable]
		public EffectCloak()
			: this(0)
		{
		}

		[Constructable]
		public EffectCloak(int hue)
			: base(0x1515, hue)
		{
			AuraSetup();
			Weight = 5.0;
		}

		private void AuraSetup()
		{
			EffectAura = new Bittiez.Aura.Aura(this, 8, Bittiez.Aura.AURATYPE.EFFECTAURA);
			EffectAura.AffectsSelf = true;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (EffectAura.ToggleAura()) from.SendMessage("Aura on"); else from.SendMessage("Aura off");
			base.OnDoubleClick(from);
		}

		public EffectCloak(Serial serial)
			: base(serial)
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
