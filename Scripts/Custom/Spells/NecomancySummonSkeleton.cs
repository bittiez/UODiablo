using Server.Custom.Mobiles;
using Server.Mobiles;
using System;

namespace Server.Spells.Necromancy
{
    public class NecomancySummonSkeleton : NecromancerSpell
    {
        public NecomancySummonSkeleton(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        private static readonly SpellInfo m_Info = new SpellInfo(
            "Summon Skeleton", "Voco Ossa",
            203,
            9031,
            Reagent.GraveDust,
            Reagent.DaemonBone);

        public override double RequiredSkill => 10.0;

        public override int RequiredMana => 34;

        public override TimeSpan CastDelayBase => TimeSpan.FromSeconds(2);

        public override void OnCast()
        {
            if (Caster != null && Caster is PlayerMobile player)
            {
                int summonedSkeleCount = 0;
                for (int i = 0; i < player.AllFollowers.Count; i++)
                {
                    Mobile m = player.AllFollowers[i];
                    if (m != null)
                    {
                        if (m is SummonedSkeleton || m is SummonedSkeletonMage)
                        {
                            summonedSkeleCount++;
                        }
                    }
                }

                if (summonedSkeleCount > 20)
                {
                    Caster.SendMessage("You have too many skeletons summoned.");
                    return;
                }
            }

            TimeSpan duration = TimeSpan.FromSeconds((2 * Caster.Skills.Necromancy.Fixed) / 2);

            BaseCreature skele = null;
            switch (Utility.RandomBool())
            {
                case true:
                    skele = new SummonedSkeleton();
                    break;
                case false:
                    skele = new SummonedSkeletonMage();
                    break;
            }

            SpellHelper.Summon(skele, Caster, 0x216, duration, false, false);
            skele.FixedParticles(0x3728, 8, 20, 5042, EffectLayer.Head);
        }
    }
}

namespace Server.Items
{
    public class SummonSkeletonScroll : SpellScroll
    {
        [Constructable]
        public SummonSkeletonScroll()
            : this(1)
        {
        }

        [Constructable]
        public SummonSkeletonScroll(int amount)
            : base(117, 0x1F35, amount)
        {
            Name = "summon skeleton";
        }

        public SummonSkeletonScroll(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}
