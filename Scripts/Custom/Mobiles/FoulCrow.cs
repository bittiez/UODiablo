// **********
// ServUO - FoulCrow.cs
// **********

using Server.Mobiles;

namespace Server.Custom.Mobiles
{
    [CorpseName("foul crow corpse")]
    public class FoulCrow : BaseCreature
    {
        [Constructable]
        public FoulCrow()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "foul crow";
            Body = 5;
            BaseSoundID = 0x2EE;

            MonsterLevel = 4;

            SetStr(16, 30);
            SetDex(26, 38);
            SetInt(6, 14);

            SetHits(4, 6);
            SetMana(0);

            SetDamage(1, 2);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);

            SetSkill(SkillName.MagicResist, 5.1, 14.0);
            SetSkill(SkillName.Tactics, 5.1, 10.0);
            SetSkill(SkillName.Wrestling, 5.1, 10.0);

            Fame = 150;
            Karma = -150;

            VirtualArmor = 10;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;
        }

        public FoulCrow(Serial serial)
            : base(serial)
        {
        }

        public override int Meat => 1;

        public override FoodType FavoriteFood => FoodType.Meat;

        public override bool CanFly => true;

        public override int Feathers => 10;

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
