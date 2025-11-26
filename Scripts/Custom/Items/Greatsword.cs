namespace Server.Items
{
    [Flipable(0x26CE, 0x26CF)]
    public class Greatsword : BaseSword
    {
        [Constructable]
        public Greatsword()
            : base(0x26CE)
        {
            Weight = 6.0;
            Layer = Layer.TwoHanded;
            Name = "great sword";

        }

        public Greatsword(Serial serial)
            : base(serial)
        {
        }

        public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
        public override WeaponAbility SecondaryAbility => WeaponAbility.Disarm;
        public override int AosStrengthReq => 85;
        public override int AosMinDamage => 20;
        public override int AosMaxDamage => 24;
        public override float MlSpeed => 4.5f;
        public override int InitMinHits => 36;
        public override int InitMaxHits => 48;

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
