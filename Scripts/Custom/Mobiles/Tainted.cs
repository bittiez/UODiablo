namespace Server.Mobiles
{
    [CorpseName("tainted corpse")]
    public class Tainted : BaseCreature
    {

        [Constructable]
        public Tainted()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "tainted";
            Body = 241;

            MonsterLevel = 11;

            SetStr(100, 150);
            SetDex(100, 150);
            SetInt(171, 195);

            SetHits(90, 120);

            SetDamage(7, 11);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 45, 60);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 35, 50);
            SetResistance(ResistanceType.Poison, 45, 65);
            SetResistance(ResistanceType.Energy, 45, 65);

            SetSkill(SkillName.Anatomy, 65.1, 75.0);
            SetSkill(SkillName.MagicResist, 65.1, 80.0);
            SetSkill(SkillName.Tactics, 76.1, 80.0);
            SetSkill(SkillName.Wrestling, 70.1, 80.0);

            Fame = 800;
            Karma = -800;

        }

        public Tainted(Serial serial)
            : base(serial)
        {
        }

        public override int GetAngerSound()
        {
            return 0x4E3;
        }

        public override int GetIdleSound()
        {
            return 0x4E2;
        }

        public override int GetAttackSound()
        {
            return 0x4E1;
        }

        public override int GetHurtSound()
        {
            return 0x4E4;
        }

        public override int GetDeathSound()
        {
            return 0x4E0;
        }

        //public override void GenerateLoot()
        //{
        //    AddLoot(LootPack.Average, 3);
        //}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
