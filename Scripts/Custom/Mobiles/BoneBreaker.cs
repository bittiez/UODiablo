namespace Server.Mobiles
{
    [CorpseName("a skeletal corpse")]
    public class BoneBreaker : BaseCreature
    {
        [Constructable]
        public BoneBreaker()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "bonebreaker";
            Body = 147;
            BaseSoundID = 451;
            Hue = 1209;

            MonsterLevel = 5;

            SetStr(200, 250);
            SetDex(75, 95);
            SetInt(40, 60);

            SetHits(200, 250);

            SetDamage(9, 18);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Tactics, 85.1, 100.0);
            SetSkill(SkillName.Wrestling, 85.1, 95.0);

            Fame = 1000;
            Karma = -1000;
        }

        public BoneBreaker(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune => true;

        public override TribeType Tribe => TribeType.Undead;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.UOD_AllRunesForBosses);
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
