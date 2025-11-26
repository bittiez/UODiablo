using Server.Engines.CannedEvil;
using Server.Items;
using Server.Mobiles;
using System;
using System.Linq;
using VitaNex.FX;

namespace Server.Custom.Mobiles.Champs
{
    public class Andarial : BaseChampion
    {
        [Constructable]
        public Andarial() : base(AIType.AI_Melee)
        {
            Body = 174;
            Name = "Andarial";
            BaseSoundID = 0x4B0;
            MonsterLevel = 12;

            SetStr(100, 200);
            SetDex(100, 150);
            SetInt(51, 100);

            SetHits(2000);
            SetStam(203, 650);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 30, 50);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 30, 50);

            SetSkill(SkillName.Anatomy, 60, 100.0);
            SetSkill(SkillName.MagicResist, 60, 100);
            SetSkill(SkillName.Tactics, 60, 100.0);
            SetSkill(SkillName.Wrestling, 60, 100);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 80;

            ForceActiveSpeed = 0.4;
            ForcePassiveSpeed = 0.6;
        }

        public Andarial(Serial serial) : base(serial)
        {
        }

        public override bool Murderer => true;

        public override bool AlwaysMurderer => true;

        public override Poison PoisonImmune => Poison.Greater;

        public override ChampionSkullType SkullType => ChampionSkullType.Venom;

        public override Type[] UniqueList => new Type[] { };

        public override Type[] SharedList => new Type[] { };

        public override Type[] DecorativeList => new Type[] { };

        private DateTime nextPoisonNova = DateTime.MinValue;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.Now > nextPoisonNova)
            {
                if (Combatant.GetDistance(this) < 15)
                {
                    ExplodeFX.Poison.CreateInstance(Location, Map, 10, effectHandler: (e) =>
                    {
                        foreach (Mobile m in
                            e.Source.Location.GetMobilesInRange(e.Map, 0)
                                .Where(m => m != null && !m.Deleted && m.CanBeHarmful(m, false, true)))
                        {
                            if (m == this || !CanBeHarmful(m))
                                continue;

                            m.ApplyPoison(this, Poison.Regular);
                        }
                    }).Send();
                    nextPoisonNova = DateTime.Now + TimeSpan.FromSeconds(20);
                }
            }
        }

        public override void OnDeath(Container c)
        {
            ExplodeFX.Poison.CreateInstance(Location, Map, 10, 1).Send();
            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.UOD_AllRunesForBosses);
        }

        public override MonsterStatuetteType[] StatueTypes => new MonsterStatuetteType[] { };

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }
    }
}
