using Server.Items;
using Server.Mobiles;
using VitaNex.FX;

namespace Server.Custom.Mobiles
{
    internal class Griswold : BaseCreature
    {
        [Constructable]
        public Griswold() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.3, 0.5)
        {
            Name = "griswold";
            Body = 994;

            MonsterLevelNormal = 5;
            MonsterLevelNightmare = 39;
            MonsterLevelHell = 84;
            
            SetStr(65, 85);
            SetDex(50, 80);
            SetInt(20, 30);

            SetHits(500);
            SetStam(85, 100);

            SetDamage(2, 6);

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Fire, 50);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.MagicResist, 30, 50);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 60, 80);

            Fame = 800;
            Karma = -800;

            VirtualArmor = 20;
        }

        public Griswold(Serial serial) : base(serial)
        {
        }

        public override bool AlwaysMurderer => true;

        public override void OnDeath(Container c)
        {
            ExplodeFX.Earth.CreateInstance(this, Map, 3, 1).Send();
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UOD_AllRunesForBosses);
            base.GenerateLoot();
        }

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
