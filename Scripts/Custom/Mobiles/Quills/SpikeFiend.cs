using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    internal class SpikeFiend : BaseCreature
    {
        [Constructable]
        public SpikeFiend() : base(AIType.AI_Archer, FightMode.Closest, 10, 8)
        {
            Name = "spike fiend";
            Body = 0x20;
            Hue = 50;

            MonsterLevelNormal = 5;
            MonsterLevelNightmare = 38;
            MonsterLevelHell = 68;
            
            SetStr(20, 50);
            SetDex(50, 75);
            SetInt(10, 30);

            SetDamage(1, 5);

            SetSkill(SkillName.Archery, 46.0, 67.5);

            SetResistance(ResistanceType.Physical, 45, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 45);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 30, 40);

            Fame = 300;
            Karma = -300;

            AddItem(new Bow());
        }

        public SpikeFiend(Serial serial) : base(serial)
        {
        }

        //public override void GenerateLoot()
        //{
        //    AddLoot(LootPack.Poor);
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
