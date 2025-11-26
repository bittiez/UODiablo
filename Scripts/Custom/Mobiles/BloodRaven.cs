using Server.Items;
using Server.Mobiles;
using Server.Spells;
using System;
using VitaNex.FX;

namespace Server.Custom.Mobiles
{
    internal class BloodRaven : BaseCreature
    {
        private readonly static TimeSpan ZombieSpawnFrequency = TimeSpan.FromSeconds(5);

        private DateTime lastZombieSpawn = DateTime.Now;

        public BloodRaven(Serial serial) : base(serial)
        {
        }

        [Constructable]
        public BloodRaven() : base(AIType.AI_Archer, FightMode.Closest, 10, 8, 0.1, 0.4)
        {
            Name = "blood raven";
            Body = 0x191;
            Hue = 0;
            Female = true;

            MonsterLevelNormal = 10;
            MonsterLevelNightmare = 43;
            MonsterLevelHell = 88;
            
            SetStr(85, 95);
            SetDex(65, 120);
            SetInt(15, 30);

            SetHits(240, 300);

            SetDamage(4, 13);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.Anatomy, 35.1, 80.0);
            SetSkill(SkillName.Archery, 55.1, 80.0);

            Fame = 800;
            Karma = -800;

            VirtualArmor = 15;

            AddItem(new Bow());

            AddItem(new OrcHelm());
            AddItem(new FemaleStuddedChest() { Hue = 1209 });
            AddItem(new WoodlandBelt() { Hue = 1209 });
            AddItem(new ThighBoots() { Hue = 1209 });

            HairItemID = 0x203C;
            HairHue = 2213;

            PackItem(new Arrow(200));
        }

        public override bool AlwaysMurderer => true;

        public override void OnThink()
        {
            base.OnThink();

            Mobile combatant = Combatant as Mobile;

            if (combatant != null && Alive && combatant.GetDistance(this) < 20)
            {
                if (lastZombieSpawn + ZombieSpawnFrequency < DateTime.Now)
                {
                    BaseCreature zombie = new Zombie();
                    Point3D p = new Point3D(this);

                    for (int i = 10; i > 0; i--)
                        if (SpellHelper.FindValidSpawnLocation(Map, ref p, false))
                            break;

                    BaseCreature.Summon(zombie, true, this, p, 0x216, TimeSpan.FromSeconds(45));
                    zombie.FixedParticles(0x3728, 8, 20, 5042, EffectLayer.Head);
                    zombie.ControlOrder = OrderType.Guard;

                    lastZombieSpawn = DateTime.Now;
                }
            }
        }

        public override void OnDeath(Container c)
        {
            //ExplodeFX.Air.CreateInstance(this, Map, 15, 3).Send();
            new AirExplodeEffect(this, Map, 15, 1).Send();

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.UOD_AllRunesForBosses);
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
