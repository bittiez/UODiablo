// Treasure Chest Pack - Version 0.99I
// By Nerun

using Server.Mobiles;
using Server.Spells;

namespace Server.Items
{

    // ---------- [Level 1] ----------
    // Large, Medium and Small Crate
    [FlipableAttribute(0xe3e, 0xe3f)]
    public class TreasureLevel1 : BaseTreasureChestMod
    {
        public override int DefaultGumpID { get { return 0x49; } }

        [Constructable]
        public TreasureLevel1() : base(Utility.RandomList(0xE3C, 0xE3E, 0x9a9))
        {
            RequiredSkill = 26;
            LockLevel = this.RequiredSkill - Utility.Random(1, 10);
            MaxLockLevel = this.RequiredSkill;
            TrapType = TrapType.MagicTrap;
            TrapPower = 1 * Utility.Random(1, 25);

            DropItem(new Gold(30, 100));
            DropItem(new Bolt(10));

            AddLoot(Loot.RandomWeapon());
            AddLoot(Loot.RandomArmorOrShield());
            AddLoot(Loot.RandomJewelry());

            for (int i = Utility.Random(3) + 1; i > 0; i--) // random 1 to 3
                DropItem(Loot.RandomGem());
        }

        public TreasureLevel1(Serial serial) : base(serial)
        {
        }

