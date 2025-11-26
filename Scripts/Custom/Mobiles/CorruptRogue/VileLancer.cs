using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class VileLancer : BaseCreature
    {
        [Constructable]
        public VileLancer()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "vile lancer";
            Body = 0x191;
            Female = true;
            Hue = 50;

            MonsterLevel = 5;

            SetStr(60, 75);
            SetDex(80, 95);
            SetInt(60, 75);

            SetDamage(3, 6);
            
            SetSkill(SkillName.Fencing, 35.0, 60.5);
            SetSkill(SkillName.MagicResist, 25.0, 47.5);
            SetSkill(SkillName.Tactics, 65.0, 87.5);
            SetSkill(SkillName.Wrestling, 15.0, 37.5);

            SetResistance(ResistanceType.Physical, 0, 50);
            SetResistance(ResistanceType.Fire, 0, 30);
            SetResistance(ResistanceType.Cold, 0, 45);
            SetResistance(ResistanceType.Poison, 0, 30);
            SetResistance(ResistanceType.Energy, 0, 40);
            
            SetDamageType(ResistanceType.Poison, 0, 20);

            Fame = 500;
            Karma = -500;

            if(RandomImpl.NextDouble() < 0.33)
                SetWearable(new ThighBoots(), Utility.RandomNeutralHue(), dropChance: 0.3);
            
            if(RandomImpl.NextDouble() < 0.33)
                SetWearable(new FemaleStuddedChest(), dropChance: 0.3);
            
            if(RandomImpl.NextDouble() < 0.33)
                SetWearable(new OrcHelm(), dropChance: 1);

            SetWearable((Item)Activator.CreateInstance(Utility.RandomList(_WeaponsList)), dropChance: 1);

            Utility.AssignRandomHair(this);
        }

        public VileLancer(Serial serial)
            : base(serial)
        {
        }

        public override bool ClickTitle => false;
        public override bool AlwaysMurderer => true;

        public override bool ShowFameTitle => false;

        private static readonly Type[] _WeaponsList =
        {
            typeof(Spear), typeof(Pike), typeof(ShortSpear)
        };

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
