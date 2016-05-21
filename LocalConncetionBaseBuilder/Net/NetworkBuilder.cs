using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;
using INavigation;
using Navigation.DataModels;
using System.IO;

namespace LocalConncetionBaseBuilder.Net
{
    class Way
    {
        public List<long> Nodes { get; set; }
        public ArchType Type { get; set; }
        public Way(List<long> nodes, ArchType type)
        {
            Nodes = nodes;
            Type = type;
        }
    }

    // Not thread safe
    class NetworkBuilder
    {
        private static readonly Dictionary<string, ArchType> _highwaysDictionary = new Dictionary<string, ArchType>()
        {
            {"trunk",ArchType.MOTOR},
            {"primary",ArchType.MOTOR},
            {"secondary",ArchType.MOTOR},
            {"tertiary",ArchType.MOTOR},
            {"unclassified",ArchType.MOTOR},
            {"residential",ArchType.MOTOR},
            {"service",ArchType.MOTOR},
            {"trunk_link",ArchType.MOTOR},
            {"primary_link",ArchType.MOTOR},
            {"secondary_link",ArchType.MOTOR},
            {"tertiary_link",ArchType.MOTOR},
            {"living_street",ArchType.FOOT},
            {"pedestrian",ArchType.FOOT},
            {"track",ArchType.OTHER},
            {"road",ArchType.OTHER},
            {"footway",ArchType.FOOT},
            {"path",ArchType.OTHER},
            {"cycleway",ArchType.BICYCLE},
        };

        private List<Way> ExtractWays(IEnumerable<XElement> allWays)
        {
            var ways = new List<Way>(allWays.Count());

            foreach (var way in allWays)
            {
                var tags = way.Elements("tag");
                var valid = false;
                var type = ArchType.OTHER;
                var direction = 0;
                var bikeDirection = 0;
                var directionOverriden = false;

                // Check if way is ok basing on contained tags
                foreach (var tag in tags)
                {
                    var key = tag.Attribute("k").Value;
                    var value = tag.Attribute("v").Value;

                    if (key == "bicycle" && value == "no")
                    {
                        valid = false;
                        break; // No bicycles allowed on this road anyway
                    }
                    else if (key == "cycleway")
                    {
                        valid = true;
                        type = ArchType.BICYCLE;
                    }
                    else if (key == "highway")
                    {
                        if(_highwaysDictionary.ContainsKey(value))
                        {
                            valid = true;
                            type = _highwaysDictionary[value];
                        }
                    }
                    else if (key == "junction")
                    {
                        valid = true;
                        type = ArchType.MOTOR;
                        directionOverriden = true;
                        bikeDirection = 1;
                    }
                    else if (key == "oneway")
                    {
                        if (value == "yes")
                            direction = 1;
                        else if (value == "no")
                            direction = 0;
                        else if (value == "-1")
                            direction = -1;
                    }
                    else if (key == "oneway:bicycle")
                    {
                        if (value == "yes")
                            bikeDirection = 1;
                        else if (value == "no")
                            bikeDirection = 0;
                        else if (value == "-1")
                            bikeDirection = -1;

                        directionOverriden = true;
                    }
                }

                if (valid)
                {
                    var nodes = way.Elements("nd").Select(ndref => Convert.ToInt64(ndref.Attribute("ref").Value)).ToList();
                    var dir = directionOverriden ? bikeDirection : direction;
                    if (nodes.Count > 0)
                    {
                        if(dir >= 0)
                            ways.Add(new Way(nodes, type));
                        if(dir <= 0)
                        {
                            var reversed = new List<long>(nodes);
                            reversed.Reverse();
                            ways.Add(new Way(reversed, type));
                        }
                            
                    }
                }
            }

            return ways;
        }

        private List<Way> SplitWays(List<Way> originalWays, IEnumerable<long> allNodes)
        {
            var nodesDegrees = new Dictionary<long, int>(allNodes.Count());
            foreach (var node in allNodes)
                nodesDegrees.Add(node, 0);

            // Compute nodes degrees
            for(int i = 0; i < originalWays.Count; ++i)
            {
                var waynodes = originalWays[i].Nodes;
                for (int j = 0; j < waynodes.Count; ++j)
                    ++nodesDegrees[waynodes[j]];
            }

            // Split ways
            List<Way> splittedWays = new List<Way>(originalWays.Count * 2); // Heuristic prediction of how many ways we can get
            for (int i = 0; i < originalWays.Count; ++i)
            {
                var waynodes = originalWays[i].Nodes;
                int split = 0;
                for (int j = 1; j < waynodes.Count; ++j)
                {
                    if(nodesDegrees[waynodes[j]] > 1 || j == waynodes.Count - 1)
                    {
                        var subnodes = waynodes.GetRange(split, j - split + 1);
                        split = j;
                        splittedWays.Add(new Way(subnodes, originalWays[i].Type));
                    }
                }                  
            }

            return splittedWays;
        }

        HashSet<int> FindReachableNodes(List<Node> nodes, int seed)
        {
            HashSet<int> expandedNodes = new HashSet<int>();
            Queue<int> nodesToExpand = new Queue<int>();

            nodesToExpand.Enqueue(seed);

            while (nodesToExpand.Count > 0)
            {
                var current = nodesToExpand.Dequeue();
                if (expandedNodes.Add(current))
                {
                    foreach (var archFromCurrent in nodes[current].Archs)
                    {
                        nodesToExpand.Enqueue(archFromCurrent.To);
                    }
                }
            }

            return expandedNodes;
        }