        public override void BeforeFirstOpened(Mobile from)
        {
            base.BeforeFirstOpened(from);

            if (Map == Map.Normal)
            {
                DropItem(Loot.RandomClothing());
            }
            else if (Map == Map.Nightmare)
            {
                DropItem(new Gold(30, 100));
                for (int i = Utility.Random(3) + 1; i > 0; i--) // random 1 to 3
                    DropItem(Loot.RandomGem());
            }
            else if (Map == Map.Hell)
            {
                DropItem(new Gold(60, 200));
                for (int i = Utility.Random(6) + 1; i > 0; i--) // random 1 to 3
                    DropItem(Loot.RandomGem());
            }
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

    // ---------- [Level 1 Hybrid] ----------
    // Large, Medium and Small Crate
    [FlipableAttribute(0xe3e, 0xe3f)]
    public class TreasureLevel1h : BaseTreasureChestMod
    {
        public override int DefaultGumpID { get { return 0x49; } }

        [Constructable]
        public TreasureLevel1h() : base(Utility.RandomList(0xE3C, 0xE3E, 0x9a9))
        {
            RequiredSkill = 26;
            LockLevel = this.RequiredSkill - Utility.Random(1, 10);
            MaxLockLevel = this.RequiredSkill;
            TrapType = TrapType.MagicTrap;
            TrapPower = 1 * Utility.Random(1, 25);

            DropItem(new Gold(10, 40));
            DropItem(new Bolt(5));
            switch (Utility.Random(2))
            {
                case 0: DropItem(new Shoes(Utility.Random(1, 2))); break;
                case 1: DropItem(new Sandals(Utility.Random(1, 2))); break;
            }

            switch (Utility.Random(3))
            {
                case 0: DropItem(new BeverageBottle(BeverageType.Ale)); break;
                case 1: DropItem(new BeverageBottle(BeverageType.Liquor)); break;
                case 2: DropItem(new Jug(BeverageType.Cider)); break;
            }
        }

        public TreasureLevel1h(Serial serial) : base(serial)
        {
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

    // ---------- [Level 2] ----------
    // Large, Medium and Small Crate
    // Wooden, Metal and Metal Golden Chest
    // Keg and Barrel
    [FlipableAttribute(0xe43, 0xe42)]
    public class TreasureLevel2 : BaseTreasureChestMod
    {
        [Constructable]
        public TreasureLevel2() : base(Utility.RandomList(0xe3c, 0xE3E, 0x9a9, 0xe42, 0x9ab, 0xe40, 0xe7f, 0xe77))
        {
            RequiredSkill = 36;
            LockLevel = this.RequiredSkill - Utility.Random(1, 10);
            MaxLockLevel = this.RequiredSkill;
            TrapType = TrapType.MagicTrap;
            TrapPower = 2 * Utility.Random(1, 25);

            DropItem(new Gold(70, 100));
            DropItem(new Arrow(10));
            DropItem(Loot.RandomPotion());
            AddLoot(Loot.RandomWeapon());
            AddLoot(Loot.RandomArmorOrShield());
            AddLoot(Loot.RandomJewelry());
            for (int i = Utility.Random(1, 2); i > 1; i--)
            {
                Item ReagentLoot = Loot.RandomReagent();
                ReagentLoot.Amount = Utility.Random(1, 2);
                DropItem(ReagentLoot);
            }
            if (Utility.RandomBool()) //50% chance
                for (int i = Utility.Random(8) + 1; i > 0; i--)
                    DropItem(Loot.RandomScroll(0, 39, SpellbookType.Regular));

            if (Utility.RandomBool()) //50% chance
                for (int i = Utility.Random(6) + 1; i > 0; i--)
                    DropItem(Loot.RandomGem());
        }

        public TreasureLevel2(Serial serial) : base(serial)
        {
        }


        public override void BeforeFirstOpened(Mobile from)
        {
            base.BeforeFirstOpened(from);

            if (Map == Map.Normal)
            {
            }
            else if (Map == Map.Nightmare)
            {
                DropItem(new Gold(70, 100));
                DropItem(Loot.RandomPotion());

                for (int i = Utility.Random(6) + 1; i > 0; i--)
                    DropItem(Loot.RandomGem());

                if (Utility.RandomBool()) //50% chance
                    for (int i = Utility.Random(2) + 1; i > 0; i--)
                        DropItem(Loot.RandomScroll(0, (int)(Loot.RegularScrollTypes.Length / 2), SpellbookType.Regular));
            }
            else if (Map == Map.Hell)
            {
                DropItem(new Gold(140, 200));
                DropItem(Loot.RandomPotion());
                DropItem(Loot.RandomPotion());

                for (int i = Utility.Random(10) + 1; i > 0; i--)
                    DropItem(Loot.RandomGem());

                if (Utility.RandomBool()) //50% chance
                    for (int i = Utility.Random(2) + 1; i > 0; i--)
                        DropItem(Loot.RandomScroll(0, Loot.RegularScrollTypes.Length, SpellbookType.Regular));
            }
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

    // ---------- [Level 3] ----------
    // Wooden, Metal and Metal Golden Chest
    [FlipableAttribute(0x9ab, 0xe7c)]
    public class TreasureLevel3 : BaseTreasureChestMod
    {
        public override int DefaultGumpID { get { return 0x4A; } }

        [Constructable]
        public TreasureLevel3() : base(Utility.RandomList(0x9ab, 0xe40, 0xe42))
        {
            RequiredSkill = 56;
            LockLevel = this.RequiredSkill - Utility.Random(1, 10);
            MaxLockLevel = this.RequiredSkill;
            TrapType = TrapType.MagicTrap;
            TrapPower = 3 * Utility.Random(1, 25);

            DropItem(new Gold(180, 240));
            DropItem(new Arrow(10));

            for (int i = Utility.Random(1, 3); i > 1; i--)
            {
                Item ReagentLoot = Loot.RandomReagent();
                ReagentLoot.Amount = Utility.Random(1, 9);
                DropItem(ReagentLoot);
            }

            for (int i = Utility.Random(1, 3); i > 1; i--)
                DropItem(Loot.RandomPotion());

            if (0.67 > Utility.RandomDouble()) //67% chance = 2/3
                for (int i = Utility.Random(12) + 1; i > 0; i--)
                    DropItem(Loot.RandomScroll(0, 47, SpellbookType.Regular));

            if (0.67 > Utility.RandomDouble()) //67% chance = 2/3
                for (int i = Utility.Random(9) + 1; i > 0; i--)
                    DropItem(Loot.RandomGem());

            for (int i = Utility.Random(1, 3); i > 1; i--)
                DropItem(Loot.RandomWand());

            MagicItemGen();

            for (int i = Utility.Random(1, 2); i > 1; i--)
                AddLoot(Loot.RandomJewelry());

            // Magic clothing (not implemented)

            // Magic jewelry (not implemented)
        }

        public TreasureLevel3(Serial serial) : base(serial) { }


        public override void BeforeFirstOpened(Mobile from)
        {
            base.BeforeFirstOpened(from);

            if (Map == Map.Normal)
            {
                for (int i = Utility.Random(1, 2); i > 1; i--)
                    AddLoot(Loot.RandomClothing());
            }
            else if (Map == Map.Nightmare)
            {
                DropItem(new Gold(180, 240));

                for (int i = Utility.Random(1, 3); i > 1; i--)
                    DropItem(Loot.RandomPotion());

                if (Utility.RandomBool()) //50% chance
                    for (int i = Utility.Random(5) + 1; i > 0; i--)
                        DropItem(Loot.RandomScroll(0, (int)(Loot.RegularScrollTypes.Length / 2), SpellbookType.Regular));

                MagicItemGen();
            }
            else if (Map == Map.Hell)
            {
                DropItem(new Gold(360, 480));

                for (int i = Utility.Random(1, 6); i > 1; i--)
                    DropItem(Loot.RandomPotion());

                if (Utility.RandomBool()) //50% chance
                    for (int i = Utility.Random(5) + 1; i > 0; i--)
                        DropItem(Loot.RandomScroll(0, Loot.RegularScrollTypes.Length, SpellbookType.Regular));

                MagicItemGen();
            }
        }

        private void MagicItemGen()
        {
            // Magical ArmorOrWeapon
            for (int i = Utility.Random(1, 3); i > 1; i--)
            {
                Item item = Loot.RandomArmorOrShieldOrWeapon();

                if (!Core.AOS)
                {
                    if (item is BaseWeapon)
                    {
                        BaseWeapon weapon = (BaseWeapon)item;
                        weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(3);
                        weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(3);
                        weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(3);
                        weapon.Quality = ItemQuality.Normal;
                    }
                    else if (item is BaseArmor)
                    {
                        BaseArmor armor = (BaseArmor)item;
                        armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random(3);
                        armor.Durability = (ArmorDurabilityLevel)Utility.Random(3);
                        armor.Quality = ItemQuality.Normal;
                    }
                }
                else
                    AddLoot(item);
            }
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

    // ---------- [Level 4] ----------
    // Wooden, Metal and Metal Golden Chest
    [FlipableAttribute(0xe41, 0xe40)]
    public class TreasureLevel4 : BaseTreasureChestMod
    {
        [Constructable]
        public TreasureLevel4() : base(Utility.RandomList(0xe40, 0xe42, 0x9ab))
        {
            RequiredSkill = 66;
            LockLevel = this.RequiredSkill - Utility.Random(1, 10);
            MaxLockLevel = this.RequiredSkill;
            TrapType = TrapType.MagicTrap;
            TrapPower = 4 * Utility.Random(1, 25);

            DropItem(new Gold(200, 400));
            DropItem(new BlankScroll(Utility.Random(1, 4)));

            RegGen();

            for (int i = Utility.Random(1, 4); i > 1; i--)
                DropItem(Loot.RandomPotion());

            if (0.75 > Utility.RandomDouble()) //75% chance = 3/4
                for (int i = Utility.RandomMinMax(8, 16); i > 0; i--)
                    DropItem(Loot.RandomScroll(0, 47, SpellbookType.Regular));

            if (0.75 > Utility.RandomDouble()) //75% chance = 3/4
                for (int i = Utility.RandomMinMax(6, 12) + 1; i > 0; i--)
                    DropItem(Loot.RandomGem());

            for (int i = Utility.Random(1, 4); i > 1; i--)
                DropItem(Loot.RandomWand());

            MagicItemGen();

            for (int i = Utility.Random(1, 2); i > 1; i--)
                AddLoot(Loot.RandomJewelry());
        }

        public TreasureLevel4(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (CheckLocked(from))
                return;

            if (Utility.RandomBool() && !OpenedOnce)
            {
                for (int i = 0; i < Utility.Random(4); i++)
                {
                    Skeleton s = new Skeleton();
                    Point3D p = new Point3D(Location);
                    if (SpellHelper.FindValidSpawnLocation(Map, ref p, true))
                    {
                        s.MoveToWorld(p, Map);
                    }
                }
            }

            base.OnDoubleClick(from);
        }

        public override void BeforeFirstOpened(Mobile from)
        {
            base.BeforeFirstOpened(from);

            if (Map == Map.Normal)
            {
                for (int i = Utility.Random(1, 2); i > 1; i--)
                    AddLoot(Loot.RandomClothing());
            }
            else if (Map == Map.Nightmare)
            {
                MagicItemGen();
                DropItem(new Gold(200, 400));
                for (int i = Utility.Random(1, 4); i > 1; i--)
                    DropItem(Loot.RandomPotion());

                if (Utility.RandomBool()) //50% chance
                    for (int i = Utility.Random(9) + 1; i > 0; i--)
                        DropItem(Loot.RandomScroll(0, (int)(Loot.RegularScrollTypes.Length / 2), SpellbookType.Regular));

            }
            else if (Map == Map.Hell)
            {
                MagicItemGen();
                DropItem(new Gold(400, 800));
                DropItem(new BlankScroll(Utility.Random(1, 4)));
                RegGen();
                for (int i = Utility.Random(1, 8); i > 1; i--)
                    DropItem(Loot.RandomPotion());

                if (Utility.RandomBool()) //50% chance
                    for (int i = Utility.Random(9) + 1; i > 0; i--)
                        DropItem(Loot.RandomScroll(0, Loot.RegularScrollTypes.Length, SpellbookType.Regular));
            }
        }

        private void MagicItemGen()
        {
            // Magical ArmorOrWeapon
            for (int i = Utility.Random(1, 4); i > 1; i--)
            {
                Item item = Loot.RandomArmorOrShieldOrWeapon();

                if (!Core.AOS)
                {
                    if (item is BaseWeapon)
                    {
                        BaseWeapon weapon = (BaseWeapon)item;
                        weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(4);
                        weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(4);
                        weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(4);
                        weapon.Quality = ItemQuality.Normal;
                    }
                    else if (item is BaseArmor)
                    {
                        BaseArmor armor = (BaseArmor)item;
                        armor.ProtectionLevel = (ArmorProtectionLevel)Utility.Random(4);
                        armor.Durability = (ArmorDurabilityLevel)Utility.Random(4);
                        armor.Quality = ItemQuality.Normal;
                    }
                }
                else
                    AddLoot(item);
            }
        }

        private void RegGen()
        {
            for (int i = Utility.Random(1, 4); i > 1; i--)
            {
                Item ReagentLoot = Loot.RandomReagent();
                ReagentLoot.Amount = Utility.Random(6, 12);
                DropItem(ReagentLoot);
            }
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
