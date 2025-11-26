using Server.Mobiles;

namespace Server.Custom.Mobiles
{
    public class SummonedSkeletonMage : BaseCreature
	{
		public SummonedSkeletonMage() : base(AIType.AI_Mage, FightMode.Closest, 10, 8, 0.2, 0.4)
		{
			Name = "summoned skeleton mage";

			Body = 148;
			BaseSoundID = 451;

			SetStr(56, 70);
			SetDex(56, 75);
			SetInt(45, 120);

			SetHits(34, 48);

			SetDamage(3, 7);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 15, 20);
			SetResistance(ResistanceType.Fire, 5, 10);
			SetResistance(ResistanceType.Cold, 25, 40);
			SetResistance(ResistanceType.Poison, 25, 35);
			SetResistance(ResistanceType.Energy, 5, 15);

			SetSkill(SkillName.EvalInt, 65.1, 85);
			SetSkill(SkillName.Magery, 65.1, 85);
			SetSkill(SkillName.MagicResist, 65.0, 85);
			SetSkill(SkillName.Tactics, 55.0, 85);
			SetSkill(SkillName.Wrestling, 20.2, 50);

			Fame = 450;
			Karma = -450;

			ControlSlots = 0;
            FollowRange = 2;
        }

        public override bool BleedImmune => true;
		public override Poison PoisonImmune => Poison.Lesser;
		public override TribeType Tribe => TribeType.Undead;

		public override void GenerateLoot()
		{
		}

		public SummonedSkeletonMage(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
