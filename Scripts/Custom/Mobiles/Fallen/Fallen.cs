namespace Server.Mobiles
{
    internal class Fallen : BaseCreature
    {
        public Fallen(Serial serial) : base(serial)
        {
        }

        [Constructable]
        public Fallen() : base(AIType.AI_Melee, FightMode.Closest, 8, 1, 0.2, 0.4)
        {
            Name = "fallen";
            Body = RandomImpl.NextBool() ? 0xF5 : 0xFF;

            MonsterLevelNormal = 1;
            MonsterLevelNightmare = 36;
            MonsterLevelHell = 67;
            
            SetStr(36, 41);
            SetDex(25, 45);
            SetInt(15, 30);

            SetHits(20, 38);

            SetDamage(1, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.Wrestling, 35.1, 50.0);

            Fame = 100;
            Karma = -100;

            VirtualArmor = 10;

            ControlSlots = 1;

            PackBodyPartOrBones();
        }

        public override TribeType Tribe => TribeType.Undead;

        //public override void GenerateLoot()
        //{
        //    AddLoot(LootPack.Poor);
        //}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
}
