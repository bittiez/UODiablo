using Server.Items;
using System;

namespace Server.Mobiles
{
    public class DarkHunter : BaseCreature
    {
        [Constructable]
        public DarkHunter()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "dark hunter";
            Body = 0x191;

            MonsterLevel = 2;

            SetStr(60, 75);
            SetDex(80, 95);
            SetInt(60, 75);

            SetDamage(6, 10);

            SetSkill(SkillName.Fencing, 35.0, 60.5);
            SetSkill(SkillName.Macing, 35.0, 60.5);
            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Swords, 35.0, 60.5);
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
            SetWearable(new Buckler(), dropChance: 1);

            SetWearable((Item)Activator.CreateInstance(Utility.RandomList(_WeaponsList)), dropChance: 1);

            Utility.AssignRandomHair(this);
        }

        public DarkHunter(Serial serial)
            : base(serial)
        {
        }

        public override bool ClickTitle => false;
        public override bool AlwaysMurderer => true;

        public override bool ShowFameTitle => false;

        private static readonly Type[] _WeaponsList =
        {
            typeof(Cutlass), typeof(Mace), typeof(Kryss)
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
