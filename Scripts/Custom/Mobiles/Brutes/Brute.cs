namespace Server.Mobiles
{
    public class Brute : BaseCreature
    {
        [Constructable]
        public Brute()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Brute";
            Body = 1;
            BaseSoundID = 427;

            MonsterLevelNormal = 5;
            MonsterLevelNightmare = 38;
            MonsterLevelHell = 69;
            
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

        public Brute(Serial serial)
            : base(serial)
        {
        }

        public override int Meat => 2;

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
