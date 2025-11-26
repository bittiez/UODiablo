using Server;
using Server.Items;
using Server.Targeting;
using System;

namespace Bittiez.RuneWords
{
    public class AllRuneBag : Bag
    {
        [Constructable]
        public AllRuneBag() : base()
        {
            this.AddItem(new RuneWords.Fe());
            this.AddItem(new RuneWords.Ar());
            this.AddItem(new RuneWords.Bjarka());
            this.AddItem(new RuneWords.Hagal());
            this.AddItem(new RuneWords.Is());
            this.AddItem(new RuneWords.Kaun());
            this.AddItem(new RuneWords.Logur());
            this.AddItem(new RuneWords.Maoer());
            this.AddItem(new RuneWords.Nauo());
            this.AddItem(new RuneWords.Os());
            this.AddItem(new RuneWords.Purs());
            this.AddItem(new RuneWords.Reio());
            this.AddItem(new RuneWords.Tyr());
            this.AddItem(new RuneWords.Sol());
            this.AddItem(new RuneWords.Ur());
            this.AddItem(new RuneWords.Yr());
        }
        public AllRuneBag(Serial serial) : base(serial)
        { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }
    }
    public abstract class Rune : Item
    {
        private double m_rarity = 100.0;

        public double Rarity
        {
            get { return m_rarity; }
            set { m_rarity = value; }
        }

        [Constructable]
        public Rune() : base(0x1F14)
        {
            Hue = 923;
        }
        public Rune(Serial serial) : base(serial)
        {
        }

        public static void AddLoot(Mobile m)
        {
            if (m != null)
            {
                if (m.HitsMax > 75)
                {
                    double chance = Utility.Random(10000);
                    Rune runeToAdd = null;
                    if (100 > chance)
                    {
                        switch (Utility.Random(17))
                        {
                            case 0: runeToAdd = new Fe(); break;
                            case 1: runeToAdd = new Ur(); break;
                            case 2: runeToAdd = new Purs(); break;
                            case 3: runeToAdd = new Os(); break;
                            case 4: runeToAdd = new Reio(); break;
                            case 5: runeToAdd = new Kaun(); break;
                            case 6: runeToAdd = new Hagal(); break;
                            case 7: runeToAdd = new Nauo(); break;
                            case 8: runeToAdd = new Is(); break;
                            case 9: runeToAdd = new Ar(); break;
                            case 10: runeToAdd = new Sol(); break;
                            case 11: runeToAdd = new Tyr(); break;
                            case 12: runeToAdd = new Bjarka(); break;
                            case 13: runeToAdd = new Maoer(); break;
                            case 14: runeToAdd = new Logur(); break;
                            case 15: runeToAdd = new Yr(); break;
                            case 16: m.AddToBackpack(new RuneBoard()); break;
                            default: break;
                        }
                        if (runeToAdd != null)
                        {
                            m.AddToBackpack(runeToAdd);
                        }
                    }
                }
            }
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("/c[gold]a magic rune/cd");
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("That must be in your posession!");
                return;
            }
            from.SendMessage("What rune board would you like to etch this rune into?");
            from.Target = new RuneTarget(this);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
        }