        public Network BuildNetworkFromXml(string xmlFileName)
        {
            // TODO: Validation if point is accessible by bike

            // This part may seem complicated but optimizations were required

            // Parse xml file:
            XElement xmlData = XElement.Load(new StreamReader(xmlFileName));

            // Extract from xml all way elements that are part of ways network
            // and convert them to Way class objects colection
            var ways = ExtractWays(xmlData.Elements("way"));

            // From ways extract all points ids they contain and create look-up table
            // for speeding up checking if particular point is part of ways network
            var allPointsIdsLut = new HashSet<long>(ways.SelectMany(w => w.Nodes).Distinct());

            // Split ways, so that except start and endpoint of way there is no connections 
            // to other ways
            ways = SplitWays(ways, allPointsIdsLut);

            // Extract all points that are part of ways network
            var allPointsTmp = xmlData.Elements("node")
                .Where(n => allPointsIdsLut.Contains(Convert.ToInt64(n.Attribute("id").Value)))
                .Select(n => new Tuple<long,double,double>(
                    Convert.ToInt64(n.Attribute("id").Value),
                    Convert.ToDouble(n.Attribute("lat").Value, CultureInfo.InvariantCulture),
                    Convert.ToDouble(n.Attribute("lon").Value, CultureInfo.InvariantCulture)
                    ))
                .ToArray();

            // These are no longer needed and probably consume a bit of memory
            allPointsIdsLut = null;
            xmlData = null;

            List<Point> allPoints = new List<Point>(allPointsTmp.Length); // List of points being part of ways network
            Dictionary<long, int> pointsMap = new Dictionary<long, int>(allPointsTmp.Length); // Maps point id to index in allPoints list

            // Fill allPoints and pointsMap with data from allPointsTmp
            for (int i = 0; i < allPointsTmp.Length; ++i)
            {
                var nodeElem = allPointsTmp[i];
                var id = nodeElem.Item1;
                var lat = nodeElem.Item2;
                var lon = nodeElem.Item3;
                pointsMap.Add(id, i);
                allPoints.Add(new Point(lat, lon));
            }

            // This is no longer needed and probably consumes a bit of memory
            allPointsTmp = null;
            
            // Extract all nodes ids
            var nodesIds = ways.SelectMany(w => new long[] { w.Nodes.First(), w.Nodes.Last() }).Distinct().ToList();

            // Build nodes list
            List<Node> nodes = new List<Node>(nodesIds.Count); // List of nodes being part of ways network
            Dictionary<long, int> nodesMap = new Dictionary<long, int>(nodesIds.Count); // Maps point id to index in nodes list
            for (int i = 0; i < nodesIds.Count; ++i)
            {
                var nid = nodesIds[i];
                var point = allPoints[pointsMap[nid]];
                nodes.Add(new Node(point));
                nodesMap.Add(nid, i);
            }

            // Add archs to nodes
            for (int i = 0; i < ways.Count; ++i)
            {
                var way = ways[i];
                var srcNodeId = nodesMap[way.Nodes.First()];
                var dstNodeId = nodesMap[way.Nodes.Last()];
                var waypoints = way.Nodes.Select(nid =>  allPoints[pointsMap[nid]]).ToList();
                nodes[srcNodeId].AddArch(dstNodeId, way.Type, new Route(waypoints));
            }

            Console.WriteLine("All ways count = " + ways.Count);
            Console.WriteLine("All points count = " + allPoints.Count);
            Console.WriteLine("All nodes count = " + nodesIds.Count);

            // Free memory
            nodesMap = null;
            nodesIds = null;
            allPoints = null;
            ways = null;

            // Integrity check
            int bestSeed = 2770;
            HashSet<int> reachableNodes = FindReachableNodes(nodes, bestSeed);
            //int bestSeed = 0;
            //HashSet<int> reachableNodes = new HashSet<int>();
            //for (int seed = 0; seed < nodes.Count; ++seed)
            //{
            //    var reachableFromSeed = FindReachableNodes(nodes, seed);
            //    if (reachableFromSeed.Count > reachableNodes.Count)
            //    {
            //        reachableNodes = reachableFromSeed;
            //        bestSeed = seed;
            //    }
            //}

            // Up to this point expanded nodes contain all nodes reachable from seed
            double reachableToAll = 100.0 * reachableNodes.Count / nodes.Count;
            Console.WriteLine(reachableNodes.Count + " nodes reachable from " + bestSeed + " (" + reachableToAll + "%)");

            Dictionary<int, int> transformationMap = new Dictionary<int, int>(reachableNodes.Count);
            int ndid = 0;
            foreach(var node in reachableNodes)
            {
                transformationMap.Add(node, ndid++);
            }

            Node[] transformedNodes = new Node[reachableNodes.Count];
            for(int i = 0; i < nodes.Count; ++i)
            {
                if(reachableNodes.Contains(i))
                {
                    var transformedArchs = nodes[i].Archs.Where(a => reachableNodes.Contains(a.To)).ToList();
                    foreach(var arch in transformedArchs)
                    {
                        arch.To = transformationMap[arch.To];
                    }
                    nodes[i].Archs = transformedArchs;
                    transformedNodes[transformationMap[i]] = nodes[i];
                }
            }

            var net = new Network(transformedNodes.ToList());

            return net;
        }


    }
}
