using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;
using Navigation.Graph;
using Navigation.DataModels;
using Navigation.DataProviders;
using Navigation.Network;

namespace Navigation
{
    public class NavigationResolver : INavigationResolver
    {
        //private static volatile SuperGraph _graph = null;
        //private static object syncRoot = new Object();

        //private static SuperGraph GetGraph(TravelMetric builderMetric, IGeoDataProvider builderGeoData)
        //{
        //    if (_graph == null)
        //        lock (syncRoot)
        //        {
        //            if (_graph == null)
        //            {
        //                var builder = new GraphBuilder(builderGeoData, builderMetric);
        //                _graph = builder.BuildGraph();
        //            }
        //        }
        //    return _graph;
        //}

        private IGeoDataProvider geoData;

        // Constructor for tests
        public NavigationResolver(IGeoDataProvider geoData)
        {
            this.geoData = geoData;
        }

        public IRoute GetBestRoute(Point source, Point destination, double avgSpeed = 15.0)
        {
            var metric = new TravelMetric(avgSpeed);
            var builder = new GraphBuilder(geoData, metric);
            var graph = builder.BuildGraph();
            return GetBestRouteInSuperGraph(graph, source, destination);
        }

        private IRoute GetBestRouteInSuperGraph(SuperGraph graph, Point source, Point destination)
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
                route.Append(GetBestRouteInSubGraph(graph.SubGraphs[gsrc], vsrc, vdst));
            }
            // Ddestination and source in different subgraphs
            else
            {
                var superPath = Dijkstra.FindBestPath(graph, gsrc, gdst);

                var vfrom = vsrc;

                for (int i = 0; i < superPath.Count - 1; ++i)
                {
                    var gCur = superPath[i];
                    var gNxt = superPath[i + 1];
                    var archToNxtId = graph.SubGraphs[gCur].NeighborGraphs.FindIndex(gid => gid == gNxt);
                    var archToNxt = graph.SubGraphs[gCur].IntergraphArchs[archToNxtId];
                    var vto = archToNxt.StartVertex;

                    route.Append(GetBestRouteInSubGraph(graph.SubGraphs[gCur], vfrom, vto));

                    route.Append(archToNxt.Arch);
                    vfrom = archToNxt.EndVertex;
                }

                var gLast = superPath[superPath.Count - 1];
                route.Append(GetBestRouteInSubGraph(graph.SubGraphs[gLast], vfrom, vdst));
            }

            route.Append(endingRoute);

            return route;
        }

        private IRoute GetBestRouteInSubGraph(SubGraph g, int vsrc, int vdst)
        {
            var path = Dijkstra.FindBestPath(g, vsrc, vdst);
            var route = new Route(new List<Point>() { g.Vertices[path.First()].Position });

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
