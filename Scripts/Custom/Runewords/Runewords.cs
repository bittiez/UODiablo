using Server;
using Server.Items;
using System;
using System.Collections.Generic;

namespace Bittiez.RuneWords
{
    public abstract class RuneWord
    {
        private List<Type> m_RequiredRunes = new List<Type>();
        private string m_Name = "";
        public List<Type> RequiredRunes { get { return m_RequiredRunes; } set { this.m_RequiredRunes = value; } }
        public string Name { get { return m_Name; } set { m_Name = value; } }

        public virtual bool ApplyToWeapon(Item weapon) { return false; }
        public virtual bool ApplyToArmor(Item armor) { return false; }

        public void ApplyRuneWordName(Item item)
        {
            string name = "/c[gold][" + Name + "]";

            string runes = " [";
            foreach (Type r in RequiredRunes)
            {
                runes += r.Name;
            }
            runes += "]/cd";

            name += runes;
            item.HasRuneword = true;
            item.RuneWordName = name;
        }

        public static void AttemptAddSkillBonus(BaseWeapon weapon, SkillName skill, double value)
        {
            for (int i = 0; i < 5; i++)
            {
                if (weapon.SkillBonuses.GetBonus(i) == 0)
                {
                    weapon.SkillBonuses.SetValues(i, skill, value);
                    return;
                }
            }
        }
        public static void AttemptAddSkillBonus(BaseArmor armor, SkillName skill, double value)
        {
            for (int i = 0; i < 5; i++)
            {
                if (armor.SkillBonuses.GetBonus(i) == 0)
                {
                    armor.SkillBonuses.SetValues(i, skill, value);
                    return;
                }
            }
        }
    }

    public class PursFe : RuneWord
    {
        public PursFe()
        {
            RequiredRunes.Add(typeof(Purs));
            RequiredRunes.Add(typeof(Fe));
            Name = "Wealth";
        }

        public override bool ApplyToWeapon(Item weapon)
        {
            if (weapon is BaseWeapon)
            {
                BaseWeapon bw_weapon = (BaseWeapon)weapon;
                bw_weapon.WeaponAttributes.SelfRepair = 1;
                bw_weapon.WeaponAttributes.DurabilityBonus = 30;
                bw_weapon.Attributes.Luck = 85;
                bw_weapon.Attributes.LowerRegCost = 12;
                ApplyRuneWordName(bw_weapon);
                return true;
            }
            return base.ApplyToWeapon(weapon);
        }

        public override bool ApplyToArmor(Item armor)
        {
            if (armor is BaseArmor)
            {
                BaseArmor bw_armor = (BaseArmor)armor;
                bw_armor.Attributes.AttackChance = 6;
                bw_armor.ArmorAttributes.LowerStatReq = 1;
                bw_armor.Attributes.Luck = 75;
                bw_armor.Attributes.LowerRegCost = 12;
                ApplyRuneWordName(bw_armor);
                return true;
            }
            return base.ApplyToArmor(armor);
        }
    }

    public class LogurAr : RuneWord
    {
        public LogurAr()
        {
            RequiredRunes.Add(typeof(Logur));
            RequiredRunes.Add(typeof(Ar));
            Name = "Magabun";
        }

        public override bool ApplyToWeapon(Item weapon)
        {
            if (weapon is BaseWeapon)
            {
                BaseWeapon bw_weapon = (BaseWeapon)weapon;
                bw_weapon.WeaponAttributes.HitLeechMana = 1;
                bw_weapon.WeaponAttributes.ResistColdBonus = 15;
                bw_weapon.Attributes.RegenMana = 8;
                bw_weapon.Attributes.BonusMana = 12;
                bw_weapon.Attributes.LowerRegCost = 9;
                bw_weapon.Attributes.SpellChanneling = 1;
                AttemptAddSkillBonus(bw_weapon, SkillName.Magery, 6.0);
                ApplyRuneWordName(bw_weapon);
                return true;
            }
            return base.ApplyToWeapon(weapon);
        }

        public override bool ApplyToArmor(Item armor)
        {
            if (armor is BaseArmor)
            {
                BaseArmor bw_armor = (BaseArmor)armor;
                bw_armor.Attributes.RegenMana = 8;
                bw_armor.Attributes.BonusInt = 6;
                bw_armor.Attributes.LowerManaCost = 4;
                bw_armor.ArmorAttributes.MageArmor = 1;
                bw_armor.Attributes.LowerRegCost = 4;
                bw_armor.Attributes.SpellChanneling = 1;
                AttemptAddSkillBonus(bw_armor, SkillName.Magery, 2.0);
                ApplyRuneWordName(bw_armor);
                return true;
            }
            return base.ApplyToArmor(armor);
        }
    }

    public class UrIs : RuneWord
    {
        public UrIs()
        {
            RequiredRunes.Add(typeof(Ur));
            RequiredRunes.Add(typeof(Is));
            Name = "Ice Shower";
        }

        public override bool ApplyToWeapon(Item weapon)
        {
            if (weapon is BaseWeapon)
            {
                BaseWeapon bw_weapon = (BaseWeapon)weapon;
                bw_weapon.WeaponAttributes.HitColdArea = 7;
                bw_weapon.WeaponAttributes.ResistColdBonus = 15;
                bw_weapon.AosElementDamages.Cold = 100;
                bw_weapon.Attributes.ReflectPhysical = 8;
                AttemptAddSkillBonus(bw_weapon, SkillName.MagicResist, 3.0);
                ApplyRuneWordName(bw_weapon);
                return true;
            }
            return base.ApplyToWeapon(weapon);
        }

        public override bool ApplyToArmor(Item armor)
        {
            if (armor is BaseArmor)
            {
                BaseArmor bw_armor = (BaseArmor)armor;
                bw_armor.Attributes.ReflectPhysical = 8;
                bw_armor.ArmorAttributes.DurabilityBonus = 25;
                bw_armor.Attributes.DefendChance = 9;
                AttemptAddSkillBonus(bw_armor, SkillName.MagicResist, 3.0);
                ApplyRuneWordName(bw_armor);
                return true;
            }
            return base.ApplyToArmor(armor);
        }
    }
}
