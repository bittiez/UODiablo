using System;
using Server.Items;
using Server.Network;
using Server.Engines.Harvest;

namespace Server.Items
{
    public class ReagentTool : Dagger, IHarvestTool
    {
        public HarvestSystem HarvestSystem => ReagentGathering.System;

        [Constructable]
        public ReagentTool() : base()
        {
            Name = "reagent gathering dagger";
            Hue = 64;
            Weight = 1.0;
            UsesRemaining = Utility.Random(100);
            ShowUsesRemaining = true;
        }

        public ReagentTool(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (HarvestSystem == null || Deleted)
                return;

            Point3D loc = GetWorldLocation();

            if (!from.InLOS(loc) || !from.InRange(loc, 2))
            {
                from.LocalOverheadMessage(Server.Network.MessageType.Regular, 0x3E9, 1019045); // I can't reach that
                return;
            }
            else if (!IsAccessibleTo(from))
            {
                PublicOverheadMessage(Server.Network.MessageType.Regular, 0x3E9, 1061637); // You are not allowed to access 
                return;
            }

            from.SendLocalizedMessage(1010018); // What do you want to use this item on?

            HarvestSystem.BeginHarvesting(from, this);
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
            ShowUsesRemaining = true;
        }
    }
}
