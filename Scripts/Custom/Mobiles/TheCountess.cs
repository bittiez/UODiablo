using Server.Items;
using System;

namespace Server.Mobiles
{
    public class TheCountess : BaseCreature
    {
        [Constructable]
        public TheCountess()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "the countess";
            Body = 0x191;

            MonsterLevel = 11;

            SetStr(100, 150);
            SetDex(80, 95);
            SetInt(60, 75);

            SetHits(200, 300);

            SetDamage(10, 23);

            SetSkill(SkillName.Fencing, 66.0, 97.5);
            SetSkill(SkillName.Macing, 65.0, 87.5);
            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Swords, 65.0, 87.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.Wrestling, 15.0, 37.5);

            SetResistance(ResistanceType.Physical, 45, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 45);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            Fame = 1000;
            Karma = -1000;

            SetWearable(new ThighBoots(), Utility.RandomNeutralHue(), dropChance: 1);
            SetWearable(new FemaleStuddedChest(), dropChance: 1);
            SetWearable(new OrcHelm(), dropChance: 1);
            SetWearable(new ChaosShield(), dropChance: 1);

            SetWearable((Item)Activator.CreateInstance(Utility.RandomList(_WeaponsList)), dropChance: 1);
        }

        public TheCountess(Serial serial)
            : base(serial)
        {
        }

        public override bool ClickTitle => false;
        public override bool AlwaysMurderer => true;

        public override bool ShowFameTitle => false;

        private static readonly Type[] _WeaponsList =
        {
            typeof(Scimitar)
        };

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.UOD_AllRunesForBosses);
        }

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
