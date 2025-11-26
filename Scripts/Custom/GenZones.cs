using Server.Commands;
using Server.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Server.Custom
{
    public class GenZones
    {
        private static string SaveFileLocation { get { return ""; } } //Defaults to saving inside your main servuo directory

        //Usage
        //Type [genzones in game
        //This will go through all your registered maps and the regions registerd to those maps and create matching zones files in the SaveFileLocation directory
        //Customize which maps to process in ProcessMap method
        //Customize which regions to process in ProcessRegion method
        public static void Initialize()
        {
            CommandSystem.Register("genzones", AccessLevel.GameMaster, OnGenZones);
        }

        private static void OnGenZones(CommandEventArgs e)
        {
            try
            {
                if (Map.AllMaps == null)
                {
                    e.Mobile.SendMessage("Maps list is apparantly null.");
                    return;
                }

                if (Region.Regions == null)
                {
                    e.Mobile.SendMessage("Regions list is apparantly null.");
                    return;
                }
                foreach (Map map in Map.AllMaps)
                {
                    if (!ProcessMap(map))
                    {
                        continue;
                    }

                    ZonesFile zonesFile = new ZonesFile();
                    zonesFile.MapIndex = map.MapIndex;

                    foreach (var region in map.Regions)
                    {
                        if (!ProcessRegion(region.Value, map))
                        {
                            continue;
                        }

                        var r = region.Value;

                        ZonesFileZoneData zone = new ZonesFileZoneData();

                        zone.Label = r.Name;

                        // CUO supports red, green, blue, purple, black, yellow, white, or none
                        zone.Color = "white";

                        List<Point2D> polygon = ConvertRectanglesToPolygon(Convert3Dto2D(region.Value.Area));
                        List<int[]> points = new List<int[]>();
                        foreach (var point in polygon)
                        {
                            if (point != Point2D.Zero)
                            {
                                points.Add(new int[] { point.X, point.Y });
                            }
                        }
                        polygon = PointsToPolygons(points);

                        foreach (var point in polygon)
                        {
                            if (point != Point2D.Zero)
                            {
                                zone.Polygon.Add(new List<int> { point.X, point.Y });
                            }
                        }

                        //Don't add this zone if there are no polygons
                        if (zone.Polygon.Count <= 0)
                        {
                            continue;
                        }

                        zonesFile.Zones.Add(zone);
                    }

                    //Don't save this zones file if there are no zones
                    if (zonesFile.Zones.Count > 0)
                    {
                        SaveFile(zonesFile, map);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static bool ProcessMap(Map m)
        {
            if (m == null)
            {
                return false;
            }

            if (m.Regions == null || m == Map.Internal)
            {
                return false;
            }

            return true;
        }

        private static bool ProcessRegion(Region r, Map m)
        {
            if (r == null || r is HouseRegion)
            {
                return false;
            }

            return true;
        }

        private static void SaveFile(ZonesFile zonesFile, Map map)
        {
            string fname = (map.Name == "" ? map.MapIndex.ToString() : map.Name);
            File.WriteAllText(Path.Combine(SaveFileLocation, $"{fname}.zones.json"), zonesFile.ToJsonString());
        }

        private static List<Rectangle2D> Convert3Dto2D(Rectangle3D[] list)
        {
            List<Rectangle2D> Rect2D = new List<Rectangle2D>();

            foreach (Rectangle3D rect in list)
            {
                Rect2D.Add(new Rectangle2D(rect.Start.X, rect.Start.Y, rect.Width, rect.Height));
            }

            return Rect2D;
        }

        private static List<Point2D> ConvertRectanglesToPolygon(List<Rectangle2D> rectangles)
        {
            List<Point2D> polygonPoints = new List<Point2D>();

            foreach (var rectangle in rectangles)
            {
                // Add the four corners of each rectangle to the polygon points
                polygonPoints.Add(new Point2D(rectangle.X, rectangle.Y));
                polygonPoints.Add(new Point2D(rectangle.X + rectangle.Width, rectangle.Y));
                polygonPoints.Add(new Point2D(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height));
                polygonPoints.Add(new Point2D(rectangle.X, rectangle.Y + rectangle.Height));
            }

            return polygonPoints;
        }

        internal class ZonesFileZoneData
        {
            public string Label { get; set; } = "";

            public string Color { get; set; } = "";

            public List<List<int>> Polygon { get; set; } = new List<List<int>>();
        }

        internal class ZonesFile
        {
            public int MapIndex { get; set; }
            public List<ZonesFileZoneData> Zones { get; set; } = new List<ZonesFileZoneData>();

            public string ToJsonString()
            {
                string json = "{";
                json += "\"MapIndex\":" + MapIndex + ",";
                json += "\"Zones\":[";

                foreach (var zone in Zones)
                {
                    json += "{";
                    json += "\"Label\":\"" + zone.Label + "\",";
                    json += "\"Color\":\"" + zone.Color + "\",";
                    json += "\"Polygon\":[";

                    for (int i = 0; i < zone.Polygon.Count; i++)
                    {
                        for (int j = 0; j < zone.Polygon[i].Count; j++)
                        {
                            if (zone.Polygon[i].Count > j + 1)
                            {
                                json += "[";
                                json += zone.Polygon[i][j] + ",";
                                json += zone.Polygon[i][j + 1];
                                json += "],";
                            }
                            j++;//Because we are consuming 2 points
                        }
                    }

                    // Remove the trailing comma
                    if (zone.Polygon.Count > 0)
                        json = json.TrimEnd(',');

                    json += "]},";
                }

                // Remove the trailing comma
                if (Zones.Count > 0)
                    json = json.TrimEnd(',');

                json += "]}";
                return json;
            }
        }

        private static void SetEdges(Dictionary<int[], int[]> edges, Comparison<int[]> cmp, int e, ref List<int[]> points)
        {
            points.Sort(cmp);
            int edgeIndex = 0;
            int length = points.Count;

            while (edgeIndex < length)
            {
                int[] curr = points[edgeIndex];
                do
                {
                    edges[points[edgeIndex]] = points[edgeIndex + 1];
                    edges[points[edgeIndex + 1]] = points[edgeIndex];
                    edgeIndex += 2;
                } while (edgeIndex < length && points[edgeIndex][e] == curr[e]);
            }
        }
        private static List<Point2D> PointsToPolygons(List<int[]> points)
        {
            Dictionary<int[], int[]> edgesV = new Dictionary<int[], int[]>(new ArrayComparer<int>());
            Dictionary<int[], int[]> edgesH = new Dictionary<int[], int[]>(new ArrayComparer<int>());


            SetEdges(edgesV, CompareXThenY, 0, ref points);
            SetEdges(edgesH, CompareYThenX, 1, ref points);

            List<Point2D> polygons = new List<Point2D>();
            while (edgesH.Any())
            {
                var key = edgesH.Keys.First();
                edgesH.Remove(key);

                var firstVertex = new V2(key);
                var previous = new Tuple<int[], int>(firstVertex.ToArray(), 0);
                var vertices = new List<V2> { firstVertex };

                while (true)
                {
                    var edgeIndex = previous.Item1;
                    var edge = previous.Item2;
                    var edges = edge == 0 ? edgesV : edgesH;
                    var nextVertex = new V2(edges[edgeIndex]);
                    var next = new Tuple<int[], int>(nextVertex.ToArray(), 1 - edge);
                    edges.Remove(edgeIndex);

                    if (firstVertex.Compare(nextVertex))
                    {
                        break;
                    }

                    vertices.Add(nextVertex);
                    previous = next;
                }

                foreach (var vertex in vertices)
                {
                    polygons.Add(vertex.ToPoint());

                    var edgeIndex = vertex.ToArray();
                    edgesV.Remove(edgeIndex);
                    edgesH.Remove(edgeIndex);
                }

            }

            return polygons.Distinct().ToList();
        }

        private static Comparison<int[]> CompareXThenY = (a, b) => a[0] < b[0] || (a[0] == b[0] && a[1] < b[1]) ? -1 : 1;
        private static Comparison<int[]> CompareYThenX = (a, b) => a[1] < b[1] || (a[1] == b[1] && a[0] < b[0]) ? -1 : 1;


        internal class V2
        {
            public int X { get; }
            public int Y { get; }

            public V2(int x, int y)
            {
                X = x;
                Y = y;
            }

            public V2(int[] array) : this(array[0], array[1])
            {
            }

            public Point2D ToPoint()
            {
                return new Point2D(X, Y);
            }

            public V2 Scale(int scale)
            {
                return new V2(X * scale, Y * scale);
            }

            public bool Compare(V2 v)
            {
                return X == v.X && Y == v.Y;
            }

            public int[] ToArray()
            {
                return new[] { X, Y };
            }
        }

        internal class ArrayComparer<T> : IEqualityComparer<T[]>
        {
            public bool Equals(T[] x, T[] y)
            {
                return x.SequenceEqual(y);
            }

            public int GetHashCode(T[] obj)
            {
                return obj.Aggregate(17, (hash, item) => hash * 31 + item.GetHashCode());
            }
        }
    }
}