        public static Type[] AllRunes = new Type[]
        {
            typeof(Fe),
            typeof(Ar),
            typeof(Bjarka),
            typeof(Hagal),
            typeof(Is),
            typeof(Kaun),
            typeof(Logur),
            typeof(Maoer),
            typeof(Nauo),
            typeof(Os),
            typeof(Purs),
            typeof(Reio),
            typeof(Tyr),
            typeof(Sol),
            typeof(Ur),
            typeof(Yr)
        };
    }

    public class RuneTarget : Target
    {
        private Rune m_Rune;
        public RuneTarget(Rune rune)
            : base(10, false, TargetFlags.None)
        {
            m_Rune = rune;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (targeted is RuneBoard)
            {
                RuneBoard runeBoard = (RuneBoard)targeted;

                if (runeBoard.IsChildOf(from.Backpack) || runeBoard.IsAccessibleTo(from))
                {
                    runeBoard.addRune(m_Rune);
                    m_Rune.Delete();
                    from.SendMessage("You succesfully etched the rune into the board, destroying the rune in the process.");
                }
                else
                {
                    from.SendMessage("You need to be able to access that rune board.");
                }
                return;
            }

            from.SendMessage("You can't figure out a way to use this rune on that.");
        }
    }

    public class Fe : Rune
    {
        [Constructable]
        public Fe() : base()
        {
            Name = "Fe";
            Weight = 0.5;
        }

        public Fe(Serial serial) : base(serial)
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
    }
    public class Ur : Rune
    {
        [Constructable]
        public Ur() : base()
        {
            Name = "Ur";
            Weight = 0.5;
        }

        public Ur(Serial serial) : base(serial)
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
    }
    public class Purs : Rune
    {
        [Constructable]
        public Purs() : base()
        {
            Name = "Purs";
            Weight = 0.5;
        }

        public Purs(Serial serial) : base(serial)
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
    }
    public class Os : Rune
    {
        [Constructable]
        public Os() : base()
        {
            Name = "Os";
            Weight = 0.5;
        }

        public Os(Serial serial) : base(serial)
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
    }
    public class Reio : Rune
    {
        [Constructable]
        public Reio() : base()
        {
            Name = "Reio";
            Weight = 0.5;
        }

        public Reio(Serial serial) : base(serial)
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
    }
    public class Kaun : Rune
    {
        [Constructable]
        public Kaun() : base()
        {
            Name = "Kaun";
            Weight = 0.5;
        }

        public Kaun(Serial serial) : base(serial)
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
    }
    public class Hagal : Rune
    {
        [Constructable]
        public Hagal() : base()
        {
            Name = "Hagal";
            Weight = 0.5;
        }

        public Hagal(Serial serial) : base(serial)
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
    }
    public class Nauo : Rune
    {
        [Constructable]
        public Nauo() : base()
        {
            Name = "Nauo";
            Weight = 0.5;
        }

        public Nauo(Serial serial) : base(serial)
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
    }
    public class Is : Rune
    {
        [Constructable]
        public Is() : base()
        {
            Name = "Is";
            Weight = 0.5;
        }

        public Is(Serial serial) : base(serial)
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
    }
    public class Ar : Rune
    {
        [Constructable]
        public Ar() : base()
        {
            Name = "Ar";
            Weight = 0.5;
        }

        public Ar(Serial serial) : base(serial)
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
    }
    public class Sol : Rune
    {
        [Constructable]
        public Sol() : base()
        {
            Name = "Sol";
            Weight = 0.5;
        }

        public Sol(Serial serial) : base(serial)
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
    }
    public class Tyr : Rune
    {
        [Constructable]
        public Tyr() : base()
        {
            Name = "Tyr";
            Weight = 0.5;
        }

        public Tyr(Serial serial) : base(serial)
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
    }
    public class Bjarka : Rune
    {
        [Constructable]
        public Bjarka() : base()
        {
            Name = "Bjarka";
            Weight = 0.5;
        }

        public Bjarka(Serial serial) : base(serial)
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
    }
    public class Maoer : Rune
    {
        [Constructable]
        public Maoer() : base()
        {
            Name = "Maoer";
            Weight = 0.5;
        }

        public Maoer(Serial serial) : base(serial)
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
    }
    public class Logur : Rune
    {
        [Constructable]
        public Logur() : base()
        {
            Name = "Logur";
            Weight = 0.5;
        }

        public Logur(Serial serial) : base(serial)
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
    }
    public class Yr : Rune
    {
        [Constructable]
        public Yr() : base()
        {
            Name = "Yr";
            Weight = 0.5;
        }

        public Yr(Serial serial) : base(serial)
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
    }
}
