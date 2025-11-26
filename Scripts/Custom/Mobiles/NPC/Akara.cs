using Server.Items;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class Akara : BaseVendor
    {
        private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

        public override bool ConvertsMageArmor { get { return true; } }

        [Constructable]
        public Akara()
            : base("")
        {
            Name = "Akara";

            Female = true;
            Body = 0x191;

            for (int i = 0; i < 54; i++) //Give akara all skills, so she can be a generic trainer.
            {
                if (i == 50)
                    continue;
                SetSkill((SkillName)i, 60.1, 100);
            }
        }

        public Akara(Serial serial)
            : base(serial)
        {
        }

        public override bool CanTeach => true;
        public override bool IsActiveVendor => true;

        public override NpcGuild NpcGuild
        {
            get
            {
                return NpcGuild.MagesGuild;
            }
        }
        public override VendorShoeType ShoeType
        {
            get
            {
                return Utility.RandomBool() ? VendorShoeType.Shoes : VendorShoeType.Sandals;
            }
        }

        protected override List<SBInfo> SBInfos => m_SBInfos;

        public override void InitSBInfo()
        {
            m_SBInfos.Add(new SBAlchemist(this));
            m_SBInfos.Add(new SBMage());
        }

        public override void InitOutfit()
        {
            AddItem(new CommemorativeRobe() { Hue = 15 });
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
