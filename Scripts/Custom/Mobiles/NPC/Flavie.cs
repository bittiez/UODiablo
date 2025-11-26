using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Mobiles
{
    public class Flavie : Mobile
    {
        [Constructable]
        public Flavie()
            : base()
        {
            Name = "Flavie";

            Female = true;
            Body = 0x191;
            HairItemID = 0x203C;
            HairHue = 542;

            AddItem(new StuddedChest());
            AddItem(new StuddedArms());
            AddItem(new StuddedGloves());
            AddItem(new StuddedGorget());
            AddItem(new StuddedLegs());
            AddItem(new Boots());

            AddItem(new Bow());

            Skills[SkillName.Anatomy].Base = 120.0;
            Skills[SkillName.Tactics].Base = 120.0;
            Skills[SkillName.Archery].Base = 120.0;
            Skills[SkillName.MagicResist].Base = 120.0;
            Skills[SkillName.DetectHidden].Base = 100.0;

            Timer.DelayCall(TimeSpan.FromSeconds(30), SayMessage);
        }

        public Flavie(Serial serial)
            : base(serial)
        {
            Timer.DelayCall(TimeSpan.FromSeconds(30), SayMessage);
        }

        private void SayMessage()
        {
            Say("Turn back! I can tell that you need more experience to fight safely in the next wilderness.");
            Timer.DelayCall(TimeSpan.FromSeconds(30), SayMessage);
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
