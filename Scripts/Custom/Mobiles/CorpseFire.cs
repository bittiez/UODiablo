namespace Server.Mobiles
{
    [CorpseName("a cold rotting corpse")]
    public class CorpseFire : BaseCreature
    {
        [Constructable]
        public CorpseFire()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "corpsefire";
            Body = 3;
            BaseSoundID = 471;
            Hue = 1152;

            MonsterLevelNormal = 4;
            MonsterLevelNightmare = 39;
            MonsterLevelHell = 82;
            
            SetStr(90, 125);
            SetDex(20, 30);
            SetInt(26, 40);

            SetHits(100, 200);

            SetDamage(4, 7);

            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Cold, 50, 80);
            SetResistance(ResistanceType.Poison, 30, 45);

            SetSkill(SkillName.MagicResist, 15.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 3000;
            Karma = -3000;
        }

        public CorpseFire(Serial serial)
            : base(serial)
        {
        }

        public override bool BleedImmune => true;

        public override Poison PoisonImmune => Poison.Regular;

        public override TribeType Tribe => TribeType.Undead;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
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
