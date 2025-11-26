using Server;
using Server.Items;
using Server.Targeting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bittiez.RuneWords
{
    public class RuneBoard : Item
    {
        private List<Type> m_Runes = new List<Type>();
        public List<Type> Runes
        {
            get
            { return m_Runes; }
            set { m_Runes = value; }
        }

        [Constructable]
        public RuneBoard() : base(0x0B67)
        {
            Name = "a rune board";
            Hue = 11;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            list.Add("/c[gold]A rune board, used for etching runes into items/cd");

            if (Runes.Count > 0)
            {
                String runeList = "";
                foreach (Type r in Runes)
                {
                    runeList += r.Name;
                }

                list.Add("/c[gold]" + runeList + "/cd");
            }
        }

        public RuneBoard(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);
            from.SendMessage("What would you like to attempt to apply this rune word to?");
            from.Target = new RuneBoardTarget(this);
        }

        public void addRune(Rune rune)
        {
            Runes.Add(rune.GetType());
            InvalidateProperties();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(Runes.Count);
            foreach (Type r in Runes)
            {
                writer.Write(r.FullName);
            }
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            int count = reader.ReadInt();
            Runes = new List<Type>();

            for (int i = 0; i < count; i++)
            {
                string s = reader.ReadString();
                try
                {
                    Type type = Type.GetType(s);
                    if (type != null)
                        Runes.Add(type);
                }
                catch { }
            }
            InvalidateProperties();
        }
    }

    public class RuneBoardTarget : Target
    {
        private RuneBoard runeBoard;
        public RuneBoardTarget(RuneBoard runeBoard) : base(10, false, TargetFlags.None)
        {
            this.runeBoard = runeBoard;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            base.OnTarget(from, targeted);

            if (runeBoard == null) return;

            if (runeBoard.Runes.Count < 1)
            {
                from.SendMessage("You haven't etched any runes into this rune board yet.");
                return;
            }

            if (targeted is Item item && (item.HasRuneword || item.IsArtifact))
            {
                from.SendMessage("You could not etch these into the item.");
                return;
            }

            String runeList = "";
            foreach (Type r in runeBoard.Runes)
            {
                runeList += r.Name;
            }

            Type runeWordType = Type.GetType("Bittiez.RuneWords." + runeList);
            if (runeWordType == null)
            {
                from.SendMessage("The can feel the lack of magic in the rune etchings, this may not be the correct runes for a rune word.");
                return;
            }

            RuneWord runeWord = null;
            try { runeWord = Activator.CreateInstance(runeWordType) as RuneWord; } catch { }

            if (runeWord == null)
            {
                from.SendMessage("The can feel the lack of magic in the rune etchings, this may not be the correct runes for a rune word.");
                return;
            }

            if (targeted is BaseWeapon)
            {
                BaseWeapon weapon = (BaseWeapon)targeted;
                runeWord.ApplyToWeapon(weapon);
                if (from.AccessLevel == AccessLevel.Player)
                    runeBoard.Delete();
                return;
            }

            if (targeted is BaseArmor)
            {
                BaseArmor armor = (BaseArmor)targeted;
                runeWord.ApplyToArmor(armor);
                if (from.AccessLevel == AccessLevel.Player)
                    runeBoard.Delete();
                return;
            }

            from.SendMessage("You only know how to etch rune words into armor and weapons.");
        }
    }
}
