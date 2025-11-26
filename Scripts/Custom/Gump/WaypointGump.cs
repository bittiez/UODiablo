using Server.Commands;
using Server.Custom.Items;
using Server.Engines.CityLoyalty;
using Server.Gumps;
using Server.Mobiles;
using Server.Spells;
using System.Collections.Generic;

namespace Server.Custom.Gump
{
    internal class WaypointGump : Gumps.Gump
    {
        public PlayerMobile M { get; }

        public static void Initialize()
        {
            CommandSystem.Register("waypoint", AccessLevel.GameMaster, (e) => { if (e.Mobile != null && e.Mobile.Player) { new WaypointGump(e.Mobile as PlayerMobile).SendTo(e.Mobile.NetState); } });
            CommandSystem.Register("genwaypoints", AccessLevel.GameMaster, GenWaypoints);
            CommandSystem.Register("delwaypoints", AccessLevel.GameMaster, DelWaypoints);
        }

        private static void DelWaypoints(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Deleting waypoints..");

            List<Item> list = new List<Item>();

            foreach (Item item in World.Items.Values)
            {
                if (item is TravelWaypoint)
                    list.Add(item);
            }

            foreach (Item item in list)
            {
                item.Delete();
            }

            e.Mobile.SendMessage("Waypoints deleted!..");
        }

        private static void GenWaypoints(CommandEventArgs e)
        {
            DelWaypoints(e);

            e.Mobile.SendMessage("Generating waypoints..");

            GenWP3Difficulty(new Point3D(1497, 2162, 0), Buttons.RogueEncampment);
            GenWP3Difficulty(new Point3D(1928, 2219, 0), Buttons.ColdPlains);
            GenWP3Difficulty(new Point3D(2266, 2710, 0), Buttons.StonyField);
            GenWP3Difficulty(new Point3D(2927, 1738, 0), Buttons.DarkWood);
            GenWP3Difficulty(new Point3D(3666, 1384, 0), Buttons.BlackMarsh);
            GenWP3Difficulty(new Point3D(4244, 1112, 0), Buttons.OuterCloister);
            GenWP3DungeonsDifficulty(new Point3D(194, 198, 0), Buttons.Jail);
            GenWP3Difficulty(new Point3D(4145, 1103, 0), Buttons.InnerCloister);
            GenWP3DungeonsDifficulty(new Point3D(327, 318, 0), Buttons.Catacombs2);

            e.Mobile.SendMessage("Waypoints generated!..");
        }

        private static void GenWP(Point3D point, Map map, Difficulty difficulty, Buttons location)
        {
            TravelWaypoint wp = new TravelWaypoint() { WayPoint = WayPointList.GetWayPointValue(difficulty, location) };
            World.AddItem(wp);
            wp.MoveToWorld(point, map);
        }

        private static void GenWP3Difficulty(Point3D point, Buttons location)
        {
            GenWP(point, Map.Normal, Difficulty.Normal, location);
            GenWP(point, Map.Nightmare, Difficulty.Nightmare, location);
            GenWP(point, Map.Hell, Difficulty.Hell, location);
        }

        private static void GenWP3DungeonsDifficulty(Point3D point, Buttons location)
        {
            GenWP(point, Map.Dungeons, Difficulty.Normal, location);
            GenWP(point, Map.DungeonsNightmare, Difficulty.Nightmare, location);
            GenWP(point, Map.DungeonsHell, Difficulty.Hell, location);
        }


        public WaypointGump(PlayerMobile m) : base(400, 400)
        {
            AddBackground(0, 0, 183, 216, 40000);

            int buttonX = (183 >> 1) - (126 >> 1);
            int y = 216 - (3 * 30) >> 1;

            AddButton(buttonX, y, 40019, 40019, (int)Buttons.Normal, Gumps.GumpButtonType.Reply, (int)Buttons.Normal);
            AddLabel(buttonX + 3, y + 3, 0, "Normal");

            y += 30;

            AddButton(buttonX, y, 40019, 40019, (int)Buttons.Nightmare, Gumps.GumpButtonType.Reply, (int)Buttons.Nightmare);
            AddLabel(buttonX + 3, y + 3, 0, "Nightmare");

            y += 30;

            AddButton(buttonX, y, 40019, 40019, (int)Buttons.Hell, Gumps.GumpButtonType.Reply, (int)Buttons.Hell);
            AddLabel(buttonX + 3, y + 3, 0, "Hell");
            M = m;
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            base.OnResponse(sender, info);

            switch (info.ButtonID)
            {
                case (int)Buttons.Normal:
                    new WaypointGumpDifficulty(Difficulty.Normal, M).SendTo(sender);
                    break;
                case (int)Buttons.Nightmare:
                    new WaypointGumpDifficulty(Difficulty.Nightmare, M).SendTo(sender);
                    break;
                case (int)Buttons.Hell:
                    new WaypointGumpDifficulty(Difficulty.Hell, M).SendTo(sender);
                    break;
            }
        }
    }

