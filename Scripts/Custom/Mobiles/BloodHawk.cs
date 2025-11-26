namespace Server.Mobiles
{
    [CorpseName("bloodhawk corpse")]
    public class BloodHawk : BaseCreature
    {
        [Constructable]
        public BloodHawk()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "bloodhawk";
            Body = 5;
            BaseSoundID = 0x2EE;
            Hue = 1209;

            MonsterLevel = 6;

            SetStr(50, 70);
            SetDex(36, 60);
            SetInt(8, 20);

            SetHits(30, 45);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 0, 15);
            SetResistance(ResistanceType.Fire, 10, 15);
            SetResistance(ResistanceType.Cold, 0, 15);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 5, 10);

            SetSkill(SkillName.MagicResist, 15.3, 30.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.Wrestling, 60.1, 70.0);

            Fame = 150;
            Karma = -150;

            VirtualArmor = 20;
        }

        public BloodHawk(Serial serial)
            : base(serial)
        {
        }

        public override int Meat => 1;

        public override MeatType MeatType => MeatType.Bird;

        public override int Feathers => 20;

        public override bool CanFly => true;

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
