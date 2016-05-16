using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Globalization;
using INavigation;
using System.IO;

namespace LocalConncetionBaseBuilder.Net
{
    class Way
    {
        public long[] Nodes { get; set; }
        public ArchType Type { get; set; }
        public int Direction { get; set; }
        public Way(long[] nodes, ArchType type, int direction)
        {
            Nodes = nodes;
            Type = type;
            Direction = direction;
        }
    }

    // Not thread safe
    class NetworkBuilder
    {
        private XElement _xmlData;
        private List<Node> _nodes;
        private Dictionary<long,int> _nodesMap;

        private List<Way> ExtractWays(IEnumerable<XElement> allWays)
        {
            var ways = new List<Way>(allWays.Count());

            string[] validHighways = new string[]
            {
                "trunk",
                "primary",
                "secondary",
                "tertiary",
                "unclassified",
                "residential",
                "trunk_link",
                "primary_link",
                "secondary_link",
                "tertiary_link",
                "living_street",
                "pedestrian",
                "track",
                "road",
                "footway",
                "path",
                "cycleway"
            };

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
                        break;
                    }
                    else if ((key == "highway" && value == "cycleway") || key == "cycleway")
                    {
                        valid = true;
                        type = ArchType.BICYCLE;
                    }
                    else if (key == "junction")
                    {
                        valid = true;
                    }
                    else if (key == "highway")
                    {
                        if (validHighways.Contains(value))
                            valid = true;
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
                    // TODO: It is possible that some of the nodes will have -1 id
                    var nodes = way.Elements("nd").Select(ndref => Convert.ToInt64(ndref.Attribute("ref").Value)).ToArray();
                    var dir = directionOverriden ? bikeDirection : direction;
                    if (nodes.Length > 1)
                        ways.Add(new Way(nodes, type, dir));
                }
            }

            return ways;
        }

        private void CreateArch(long from, long to, ArchType type)
        {
            int fromIdx = _nodesMap[from];
            int toIdx = _nodesMap[to];
            Node fromNode = _nodes[fromIdx];
            Node toNode = _nodes[toIdx];
            fromNode.AddArch(toIdx, type);
        }

        public Network BuildNetworkFromXml(string xmlFileName)
        {
            // TODO: Validation if point is accessible by bike

            // This part may seem complicated but optimizations were required

            // Parse xml file:
            _xmlData = XElement.Load(new StreamReader(xmlFileName));

            // Extract from xml all way elements that are part of ways network
            // and convert them to Way class objects colection
            var ways = ExtractWays(_xmlData.Elements("way"));

            // From ways extract all nodes ids they contain and create look-up table
            // for speeding up checking if particular node is part of ways network
            var allNodesIdsLut = new HashSet<long>(ways.SelectMany(w => w.Nodes).Distinct());

            // Extract all nodes that are part of ways network
            var allNodes = _xmlData.Elements("node")
                .Where(n => allNodesIdsLut.Contains(Convert.ToInt64(n.Attribute("id").Value)))
                .Select(n => new Tuple<long,double,double>(
                    Convert.ToInt64(n.Attribute("id").Value),
                    Convert.ToDouble(n.Attribute("lat").Value, CultureInfo.InvariantCulture),
                    Convert.ToDouble(n.Attribute("lon").Value, CultureInfo.InvariantCulture)
                    ))
                .ToArray();

            // These are no longer needed and probably consume a bit of memory
            allNodesIdsLut = null;
            _xmlData = null;

            _nodes = new List<Node>(allNodes.Length); // List of nodes being part of ways network
            _nodesMap = new Dictionary<long, int>(allNodes.Length); // Maps node id to index in _nodes list

            // Fill _nodes and _nodesMap with data from allNodes
            for (int i = 0; i < allNodes.Length; ++i)
            {
                var nodeElem = allNodes[i];
                var id = nodeElem.Item1;
                var lat = nodeElem.Item2;
                var lon = nodeElem.Item3;
                _nodesMap.Add(id, i);
                _nodes.Add(new Node(new Point(lat, lon)));
            }

            // This is no longer needed and probably consumes a bit of memory
            allNodes = null;

            for (int i = 0; i < ways.Count; ++i)
            {
                var way = ways[i];

                if (way.Direction >= 0)
                {
                    for(int j = 0; j < way.Nodes.Length - 1; ++j)
                    {
                        CreateArch(way.Nodes[j], way.Nodes[j + 1], way.Type);
                    }
                }
                if (way.Direction <= 0)
                {
                    for (int j = way.Nodes.Length - 1; j > 0; --j)
                    {
                        CreateArch(way.Nodes[j], way.Nodes[j - 1], way.Type);
                    }
                }
            }

            var net = new Network(_nodes);

            _nodes = null;
            _nodesMap = null;

            return net;
        }


    }
}
