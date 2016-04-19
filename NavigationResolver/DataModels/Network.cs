using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;

namespace NavigationResolver.DataModels
{
    public sealed class Network : INetwork
    {
        private static volatile Network instance;
        private static object syncRoot = new Object();

        private SuperGraph graph;

        private Network(IGeoDataProvider geoData, TravelMetric metric)
        {

            var stationsToCheck = geoData.GetStations().ToList();
            var subGraphs = new List<SubGraph>();
            var maxFreeDist = metric.FreeOfChargeTime * metric.Velocity;

            // 1) Build subgraphs of with free travel cost

            while (stationsToCheck.Count > 0)
            {
                // Variables initialization:
                var v = stationsToCheck.First();
                var G = new SubGraph(metric.Velocity, metric.ChangeTime);
                stationsToCheck.RemoveAt(0);
                // Q contains vertices of G, for which distance to other vertices needs to be evaluated
                // Stored information is: 
                // 1. index of v in G vertices list
                // 2. current index in stationsToCheck
                var Q = new Queue<Point>();
                Q.Enqueue(v);
                
                // This loop is work in progress
                // It has many issues and does not work for now
                // TODO: Finish it
                while(Q.Count > 0)
                {
                    v = Q.Dequeue();

                    var vertex = new Vertex(v);

                    for (int i = 0; i < stationsToCheck.Count; ++i)
                    {
                        var u = stationsToCheck[i];
                        var directDistance = v.GetDistanceTo(u);
                        if (directDistance > maxFreeDist)
                            continue;

                        var arch = geoData.GetRoute(v, u, RouteType.Cycle, maxFreeDist);
                        if (arch.GetLength() > maxFreeDist)
                            continue; // TODO: We will probably be searching for this route once more if geoData has no cash. Needs to rewrite this later.

                        var revArch = geoData.GetRoute(u, v, RouteType.Cycle, maxFreeDist);
                        if (revArch.GetLength() < maxFreeDist)
                        {
                            // u and v are both in G for sure
                            vertex.Neighbors.Add(G.Vertices.Count + i + 1);
                            vertex.Archs.Add(arch);
                        }
                        else
                        {
                            // Problematic situation:
                            // u may belong to G, but also may not
                            // TODO: Add handling such cases
                        }

                        G.Vertices.Add(vertex);
                    }

                }

                subGraphs.Add(G);

            }

            // 2) Build connections between subgraphs

            // foreach g,h in G 
            //      e = create_edge_of_lowest_cost(g, h);
            //      g.NeighborGraphs.Add(h);
            //      g.IntergraphArchs.Add(e);

            graph = new SuperGraph(subGraphs, metric);

            
        }

        // Creates new instance of network with given parameters.
        public static INetwork Build(IGeoDataProvider geoData, TravelMetric metric)
        {
            return new Network(geoData, metric);
        }

        // Gets existing instance of network
        // or creates new one with given parameters if no exists.
        // Locking prevents from unintentional creation of multiple default insances.
        public static INetwork Get(IGeoDataProvider geoData, TravelMetric metric)
        {
            if (instance == null)
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = Build(geoData, metric) as Network;
                }
            return instance;
        }

        public IRoute GetBestRoute(Point source, Point destination)
        {
            throw new NotImplementedException();
        }
    }
}
