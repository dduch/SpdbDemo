using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;
using Navigation.DataModels;
using Navigation.DataProviders;

namespace Navigation.Network
{
    class GraphBuilder
    {

        private IGeoDataProvider geoData;

        private TravelMetric metric;

        private double maxFreeDistance;

        public GraphBuilder(IGeoDataProvider geoData, TravelMetric metric)
        {
            this.geoData = geoData;
            this.metric = metric;
            maxFreeDistance = metric.FreeOfChargeTime * metric.Velocity;
        }

        public SuperGraph BuildGraph()
        {
            var subGraphs = BuildSubGraphs();
            var superGraph = BuildSuperGraph(subGraphs);
            return superGraph;
        }

        // Builds subgraphs with 0 travel cost between each 2 vertices
        private List<SubGraph> BuildSubGraphs()
        {
            // Vertices which currently are not part of any graph
            var detachedVertices = geoData.GetStations().ToList();

            // List of constructed subgraphs
            var subGraphs = new List<SubGraph>();

            while (detachedVertices.Count > 0)
            {
                // Create new subgraph and initialize it with starting point
                var G = new SubGraph(metric.Velocity, metric.ChangeTime);
                var p = detachedVertices.Last();
                detachedVertices.RemoveAt(detachedVertices.Count - 1);
                var v = new Vertex(p);
                G.Vertices.Add(v);
                
                // Q contains vertices added to G, for which distance to other vertices needs to be evaluated
                // Stored information is index of v on G vertices list
                var Q = new Queue<int>();
                Q.Enqueue(0);

                while (Q.Count > 0)
                {
                    var vid = Q.Dequeue();
                    v = G.Vertices[vid];
                    p = v.Position;

                    // Firstly, check vertices already added to G
                    foreach (var qvid in Q)
                    {
                        var q = G.Vertices[qvid].Position;
                        var edges = FindFreeRoutes(p, q);
                        if (edges == null)
                            continue;
                        // If edges is not null route from p to q exists
                        v.Neighbors.Add(qvid);
                        v.Archs.Add(edges.Item1);
                        // Check if route from q to p fulfills requirements
                        if (edges.Item2.GetLength() < maxFreeDistance)
                        {
                            var qv = G.Vertices[qvid];
                            qv.Neighbors.Add(vid);
                            qv.Archs.Add(edges.Item2);
                        }
                    }

                    // Secondly, check vertices not added to any graph
                    // Iterate in reverse because we will be removing elements wile iterating
                    for (int i = detachedVertices.Count - 1; i >= 0; --i)
                    {
                        var q = detachedVertices[i];
                        var edges = FindFreeRoutes(p, q);
                        if (edges == null)
                            continue;
                        // If edges is not null route from p to q exists and q should be added to G
                        var u = new Vertex(q);
                        var uid = G.Vertices.Count;
                        G.Vertices.Add(u);
                        Q.Enqueue(uid);
                        v.Neighbors.Add(uid);
                        v.Archs.Add(edges.Item1);
                        if (edges.Item2.GetLength() < maxFreeDistance)
                        {
                            u.Neighbors.Add(vid);
                            u.Archs.Add(edges.Item2);
                        }
                        detachedVertices.RemoveAt(i);
                    }
                }
                subGraphs.Add(G);
            }
            return subGraphs;
        }

        private SuperGraph BuildSuperGraph(List<SubGraph> subGraphs)
        {
            // Create edge between each 2 subgraphs
            for(int i = 0; i < subGraphs.Count; ++i)
                for (int j = i + 1; j < subGraphs.Count; ++j)
                {
                    var Gi = subGraphs[i];
                    var Gj = subGraphs[j];

                    var connections = ConnectGraphs(Gi, Gj);

                    Gi.NeighborGraphs.Add(j);
                    Gj.NeighborGraphs.Add(i);

                    Gi.IntergraphArchs.Add(connections.Item1);
                    Gj.IntergraphArchs.Add(connections.Item2);
                }

            return new SuperGraph(subGraphs, metric);
        }

        private Tuple<IRoute, IRoute> FindFreeRoutes(Point p1, Point p2)
        {
            var d = p1.GetDistanceTo(p2);
            if (d > maxFreeDistance)
                return null;
            var fromP1toP2 = geoData.GetRoute(p1, p2, RouteType.Cycle, maxFreeDistance);
            if (fromP1toP2.GetLength() > maxFreeDistance)
                return null;
            var fromP2toP1 = geoData.GetRoute(p2, p1, RouteType.Cycle, maxFreeDistance);
            return new Tuple<IRoute, IRoute>(fromP1toP2, fromP2toP1);
        }

        private Tuple<IntergraphEdge, IntergraphEdge> ConnectGraphs(SubGraph g1, SubGraph g2)
        {
            var vuDists = new List<Tuple<int, int, double>>();

            for (int i = 0; i < g1.Vertices.Count; ++i)
                for (int j = 0; j < g2.Vertices.Count; ++j)
                    vuDists.Add(new Tuple<int, int, double>(i, j, g1.Vertices[i].Position.GetDistanceTo(g2.Vertices[j].Position)));

            vuDists = vuDists.OrderBy(x => x.Item3).ToList();

            double bestG1G2ConnectionLength = Double.PositiveInfinity;
            double bestG2G1ConnectionLength = Double.PositiveInfinity;

            IntergraphEdge G1G2Connection = null;
            IntergraphEdge G2G1Connection = null;

            bool bestG1G2ConnectionFound = false;
            bool bestG2G1ConnectionFound = false;

            for (int i = 0; i < vuDists.Count; ++i)
            {
                var vu = vuDists[i];

                if (vuDists[i].Item3 > bestG1G2ConnectionLength)
                    bestG1G2ConnectionFound = true;
                else
                {
                    IRoute r = geoData.GetRoute(g1.Vertices[vu.Item1].Position, g2.Vertices[vu.Item2].Position, RouteType.All);
                    if (r.GetLength() < bestG1G2ConnectionLength)
                    {
                        G1G2Connection = new IntergraphEdge(vu.Item1, vu.Item2, r);
                        bestG1G2ConnectionLength = r.GetLength();
                    }
                }

                if (vuDists[i].Item3 > bestG2G1ConnectionLength)
                    bestG2G1ConnectionFound = true;
                else
                {
                    IRoute r = geoData.GetRoute(g2.Vertices[vu.Item2].Position, g1.Vertices[vu.Item1].Position, RouteType.All);
                    if (r.GetLength() < bestG2G1ConnectionLength)
                    {
                        G2G1Connection = new IntergraphEdge(vu.Item2, vu.Item1, r);
                        bestG2G1ConnectionLength = r.GetLength();
                    }
                }

                if (bestG1G2ConnectionFound && bestG2G1ConnectionFound)
                    break;
            }

            return new Tuple<IntergraphEdge,IntergraphEdge>(G1G2Connection, G2G1Connection);
        }
    }
}
