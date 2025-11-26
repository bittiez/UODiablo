using Server.Items;

namespace Server.Custom.Items
{
    internal class ManaPotion : BasePotion
    {
        [Constructable]
        public ManaPotion() : base(0xF0C, PotionEffect.Mana)
        {
            Name = "mana potion";
            ItemID = 0xF0C;
            Weight = 0.2;
            Hue = 100;
            Stackable = true;
        }

        public ManaPotion(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

        public override void Drink(Mobile from)
        {
            from.Mana += Utility.Random(7, 14);

            from.RevealingAction();
            from.PlaySound(0x2D6);
            from.AddToBackpack(new Bottle());

            if (from.Body.IsHuman && !from.Mounted)
            {
                if (Core.SA)
                {
                    from.Animate(AnimationType.Eat, 0);
                }
                else
                {
                    from.Animate(34, 5, 1, true, false, 0);
                }
            }

            Consume();
        }
    }
}
