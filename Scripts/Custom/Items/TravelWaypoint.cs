using Server.Custom.Gump;
using Server.Items;
using Server.Mobiles;

namespace Server.Custom.Items
{
    internal class TravelWaypoint : BaseAddon
    {
        public const int XWIDTH = 6;
        public const int YWIDTH = 7;

        private Rectangle2D bounds { get { return new Rectangle2D(X, Y, XWIDTH - 1, YWIDTH - 1); } }

        [Constructable]
        public TravelWaypoint()
        {
            Name = "";
            int wp = 0x2B15;
            AddComponent(new AddonComponent(wp + 5), 3, 0, 0);
            AddComponent(new AddonComponent(wp + 10), 4, 0, 0);

            AddComponent(new AddonComponent(wp + 1), 1, 1, 0);
            AddComponent(new AddonComponent(wp + 4), 2, 1, 0);
            AddComponent(new AddonComponent(wp + 9), 3, 1, 0);
            AddComponent(new AddonComponent(wp + 15), 4, 1, 0);
            AddComponent(new AddonComponent(wp + 20), 5, 1, 0);

            AddComponent(new AddonComponent(wp), 0, 2, 0);
            AddComponent(new AddonComponent(wp + 3), 1, 2, 0);
            AddComponent(new AddonComponent(wp + 8), 2, 2, 0);
            AddComponent(new AddonComponent(wp + 14), 3, 2, 0);
            AddComponent(new AddonComponent(wp + 19), 4, 2, 0);
            AddComponent(new AddonComponent(wp + 25), 5, 2, 0);

            AddComponent(new AddonComponent(wp + 2), 0, 3, 0);
            AddComponent(new AddonComponent(wp + 7), 1, 3, 0);
            AddComponent(new AddonComponent(wp + 13), 2, 3, 0);
            AddComponent(new AddonComponent(wp + 18), 3, 3, 0);
            AddComponent(new AddonComponent(wp + 24), 4, 3, 0);
            AddComponent(new AddonComponent(wp + 29), 5, 3, 0);

            AddComponent(new AddonComponent(wp + 6), 0, 4, 0);
            AddComponent(new AddonComponent(wp + 12), 1, 4, 0);
            AddComponent(new AddonComponent(wp + 17), 2, 4, 0);
            AddComponent(new AddonComponent(wp + 23), 3, 4, 0);
            AddComponent(new AddonComponent(wp + 28), 4, 4, 0);
            AddComponent(new AddonComponent(wp + 31), 5, 4, 0);

            AddComponent(new AddonComponent(wp + 11), 0, 5, 0);
            AddComponent(new AddonComponent(wp + 16), 1, 5, 0);
            AddComponent(new AddonComponent(wp + 22), 2, 5, 0);
            AddComponent(new AddonComponent(wp + 27), 3, 5, 0);
            AddComponent(new AddonComponent(wp + 30), 4, 5, 0);

            AddComponent(new AddonComponent(wp + 21), 1, 6, 0);
            AddComponent(new AddonComponent(wp + 26), 2, 6, 0);

            foreach (AddonComponent addonComponent in Components)
            {
                addonComponent.Name = Name;
                addonComponent.Addon = this;
            }
        }

        //public override bool HandlesOnMovement => true;

        [CommandProperty(AccessLevel.GameMaster)]
        public int WayPoint { get; set; } = -1;

        public override bool HandlesOnMovement => true;

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (bounds.Contains(m.Location))
            {
                HandleMovement(m);
            }
            else
            {
                if (m.HasGump(typeof(WaypointGump)) || m.HasGump(typeof(WaypointGumpDifficulty)))
                {
                    m.CloseGump(typeof(WaypointGump));
                    m.CloseGump(typeof(WaypointGumpDifficulty));
                }
            }
            base.OnMovement(m, oldLocation);
        }

        private void HandleMovement(Mobile m)
        {
            if (m.Player)
            {
                if (WayPoint > -1)
                {
                    WayPointUtility.AddWaypointAccess(WayPoint, m as PlayerMobile);
                }

                if (!m.HasGump(typeof(WaypointGump)) && !m.HasGump(typeof(WaypointGumpDifficulty)))
                {
                    new WaypointGump(m as PlayerMobile).SendTo(m.NetState);
                }
            }
        }

        public TravelWaypoint(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1);

            //1
            writer.Write(WayPoint);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    WayPoint = reader.ReadInt();
                    break;
            }
        }
    }
}