    internal class WaypointGumpDifficulty : Gumps.Gump
    {
        const int BGWIDTH = 183, BGHEIGHT = 500;

        const int START_BUTTON_Y = 10, BUTTON_HEIGHT = 25, BUTTON_SPACING = 3, BUTTON_WIDTH = 126, BUTTON_X = (BGWIDTH >> 1) - (BUTTON_WIDTH >> 1);

        private Difficulty difficulty { get; }
        public PlayerMobile M { get; }

        public WaypointGumpDifficulty(Difficulty difficulty, PlayerMobile m) : base(400, 400)
        {
            AddBackground(0, 0, BGWIDTH, BGHEIGHT, 40000);
            M = m;
            switch (difficulty)
            {
                case Difficulty.Normal:
                    BuildNormal();
                    break;
                case Difficulty.Nightmare:
                    BuildNightmare();
                    break;
                case Difficulty.Hell:
                    BuildHell();
                    break;
            }
            this.difficulty = difficulty;
        }

        private void BuildNormal()
        {
            int y = START_BUTTON_Y;
            int x = BUTTON_X;

            foreach (WayPointEntry entry in WayPointList.Normal.WayPointEntries)
            {
                if (!entry.HasAccess(M))
                {
                    continue;
                }

                if (y + BUTTON_HEIGHT + BUTTON_SPACING > BGHEIGHT)
                {
                    x += BUTTON_WIDTH + 5;
                    y = START_BUTTON_Y;
                }

                AddButton(x, y, 40019, 40019, (int)entry.ButtonID, Gumps.GumpButtonType.Reply, (int)entry.ButtonID);
                AddLabel(x + 3, y + 3, 0, entry.Name);

                y += BUTTON_HEIGHT + BUTTON_SPACING;
            }
        }

        private void BuildNightmare()
        {
            int y = START_BUTTON_Y;
            int x = BUTTON_X;

            foreach (WayPointEntry entry in WayPointList.Nightmare.WayPointEntries)
            {
                if (!entry.HasAccess(M))
                {
                    continue;
                }

                if (y + BUTTON_HEIGHT + BUTTON_SPACING > BGHEIGHT)
                {
                    x += BUTTON_WIDTH + 5;
                    y = START_BUTTON_Y;
                }

                AddButton(x, y, 40019, 40019, (int)entry.ButtonID, Gumps.GumpButtonType.Reply, (int)entry.ButtonID);
                AddLabel(x + 3, y + 3, 0, entry.Name);

                y += BUTTON_HEIGHT + BUTTON_SPACING;
            }
        }

        private void BuildHell()
        {
            int y = START_BUTTON_Y;
            int x = BUTTON_X;

            foreach (WayPointEntry entry in WayPointList.Hell.WayPointEntries)
            {
                if (!entry.HasAccess(M))
                {
                    continue;
                }

                if (y + BUTTON_HEIGHT + BUTTON_SPACING > BGHEIGHT)
                {
                    x += BUTTON_WIDTH + 5;
                    y = START_BUTTON_Y;
                }

                AddButton(x, y, 40019, 40019, (int)entry.ButtonID, Gumps.GumpButtonType.Reply, (int)entry.ButtonID);
                AddLabel(x + 3, y + 3, 0, entry.Name);

                y += BUTTON_HEIGHT + BUTTON_SPACING;
            }
        }

