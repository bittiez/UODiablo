using Server.Mobiles;
using Server.Regions;
using System;

namespace Server.Custom.Custom_Regions
{
    internal class UODRegions
    {
        public static void Initialize()
        {
            ThreeDifficultyTown("Rogue Encampment", new Point3D(1499, 2164, 3), new Rectangle2D(1462, 2106, 100, 140));

            ThreeDifficultyDungeonsArea("Den of evil", new Rectangle2D(2, 3, 64, 93), new Rectangle2D(66, 69, 23, 27));

            ThreeDifficultyDungeonsArea("Crypt", new Rectangle2D(83, 21, 29, 35), new Rectangle2D(112, 21, 78, 63));

            ThreeDifficultyDungeonsAreaRegularMaps("Underground Passage", GetRectangle2D(2371, 2432, 2382, 2441), GetRectangle2D(2354, 2376, 2393, 2434), GetRectangle2D(2393, 2401, 2415, 24010));

            ThreeDifficultyDungeonsArea("Forgotten Tower Level 1", GetRectangle2D(168, 114, 193, 135));
            ThreeDifficultyDungeonsArea("Forgotten Tower Level 2", GetRectangle2D(199, 16, 327, 168));
            ThreeDifficultyDungeonsArea("Forgotten Tower Level 3", GetRectangle2D(335, 33, 479, 207));
            ThreeDifficultyDungeonsArea("Forgotten Tower Level 4", GetRectangle2D(520, 65, 607, 183));

            ThreeDifficultyDungeonsArea("Cave", GetRectangle2D(21, 103, 134, 166));

            ThreeDifficultyDungeonsArea("Barracks", GetRectangle2D(28, 198, 121, 322));

            ThreeDifficultyDungeonsArea("Jail Level 1", GetRectangle2D(130, 186, 216, 319));
            ThreeDifficultyDungeonsArea("Jail Level 2", GetRectangle2D(231, 197, 302, 287));

            ThreeDifficultyDungeonsArea("Inner Cloister", GetRectangle2D(4123, 1093, 4216, 1131));

            ThreeDifficultyDungeonsArea("Catacombs Level 1", GetRectangle2D(316, 219, 462, 294));
            ThreeDifficultyDungeonsArea("Catacombs Level 2", GetRectangle2D(308, 306, 452, 407));
            ThreeDifficultyDungeonsArea("Catacombs Level 3", GetRectangle2D(475, 314, 598, 393));
            ThreeDifficultyDungeonsArea("Catacombs Level 4", GetRectangle2D(660, 254, 723, 357));
        }

        private static void ThreeDifficultyTown(string name, Point3D respawn, params Rectangle2D[] area)
        {
            new UODTown(name, Map.Normal, area) { GoLocation = respawn };
            new UODTown(name, Map.Nightmare, area) { GoLocation = respawn };
            new UODTown(name, Map.Hell, area) { GoLocation = respawn };
        }

        private static void ThreeDifficultyDungeonsAreaRegularMaps(string name, params Rectangle2D[] area)
        {
            new UODDungeon(name, Map.Normal, area);
            new UODDungeon(name, Map.Nightmare, area);
            new UODDungeon(name, Map.Hell, area);
        }

        private static void ThreeDifficultyDungeonsArea(string name, params Rectangle2D[] area)
        {
            new UODDungeon(name, Map.Dungeons, area);
            new UODDungeon(name, Map.DungeonsNightmare, area);
            new UODDungeon(name, Map.DungeonsHell, area);
        }

        private static Rectangle2D GetRectangle2D(int x, int y, int x2, int y2)
        {
            return new Rectangle2D(x, y, x2 - x, y2 - y);
        }
    }

    internal class UODTown : GuardedRegion
    {
        public UODTown(string name, Map map, params Rectangle2D[] area) : base(name, map, Region.DefaultPriority, area)
        {
            Disabled = false;
            Register();
        }

        public override TimeSpan GetLogoutDelay(Mobile m) => TimeSpan.Zero;

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);
            m.SendMessage($"You have entered {Name}");
            if (m is PlayerMobile pm)
            {
                pm.SpawnPoint = GoLocation;
                pm.SpawnMap = Map;
            }
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
            m.SendMessage($"You have left {Name}");
        }
    }

    internal class UODDungeon : BaseRegion
    {
        public UODDungeon(string name, Map map, params Rectangle2D[] area) : base(name, map, Region.DefaultPriority, area)
        {
            Register();
        }

        public override void OnEnter(Mobile m)
        {
            base.OnEnter(m);
            m.SendMessage($"You have entered {Name}");
        }

        public override void OnExit(Mobile m)
        {
            base.OnExit(m);
            m.SendMessage($"You have left {Name}");
        }

        public override bool AllowHousing(Mobile from, Point3D p) => false;

        public override bool YoungProtected => false;

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            global = LightCycle.DungeonLevel;
        }
    }
}
