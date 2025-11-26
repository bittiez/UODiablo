using System;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
    internal class CarverShaman : BaseCreature
    {
        private DateTime lastMinionSpawn { get; set; } = DateTime.MinValue;
        private static TimeSpan minionSpawnFrequency { get; set; } = TimeSpan.FromSeconds(15);

        [Constructable]
        public CarverShaman() : base(AIType.AI_Mage, FightMode.Closest, 10, 8, 0.2, 0.4)
        {
            Name = "carver shaman";
            Body = 253;
            Hue = 1103;

            MonsterLevelNormal = 6;
            MonsterLevelNightmare = 38;
            MonsterLevelHell = 69;
            
            SetStr(60, 100);
            SetDex(60, 95);
            SetInt(65, 170);

            SetHits(103, 180);

            SetDamage(18, 22);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 40, 60);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 55, 65);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.SpiritSpeak, 40.0, 59.0);

            SetSkill(SkillName.EvalInt, 65.0);
            SetSkill(SkillName.Magery, 30.1, 45.0);
            SetSkill(SkillName.Meditation, 25.1, 75.0);
            SetSkill(SkillName.MagicResist, 20.1, 50.0);
            SetSkill(SkillName.Tactics, 30.1, 60.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 20;

            FollowersMax = 3;

            switch (Utility.Random(35))
            {
                case 0: PackItem(new LichFormScroll()); break;
                case 1: PackItem(new PoisonStrikeScroll()); break;
                case 2: PackItem(new StrangleScroll()); break;
                case 3: PackItem(new VengefulSpiritScroll()); break;
                case 4: PackItem(new WitherScroll()); break;
            }


            PackItem(new GnarledStaff());
            PackNecroReg(17, 24);
        }

        public CarverShaman(Serial serial) : base(serial)
        {
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant is Mobile combatant && Alive && combatant.GetDistance(this) < 20)
            {
                if (lastMinionSpawn + minionSpawnFrequency < DateTime.Now)
                {
                    Point3D p = new Point3D(this);

                    SpellHelper.FindValidSpawnLocation(Map, ref p, false);

                    BaseCreature minion;

                    switch (RandomImpl.Next(1))
                    {
                        case 0:
                            minion = new Fallen();
                            break;
                        case 1:
                            minion = new Carver();
                            break;
                        default:
                            minion = new Fallen();
                            break;
                    }

                    BaseCreature.Summon(minion, true, this, p, 0x216, TimeSpan.FromSeconds(120));
                    minion.FixedParticles(0x3728, 8, 20, 5042, EffectLayer.Head);
                    minion.ControlOrder = OrderType.Guard;

                    lastMinionSpawn = DateTime.Now;
                }
            }
        }

        public override TribeType Tribe { get { return TribeType.Undead; } }

        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

        public override bool CanRummageCorpses => true;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            AddLoot(LootPack.LowScrolls, 2);
        }

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
