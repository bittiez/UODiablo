using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a kazra corpse")]
    public class Kazra : BaseCreature
    {
        [Constructable]
        public Kazra()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "moon clan kazra";
            Body = 138;
            BaseSoundID = 0x45A;

            MonsterLevelNormal = 4;
            MonsterLevelNightmare = 37;
            MonsterLevelHell = 68;
            
            SetStr(80, 95);
            SetDex(50, 75);
            SetInt(10, 20);

            SetHits(50, 60);

            SetDamage(4, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 35);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.MagicResist, 70.1, 85.0);
            SetSkill(SkillName.Swords, 60.1, 85.0);
            SetSkill(SkillName.Tactics, 75.1, 90.0);
            SetSkill(SkillName.Wrestling, 60.1, 85.0);

            Fame = 600;
            Karma = -600;
        }

        public Kazra(Serial serial)
                   : base(serial)
        {
        }

        //public override void GenerateLoot()
        //{
        //    AddLoot(LootPack.Meager);
        //}

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
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
