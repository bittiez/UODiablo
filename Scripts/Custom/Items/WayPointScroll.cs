using Server.Custom.Gump;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Items
{
    internal class WayPointScroll : Item, ICommodity
    {
        [Constructable]
        public WayPointScroll()
        {
            Name = "waypoint scroll";
            ItemID = 0x1F35;
            Stackable = true;
        }

        public WayPointScroll(Serial serial) : base(serial)
        {
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add("destroyed on use");
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            if (AccessCheck(from))
            {
                new WaypointGump(from as PlayerMobile).SendTo(from.NetState);
                Consume(1);
            }
        }

        private bool AccessCheck(Mobile from)
        {
            if (from == null || !(from is PlayerMobile) || !from.Alive || Parent != from.Backpack)
            {
                from.SendMessage("That must be in your backpack to use.");
                return false;
            }

            return true;
        }

        public TextDefinition Description => "waypoint scroll";

        public bool IsDeedable => true;

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
