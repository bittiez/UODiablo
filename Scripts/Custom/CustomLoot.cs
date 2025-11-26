using Bittiez.RuneWords;
using Server;
using Server.Custom.Items;
using Server.Items;
using Server.Mobiles;

namespace Bittiez.CustomLoot
{
    public static class CustomLoot
    {
        public static void Initialize()
        {
            EventSink.CreatureDeath += EventSink_CreatureDeath;
        }

        private static void EventSink_CreatureDeath(CreatureDeathEventArgs e)
        {
            if (e.Creature == null)
            {
                return;
            }

            if (e.Creature is BaseVendor)
            {
                return;
            }

            BaseCreature baseCreature = e.Creature as BaseCreature;

            if (baseCreature != null)
            {
                if (RandomChance(5)) //5% chance to drop a health globe
                {
                    HealthGlobe.DropGlobe(baseCreature.Location, baseCreature.Map);
                }
            }

            //Make sure the corpse exists
            if (e.Corpse == null)
            {
                return;
            }


            if (baseCreature != null)
            {
                if (baseCreature.IsChampionSpawn || baseCreature.MonsterLevel <= 0)
                {
                    return;
                }

                //5% chance for waypoint scroll on all mobs
                if (RandomChance(5))
                {
                    e.Corpse.AddItem(new WayPointScroll());
                }

                //Drop necro regs on all undead types
                if (baseCreature.Tribe == TribeType.Undead)
                {
                    switch (RandomImpl.Next(7))
                    {
                        case 0:
                            e.Corpse.AddItem(new BatWing());
                            break;
                        case 1:
                            e.Corpse.AddItem(new GraveDust());
                            break;
                        case 2:
                            e.Corpse.AddItem(new DaemonBlood());
                            break;
                        case 3:
                            e.Corpse.AddItem(new PigIron());
                            break;
                        case 4:
                            e.Corpse.AddItem(new NoxCrystal());
                            break;
                    }
                }

                AddLoot(e.Killer, LootPack.UOD_HPManaPots, e.Corpse);

                if (baseCreature.MonsterLevel > 2)
                {
                    AddLoot(e.Killer, LootPack.UOD_ReagentTool, e.Corpse);
                }

                if (baseCreature.MonsterLevel > 4)
                {
                    AddLoot(e.Killer, LootPack.UOD_RuneBoard, e.Corpse);
                    AddLoot(e.Killer, LootPack.UOD_GearBag, e.Corpse);
                    AddLoot(e.Killer, LootPack.Gems, e.Corpse);
                    AddLoot(e.Killer, LootPack.LowScrolls, e.Corpse);
                }

                if (baseCreature.MonsterLevel <= 12)
                {
                    AddLoot(e.Killer, LootPack.UOD_Gold3to15, e.Corpse);
                    AddLoot(e.Killer, LootPack.UOD_BelowLvl12, e.Corpse);
                }

                int r = RandomImpl.Next(Rune.AllRunes.Length * 15);
                if (Rune.AllRunes.Length > r)
                {
                    e.Corpse.AddItem(System.Activator.CreateInstance(Rune.AllRunes[r]) as Item);
                }

            }
        }

        /// <summary>
        /// Return true if the random chance is met, false if not
        /// </summary>
        /// <param name="chance">0-100% chance to be true</param>
        /// <returns></returns>
        private static bool RandomChance(double chance)
        {
            return RandomImpl.Next(1001) < (chance * 10);
        }

        private static void AddLoot(Mobile killer, LootPack lpack, Container container)
        {
            if (killer != null)
            {
                lpack.ForceGenerate(killer, container);
            }
        }
    }
}
