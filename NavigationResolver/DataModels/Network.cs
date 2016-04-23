using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationResolver.Algorithms;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;

namespace NavigationResolver.DataModels
{
    public sealed class Network : INetwork
    {
        private static volatile Network instance;
        private static object syncRoot = new Object();

        private SuperGraph graph;
        private IGeoDataProvider geoData;
        private TravelMetric metric;

        private Network(SuperGraph graph, IGeoDataProvider geoData, TravelMetric metric)
        {
            this.graph = graph;
            this.geoData = geoData;
            this.metric = metric;       
        }

        // Creates new instance of network with given parameters.
        public static INetwork Build(IGeoDataProvider geoData, TravelMetric metric)
        {
            var builder = new NetworkBuilder(geoData, metric);
            var subGraphs = builder.BuildSubGraphs();
            var superGraph = builder.BuildSuperGraph(subGraphs);
            return new Network(superGraph, geoData, metric);
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
            IRoute route = new Route(new List<Point>());

            var startingRoute = geoData.GetRouteToNearestStation(source, true);
            var endingRoute = geoData.GetRouteToNearestStation(destination, false);

            var gsrcAndVsrc = graph.GetVertexByPosition(startingRoute.GetPoints().Last());
            var gdstAndVdst = graph.GetVertexByPosition(endingRoute.GetPoints().First());

            int gsrc = gsrcAndVsrc.Item1, gdst = gdstAndVdst.Item1, vsrc = gsrcAndVsrc.Item2, vdst = gdstAndVdst.Item2;

            route.Append(startingRoute);

            // Both destination and source in the same subgraph
            if (gsrc == gdst)
            {
                route.Append(GetBestRouteInSubGraph(gsrc, vsrc, vdst));
            }
            // Ddestination and source in different subgraphs
            else
            {
                var superPath = Dijkstra.FindBestPath(graph, gsrc, gdst);

                var vfrom = vsrc;

                for(int i = 0; i < superPath.Count - 1; ++i)
                {
                    var gCur = superPath[i];
                    var gNxt = superPath[i+1];
                    var archToNxtId = graph.SubGraphs[gCur].NeighborGraphs.FindIndex(gid => gid == gNxt);
                    var archToNxt = graph.SubGraphs[gCur].IntergraphArchs[archToNxtId];
                    var vto = archToNxt.StartVertex;
                    
                    route.Append(GetBestRouteInSubGraph(gCur, vfrom, vto));

                    route.Append(archToNxt.Arch);
                    vfrom = archToNxt.EndVertex;
                }

                var gLast = superPath[superPath.Count - 1];
                route.Append(GetBestRouteInSubGraph(gLast, vfrom, vdst));
            }

            route.Append(endingRoute);

            return route;
        }

        private IRoute GetBestRouteInSubGraph(int gid, int vsrc, int vdst)
        {
            var g = graph.SubGraphs[gid];
            var path = Dijkstra.FindBestPath(g, vsrc, vdst);
            var route = new Route(new List<Point>() { g.Vertices[path.First()].Position});

            for (int i = 1; i < path.Count; ++i)
            {
                var from = path[i - 1];
                var to = path[i];
                var archId = g.Vertices[from].Neighbors.FindIndex(id => id == to);
                route.Append(g.Vertices[from].Archs[archId]);
            }

            return route;
        }
    }
}