        public override void OnResponse(Network.NetState sender, RelayInfo info)
        {
            base.OnResponse(sender, info);

            WayPointEntry entry = WayPointList.LookUpWaypoint(difficulty, (Buttons)info.ButtonID);
            if (entry != null && sender.Mobile != null)
            {
                Mobile m_Mobile = sender.Mobile;

                if (m_Mobile.Map == entry.Map && m_Mobile.InRange(entry.Location, 1))
                {
                    m_Mobile.SendLocalizedMessage(1019003); // You are already there.
                    return;
                }

                if (m_Mobile.IsStaff())
                {
                    //Staff can always use a gate!
                }
                else if (SpellHelper.CheckCombat(m_Mobile))
                {
                    m_Mobile.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
                    return;
                }
                else if (m_Mobile.Spell != null)
                {
                    m_Mobile.SendLocalizedMessage(1049616); // You are too busy to do that at the moment.
                    return;
                }

                BaseCreature.TeleportPets(m_Mobile, entry.Location, entry.Map);

                m_Mobile.Combatant = null;
                m_Mobile.Warmode = false;
                m_Mobile.Hidden = true;

                m_Mobile.MoveToWorld(entry.Location, entry.Map);

                Effects.PlaySound(entry.Location, entry.Map, 0x1FE);

                CityTradeSystem.OnPublicMoongateUsed(m_Mobile);
            }
        }
    }

    internal class WayPointList
    {
        public static Dictionary<int, WayPointEntry> WayPointCache = new Dictionary<int, WayPointEntry>();

        public static WayPointList Normal = new WayPointList("Normal", new WayPointEntry[]
        {
            new WayPointEntry(1499, 2164, 3, Map.Normal, "Rogue Encampment", Difficulty.Normal, Buttons.RogueEncampment),
            new WayPointEntry(1931, 2227, 3, Map.Normal, "Cold Plains", Difficulty.Normal, Buttons.ColdPlains),
            new WayPointEntry(2271, 2715, 3, Map.Normal, "Stony Field", Difficulty.Normal, Buttons.StonyField),
            new WayPointEntry(2932, 1743, 3, Map.Normal, "Dark Wood", Difficulty.Normal, Buttons.DarkWood),
            new WayPointEntry(3671, 1389, 3, Map.Normal, "Black Marsh", Difficulty.Normal, Buttons.BlackMarsh),
            new WayPointEntry(4249, 1117, 3, Map.Normal, "Outer Cloister", Difficulty.Normal, Buttons.OuterCloister),
            new WayPointEntry(199, 203, 3, Map.Dungeons, "Jail 1", Difficulty.Normal, Buttons.Jail),
            new WayPointEntry(4150, 1108, 3, Map.Normal, "Inner Cloister", Difficulty.Normal, Buttons.InnerCloister),
            new WayPointEntry(332, 323, 3, Map.Dungeons, "Catabombs 2", Difficulty.Normal, Buttons.Catacombs2),
        });

        public static WayPointList Nightmare = new WayPointList("Nightmare", new WayPointEntry[]
        {
            new WayPointEntry(1499, 2164, 3, Map.Nightmare, "Rogue Encampment", Difficulty.Nightmare, Buttons.RogueEncampment),
            new WayPointEntry(1931, 2227, 3, Map.Nightmare, "Cold Plains", Difficulty.Nightmare, Buttons.ColdPlains),
            new WayPointEntry(2271, 2715, 3, Map.Nightmare, "Stony Field", Difficulty.Nightmare, Buttons.StonyField),
            new WayPointEntry(2932, 1743, 3, Map.Nightmare, "Dark Wood", Difficulty.Nightmare, Buttons.DarkWood),
            new WayPointEntry(3671, 1389, 3, Map.Nightmare, "Black Marsh", Difficulty.Nightmare, Buttons.BlackMarsh),
            new WayPointEntry(4249, 1117, 3, Map.Nightmare, "Outer Cloister", Difficulty.Nightmare, Buttons.OuterCloister),
            new WayPointEntry(199, 203, 3, Map.Dungeons, "Jail 1", Difficulty.Nightmare, Buttons.Jail),
            new WayPointEntry(4150, 1108, 3, Map.Nightmare, "Inner Cloister", Difficulty.Nightmare, Buttons.InnerCloister),
            new WayPointEntry(332, 323, 3, Map.Dungeons, "Catabombs 2", Difficulty.Nightmare, Buttons.Catacombs2),
        });

