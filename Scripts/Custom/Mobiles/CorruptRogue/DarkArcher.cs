using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class DarkArcher : BaseCreature
    {
        [Constructable]
        public DarkArcher()
            : base(AIType.AI_Archer, FightMode.Closest, 10, 8, 0.2, 0.4)
        {
            Name = "dark archer";
            Body = 0x191;
            Female = true;

            MonsterLevel = 7;

            SetStr(50, 65);
            SetDex(80, 95);
            SetInt(60, 75);

            SetDamage(6, 10);

            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Archery, 35.0, 60.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.Wrestling, 15.0, 37.5);

            SetResistance(ResistanceType.Physical, 45, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 45);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            Fame = 500;
            Karma = -500;

            SetWearable(new ThighBoots(), Utility.RandomNeutralHue(), dropChance: 1);
            SetWearable(new FemaleStuddedChest(), dropChance: 1);
            SetWearable(new OrcHelm(), dropChance: 1);

            SetWearable((Item)Activator.CreateInstance(Utility.RandomList(_WeaponsList)), dropChance: 1);

            Utility.AssignRandomHair(this);

            PackItem(new Arrow(Utility.Random(40)));
        }

        public DarkArcher(Serial serial)
            : base(serial)
        {
        }

        public override bool ClickTitle => false;
        public override bool AlwaysMurderer => true;

        public override bool ShowFameTitle => false;

        private static readonly Type[] _WeaponsList =
        {
            typeof(Bow), typeof(CompositeBow)
        };

        //public override void GenerateLoot()
        //{
        //    AddLoot(LootPack.Average);
        //}

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
