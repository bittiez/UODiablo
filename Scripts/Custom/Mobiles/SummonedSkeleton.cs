using Server.Commands;
using Server.Mobiles;
using Server.Spells;
using System;

namespace Server.Custom.Mobiles
{
    public class SummonedSkeleton : BaseCreature
    {
        public static void Initialize()
        {
            CommandSystem.Register("tskele", AccessLevel.Seer, new CommandEventHandler(On_Command));
        }

        private static void On_Command(CommandEventArgs e)
        {
            if (e.Mobile != null && e.Mobile is PlayerMobile player)
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
                    e.Mobile.SendMessage("You have too many skeletons summoned.");
                    return;
                }
            }

            TimeSpan duration = TimeSpan.FromSeconds((2 * e.Mobile.Skills.Necromancy.Fixed) / 2);

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

            SpellHelper.Summon(skele, e.Mobile, 0x216, duration, false, false);
            skele.FixedParticles(0x3728, 8, 20, 5042, EffectLayer.Head);
        }

        public SummonedSkeleton() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "summoned skeleton";

            Body = Utility.RandomList(50, 56);
            BaseSoundID = 0x48D;

            SetStr(56, 80);
            SetDex(56, 75);
            SetInt(16, 40);

            SetHits(34, 48);

            SetDamage(3, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 25, 40);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 5, 15);

            SetSkill(SkillName.MagicResist, 45.1, 60.0);
            SetSkill(SkillName.Tactics, 45.1, 60.0);
            SetSkill(SkillName.Wrestling, 45.1, 55.0);

            Fame = 450;
            Karma = -450;

            ControlSlots = 0;
            FollowRange = 2;
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lesser;
        public override TribeType Tribe => TribeType.Undead;

        public override void GenerateLoot()
        {
        }

        public SummonedSkeleton(Serial serial) : base(serial)
        {
        }

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
