using Server;
using Server.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Bittiez
{
    public class LootBag : Bag
    {
        public ArrayList items;

        [Constructable]
        public LootBag()
        {
            this.Name = "a loot bag";
            this.Weight = 1;
            this.Hue = 798;
            this.LootType = LootType.Blessed;
            items = new ArrayList();
            //Add default loot types
            items.Add(typeof(Gold));
            items.Add(typeof(BaseReagent));
        }

        public override bool OnDragDrop(Mobile from, Item dropped)
        {
            return false;
        }
        public override bool OnDragDropInto(Mobile from, Item item, Point3D p)
        {
            return false;
        }

        public LootBag(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version 

            writer.Write(items.Count);
            foreach(Type t in items)
            {
                writer.Write(t.FullName);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version >= 1)
            {
                int count = reader.ReadInt();
                if (count > 0)
                    items = new ArrayList();

                for (int i = 0; i < count; i++)
                {
                    string s = reader.ReadString();
                    try
                    {
                        Type type = Type.GetType(s);
                        if (type != null)
                            items.Add(type);
                    }
                    catch { }
                }
            }

        }
    }
}

