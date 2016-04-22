using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;

namespace NavigationResolver.DataModels
{
    public class NetworkBuilder
    {
        private IGeoDataProvider geoData;

        private TravelMetric metric;

        private double maxFreeDistance;

        public NetworkBuilder(IGeoDataProvider geoData, TravelMetric metric)
        {
            this.geoData = geoData;
            this.metric = metric;
            maxFreeDistance = metric.FreeOfChargeTime * metric.Velocity;
        }

        // Builds subgraphs of with 0 travel cost between each 2 vertices
        public List<SubGraph> BuildSubGraphs()
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
    }
}
