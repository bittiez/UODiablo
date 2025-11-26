using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;
using Server.Mobiles;
using VitaNex.Mobiles;

namespace Server.Custom.Mobiles
{
    #region Placeholder Items

    public class UnidentifiedArmorToken : Item
    {
        [Constructable]
        public UnidentifiedArmorToken() : base(0x14F0) // Deed graphic
        {
            Name = "Unidentified Armor";
            Hue = 2500; // Unique hue
            Weight = 0;
        }

        public UnidentifiedArmorToken(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class UnidentifiedWeaponToken : Item
    {
        [Constructable]
        public UnidentifiedWeaponToken() : base(0x14F0) // Deed graphic
        {
            Name = "Unidentified Weapon";
            Hue = 2125; // Unique hue
            Weight = 0;
        }

        public UnidentifiedWeaponToken(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    public class UnidentifiedJewelryToken : Item
    {
        [Constructable]
        public UnidentifiedJewelryToken() : base(0x14F0) // Deed graphic
        {
            Name = "Unidentified Jewelry";
            Hue = 2213; // Unique hue
            Weight = 0;
        }

        public UnidentifiedJewelryToken(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }

    #endregion

    public class Gheed : AdvancedVendor
    {
        private const int ARMOR_PRICE = 10000;
        private const int WEAPON_PRICE = 12000;
        private const int JEWELRY_PRICE = 20000;

        [Constructable]
        public Gheed() : base("the Gambler", typeof(Gold), "Gold", "GP")
        {
            Name = "Gheed";
            Female = false;
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            // Set up appearance
            EquipItem(new FancyShirt(Utility.RandomNeutralHue()));
            EquipItem(new LongPants(Utility.RandomNeutralHue()));
            EquipItem(new Boots(Utility.RandomNeutralHue()));

            // Add some character with a hat
            switch (Utility.Random(3))
            {
                case 0: EquipItem(new FeatheredHat(Utility.RandomNeutralHue())); break;
                case 1: EquipItem(new TricorneHat(Utility.RandomNeutralHue())); break;
                case 2: EquipItem(new FloppyHat(Utility.RandomNeutralHue())); break;
            }

            // Random hair
            HairItemID = Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047);
            HairHue = Utility.RandomHairHue();

            // Random facial hair
            FacialHairItemID = Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D);
            FacialHairHue = HairHue;

            CantWalk = true;
        }

        public Gheed(Serial serial) : base(serial)
        {
        }

        protected override void InitBuyInfo()
        {
            // Add the three gambling options
            AddStock<UnidentifiedArmorToken>(ARMOR_PRICE, "Unidentified Armor", 999);
            AddStock<UnidentifiedWeaponToken>(WEAPON_PRICE, "Unidentified Weapon", 999);
            AddStock<UnidentifiedJewelryToken>(JEWELRY_PRICE, "Unidentified Jewelry", 999);
        }

        protected override void OnItemReceived(Mobile buyer, Item item, IBuyItemInfo buy)
        {
            base.OnItemReceived(buyer, item, buy);

            Item generatedItem = null;

            // Check if this is a placeholder token and generate real item
            if (item is UnidentifiedArmorToken)
            {
                generatedItem = GenerateRandomArmor();
                item.Delete(); // Remove the token
            }
            else if (item is UnidentifiedWeaponToken)
            {
                generatedItem = GenerateRandomWeapon();
                item.Delete(); // Remove the token
            }
            else if (item is UnidentifiedJewelryToken)
            {
                generatedItem = GenerateRandomJewelry();
                item.Delete(); // Remove the token
            }

            // Give the generated item to the buyer
            if (generatedItem != null)
            {
                // Mark as identified since they just bought it
                if (generatedItem is BaseArmor)
                    ((BaseArmor)generatedItem).Identified = true;
                else if (generatedItem is BaseWeapon)
                    ((BaseWeapon)generatedItem).Identified = true;

                // Try to put in backpack, otherwise drop at feet
                if (!buyer.AddToBackpack(generatedItem))
                {
                    generatedItem.MoveToWorld(buyer.Location, buyer.Map);
                }

                buyer.SendMessage(0x35, "You have received: {0}", generatedItem.Name ?? generatedItem.GetType().Name);
            }
        }

        private Item GenerateRandomArmor()
        {
            // Combine all armor types
            List<Type> allArmorTypes = new List<Type>();
            allArmorTypes.AddRange(Loot.ArmorTypes);
            allArmorTypes.AddRange(Loot.SEArmorTypes);
            allArmorTypes.AddRange(Loot.MLArmorTypes);

            for (int attempts = 0; attempts < 10; attempts++)
            {
                Type armorType = allArmorTypes[Utility.Random(allArmorTypes.Count)];

                try
                {
                    Item armor = Activator.CreateInstance(armorType) as Item;

                    if (armor != null && armor is BaseArmor)
                    {
                        BaseArmor baseArmor = (BaseArmor)armor;

                        // Generate random properties with moderate budget
                        int budget = Utility.RandomMinMax(300, 500);
                        RunicReforging.GenerateRandomItem(baseArmor, 0, budget, budget);

                        return armor;
                    }
                }
                catch
                {
                    continue;
                }
            }

            // Fallback to a basic item if generation fails
            return new LeatherChest();
        }

        private Item GenerateRandomWeapon()
        {
            // Combine all weapon types
            List<Type> allWeaponTypes = new List<Type>();
            allWeaponTypes.AddRange(Loot.WeaponTypes);
            allWeaponTypes.AddRange(Loot.SEWeaponTypes);
            allWeaponTypes.AddRange(Loot.AosWeaponTypes);
            allWeaponTypes.AddRange(Loot.MLWeaponTypes);
            allWeaponTypes.AddRange(Loot.RangedWeaponTypes);
            allWeaponTypes.AddRange(Loot.AosRangedWeaponTypes);

            for (int attempts = 0; attempts < 10; attempts++)
            {
                Type weaponType = allWeaponTypes[Utility.Random(allWeaponTypes.Count)];

                try
                {
                    Item weapon = Activator.CreateInstance(weaponType) as Item;

                    if (weapon != null && weapon is BaseWeapon)
                    {
                        BaseWeapon baseWeapon = (BaseWeapon)weapon;

                        // Generate random properties with moderate budget
                        int budget = Utility.RandomMinMax(300, 500);
                        RunicReforging.GenerateRandomItem(baseWeapon, 0, budget, budget);

                        return weapon;
                    }
                }
                catch
                {
                    continue;
                }
            }

            // Fallback to a basic item if generation fails
            return new Longsword();
        }

        private Item GenerateRandomJewelry()
        {
            // Use standard jewelry types
            List<Type> jewelryTypes = new List<Type>(Loot.JewelryTypes);

            for (int attempts = 0; attempts < 10; attempts++)
            {
                Type jewelryType = jewelryTypes[Utility.Random(jewelryTypes.Count)];

                try
                {
                    Item jewelry = Activator.CreateInstance(jewelryType) as Item;

                    if (jewelry != null && jewelry is BaseJewel)
                    {
                        BaseJewel baseJewel = (BaseJewel)jewelry;

                        // Generate random properties with moderate budget
                        int budget = Utility.RandomMinMax(300, 500);
                        RunicReforging.GenerateRandomItem(baseJewel, 0, budget, budget);

                        return jewelry;
                    }
                }
                catch
                {
                    continue;
                }
            }

            // Fallback to a basic item if generation fails
            return new GoldRing();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version

            // Reset last restock so inventory regenerates on first open
            LastRestock = DateTime.MinValue;
        }
    }
}
