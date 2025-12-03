using Server;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Bittiez
{
    public class Loot
    {
        public static void Initialize()
        {
            CommandSystem.Register("Loot", AccessLevel.Player, new CommandEventHandler(loot_OnCommand));
        }

        [Usage("Loot")]
        [Description("Loots a corpse")]
        private static void loot_OnCommand(CommandEventArgs e)
        {
            Container backpack = e.Mobile.Backpack;
            List<Item> backpackItems = Tools.List_Items_In_Container((BaseContainer)backpack, true);
            LootBag lootBag = null;

            foreach (Item bi in backpackItems)
            {
                if (bi is LootBag)
                {
                    lootBag = (LootBag)bi;
                    break;
                }
            }

            if (lootBag == null)
            {
                lootBag = new LootBag();
                e.Mobile.AddToBackpack(lootBag);
            }

            if (e.Mobile.HasGump(typeof(LootGump)))
            {
                e.Mobile.CloseGump(typeof(LootGump));
                e.Mobile.SendGump(new LootGump(e.Mobile, lootBag));
            }

            if (e.Arguments.Length > 0)
            {
                if (e.Arguments[0].ToLower() == "edit")
                {
                    e.Mobile.SendMessage("Select an item to add to your loot list.");
                    e.Mobile.Target = new LootEditTarget(lootBag);
                    e.Mobile.SendGump(new LootGump(e.Mobile, lootBag));
                } else if (e.Arguments[0].ToLower() == "export")
                {
                    LootExport le = new LootExport(e.Mobile, lootBag.items);
                    e.Mobile.AddToBackpack(le);
                    e.Mobile.SendMessage(15, "Exported loot list to your backpack.");
                }                 
            }
            else
            {
                e.Mobile.SendMessage("Select a corpse to loot.");
                e.Mobile.Target = new LootTarget(lootBag);
            }
        }
    }

    public class LootExport : Item
    {
        private Mobile FROM;
        private ArrayList itemTypes;

        public LootExport(Mobile From, ArrayList itemTypes)
            : base(0x14F0)
        {
            this.itemTypes = itemTypes;
            this.FROM = From;

            Name = "Loot List Export";
        }
        public LootExport(Serial serial) { }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);
            list.Add(string.Format("Loot list export from {0} containing {1} item(s).<br> Double click to add to your loot list.", this.FROM.Name, this.itemTypes.Count));
        }

        public override void OnDoubleClick(Mobile from)
        {
            LootBag lootBag = null;
            List<Item> backpackItems = Tools.List_Items_In_Container((BaseContainer)from.Backpack, true);
            foreach (Item bi in backpackItems)
            {
                if (bi is LootBag)
                {
                    lootBag = (LootBag)bi;
                    break;
                }
            }

            if (lootBag == null)
            {
                lootBag = new LootBag();
                from.AddToBackpack(lootBag);
            }

            foreach(Type t in this.itemTypes)
            {
                if (!lootBag.items.Contains(t))
                {
                    lootBag.items.Add(t);
                }
            }

            from.SendMessage(15, string.Format("You combined {0} items with your loot list.", itemTypes.Count));

        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0);

            //v 0
            writer.Write(this.FROM);
            writer.Write(itemTypes.Count);
            foreach (Type t in itemTypes)
            {
                writer.Write(t.FullName);
            }

        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int v = reader.ReadInt();

            if (v >= 0)
            {
                this.FROM = reader.ReadMobile();
                int count = reader.ReadInt();
                this.itemTypes = new ArrayList();

                for (int i = 0; i < count; i++)
                {
                    string s = reader.ReadString();
                    try
                    {
                        Type type = Type.GetType(s);
                        if (type != null)
                            this.itemTypes.Add(type);
                    }
                    catch { }
                }

            }
        }
    }
    
    public class LootEditTarget : Target
    {
        public LootBag lootBag;
        public LootEditTarget(LootBag lootBag)
            : base(20, false, TargetFlags.None)
        {
            this.lootBag = lootBag;
        }

        protected override void OnTarget(Mobile m, object targeted)
        {
            if (targeted is Item)
            {
                Item targ = (Item)targeted;
                bool addIt = true;
                foreach (Type t in lootBag.items)
                {
                    if (targ.GetType() == t || targ.GetType().IsSubclassOf(t))
                    {
                        m.SendMessage("That is already on your loot list!");
                        addIt = false;
                        break;
                    }
                }

                if(addIt)
                    lootBag.items.Add(targ.GetType());
                m.SendMessage("Added " + targ.GetType().Name + ".");
                m.SendMessage("Select an item to add to your loot list.");
                m.Target = new LootEditTarget(lootBag);
            }
            else
            {
                m.SendMessage("You can only loot items!");
            }
        }
    }

    public class LootTarget : Target
    {
        public LootBag lootBag;
        public LootTarget(LootBag lootBag)
            : base(4, false, TargetFlags.None)
        {
            this.lootBag = lootBag;
        }

        protected override void OnTarget(Mobile m, object targeted)
        {
            if (targeted is LootBag)
            {
                //m.SendMessage("Configuration not implemented yet, sorry!");
                return;
            }

            if (targeted is Corpse)
            {
                Corpse body = (Corpse)targeted;

                if (!body.IsAccessibleTo(m))
                {
                    m.SendMessage("You can't access that corpse!");
                    return;
                }

                if (body.Owner is PlayerMobile)
                {
                    m.SendMessage("You can't loot player corpses with this command!");
                    return;
                }
                if (body.Owner is BaseCreature)
                {
                    if (((BaseCreature)body.Owner).IsBonded)
                    {
                        m.SendMessage("You can't loot from a bonded pet's corpse.");
                    }
                }

                //Clear to loot
                List<Item> items = body.Items;

                List<Item> moveMe = new List<Item>();
                foreach (Item bi in items)
                {
                    foreach (Type lt in lootBag.items)
                    {
                        if (bi.GetType() == lt || lt.IsSubclassOf(bi.GetType()))
                        {
                            moveMe.Add(bi);
                        }
                    }
                }

                foreach (Item mi in moveMe)
                    lootBag.TryDropItem(m, mi, false);
                body.Delete();

            }
        }
    }

    public class LootGump : Gump
    {
        private Mobile caller;
        private LootBag lootBag;

        public LootGump(Mobile mobile, LootBag lootBag)
            : base(50, 50)
        {
            caller = mobile;

            if (caller.HasGump(typeof(LootGump)))
                caller.CloseGump(typeof(LootGump));

            this.lootBag = lootBag;

            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;
            this.Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 300, 426, 3500);
            AddPage(1);


            int page = 0, cpage = 1;
            int max = 18;
            AddLabel(115, 15, 15, "Loot List");

            ArrayList lootTypes = lootBag.items;

            int itemNum = 154;
            foreach (Type t in lootTypes)
            {
                if (t == null)
                    continue;

                AddLabel(35, 23 + (20 * page), 10, string.Format("{0}", t.Name));
                AddButton(15, 23 + (20 * page), 56, 56, itemNum, GumpButtonType.Reply, 1);
                page++;
                if (page >= max)
                {
                    page = 0;
                    cpage++;
                    AddButton(265, 390, 22405, 22405, 600, GumpButtonType.Page, cpage);
                    AddPage(cpage);
                    if (cpage > 1) AddButton(25, 390, 22402, 22402, 600, GumpButtonType.Page, cpage - 1);
                }
                itemNum++;
            }
        }



        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            Type removeMe = null;

            int count = 154;
            foreach(Type t in this.lootBag.items)
            {
                if(info.ButtonID == count)
                {
                    removeMe = t;
                    break;
                }
                count++;
            }

            if (removeMe != null)
            {
                this.lootBag.items.Remove(removeMe);
                sender.Mobile.CloseGump(typeof(LootGump));
                sender.Mobile.SendGump(new LootGump(sender.Mobile, this.lootBag));
                sender.Mobile.SendMessage(15, "Removed " + removeMe.Name);
            }

            switch (info.ButtonID)
            {
                case 0:
                    {

                        break;
                    }
            }
        }
    }
}

