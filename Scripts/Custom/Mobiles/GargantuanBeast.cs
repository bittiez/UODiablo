namespace Server.Mobiles
{
    [CorpseName("gargantuan beast corpse")]
    public class GargantuanBeast : BaseCreature
    {
        [Constructable]
        public GargantuanBeast()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "gargantuan beast";
            Body = 1;
            BaseSoundID = 427;

            MonsterLevel = 2;

            SetStr(165, 175);
            SetDex(45, 60);
            SetInt(10, 30);

            SetHits(100, 115);

            SetDamage(5, 9);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 35);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 15, 25);
            SetResistance(ResistanceType.Poison, 15, 25);
            SetResistance(ResistanceType.Energy, 10, 25);

            SetSkill(SkillName.MagicResist, 55.1, 70.0);
            SetSkill(SkillName.Tactics, 60.1, 70.0);
            SetSkill(SkillName.Wrestling, 70.1, 80.0);

            Fame = 1000;
            Karma = -1000;
        }

        public GargantuanBeast(Serial serial)
            : base(serial)
        {
        }

        public override int Meat => 2;
        //public override void GenerateLoot()
        //{
        //    AddLoot(LootPack.Average);
        //}

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