        public static WayPointList Hell = new WayPointList("Hell", new WayPointEntry[]
        {
            new WayPointEntry(1499, 2164, 3, Map.Hell, "Rogue Encampment", Difficulty.Hell, Buttons.RogueEncampment),
            new WayPointEntry(1931, 2227, 3, Map.Hell, "Cold Plains", Difficulty.Hell, Buttons.ColdPlains),
            new WayPointEntry(2271, 2715, 3, Map.Hell, "Stony Field", Difficulty.Hell, Buttons.StonyField),
            new WayPointEntry(2932, 1743, 3, Map.Hell, "Dark Wood", Difficulty.Hell, Buttons.DarkWood),
            new WayPointEntry(3671, 1389, 3, Map.Hell, "Black Marsh", Difficulty.Hell, Buttons.BlackMarsh),
            new WayPointEntry(4249, 1117, 3, Map.Hell, "Outer Cloister", Difficulty.Hell, Buttons.OuterCloister),
            new WayPointEntry(199, 203, 3, Map.Dungeons, "Jail 1", Difficulty.Hell, Buttons.Jail),
            new WayPointEntry(4150, 1108, 3, Map.Hell, "Inner Cloister", Difficulty.Hell, Buttons.InnerCloister),
            new WayPointEntry(332, 323, 3, Map.Dungeons, "Catabombs 2", Difficulty.Hell, Buttons.Catacombs2),
        });

        public static WayPointEntry LookUpWaypoint(Difficulty difficulty, Buttons button)
        {
            if (WayPointCache.TryGetValue(GetWayPointValue(difficulty, button), out WayPointEntry e))
            {
                return e;
            }

            return null;
        }

        public static int GetWayPointValue(Difficulty difficulty, Buttons button)
        {
            return int.Parse($"{(int)difficulty}{(int)button}");
        }

        public WayPointList(string name, WayPointEntry[] wayPointEntries)
        {
            Name = name;
            WayPointEntries = wayPointEntries;
        }

        public string Name { get; }
        public WayPointEntry[] WayPointEntries { get; }
    }

    internal class WayPointEntry
    {
        public WayPointEntry(int x, int y, int z, Map map, string name, Difficulty difficulty, Buttons buttonID)
        {
            X = x;
            Y = y;
            Z = z;
            Map = map;
            Name = name;
            ButtonID = buttonID;

            WID = WayPointList.GetWayPointValue(difficulty, buttonID);

            WayPointList.WayPointCache.Add(WID, this);
        }

        public bool HasAccess(PlayerMobile m)
        {
            if (m == null)
            {
                return false;
            }

            if (m.AccessLevel > AccessLevel.VIP)
            {
                return true;
            }

            return m.WaypointsActivated.Contains(WID);
        }

        public int WID { get; private set; }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }
        public Map Map { get; }
        public string Name { get; }
        public Buttons ButtonID { get; }
        public Point3D Location { get { return new Point3D(X, Y, Z); } }
    }

    internal class WayPointUtility
    {
        public static void AddWaypointAccess(WayPointEntry e, PlayerMobile m)
        {
            if (e != null && m != null)
            {
                if (!m.WaypointsActivated.Contains(e.WID))
                {
                    m.WaypointsActivated.Add(e.WID);
                    m.SendMessage("Waypoint found!");
                }
            }
        }

        public static void AddWaypointAccess(int wayPointID, PlayerMobile m)
        {
            if (wayPointID > -1 && m != null)
            {
                if (!m.WaypointsActivated.Contains(wayPointID))
                {
                    m.WaypointsActivated.Add(wayPointID);
                    m.SendMessage("Waypoint found!");
                }
            }
        }
    }

    enum Difficulty
    {
        None,
        Normal = 100,
        Nightmare = 200,
        Hell = 300
    }

    enum Buttons
    {
        Invalid,
        Normal,
        Nightmare,
        Hell,


        RogueEncampment,
        ColdPlains,
        StonyField,
        DarkWood,
        BlackMarsh,
        OuterCloister,
        Jail,
        InnerCloister,
        Catacombs2
    }
}
