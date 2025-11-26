using System;

namespace Server.Custom.Items
{
    internal class HealthGlobe : Item
    {
        [Constructable]
        public HealthGlobe()
        {
            Name = "health globe";
            Movable = false;
            ItemID = 0x0E26;
            Hue = 32;

            Bittiez.Tools.Start_Timer_Delayed_Call(TimeSpan.FromSeconds(5), () => { Delete(); });
        }

        public HealthGlobe(Serial serial) : base(serial)
        {
        }

        public static void DropGlobe(Point3D loc, Map map)
        {
            Item g = new HealthGlobe();
            World.AddItem(g);
            g.MoveToWorld(loc, map);
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m != null && m.Player && m.Hits < m.HitsMax)
            {
                m.Heal((int)(m.HitsMax * 0.10));
                m.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);
                Delete();
            }

            return true;
        }

        public override void Serialize(GenericWriter writer)
        {
            //Don't save
        }

        public override void Deserialize(GenericReader reader)
        {
        }
    }
}
