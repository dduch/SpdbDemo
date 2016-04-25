using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;
using Navigation.Graph;
using Navigation.DataModels;
using Navigation.DataProviders;

namespace Navigation
{
    public class NavigationResolver : INavigationResolver
    {

        private IGeoDataProvider geoData;

        // Constructor for tests
        public NavigationResolver(IGeoDataProvider geoData)
        {
            this.geoData = geoData;
        }

        public IRoute GetBestRoute(Point source, Point destination, double avgSpeed = 15.0)
        {
            var metric = new TravelMetric(avgSpeed);

            var net = new Network(geoData, metric);

            var startStation = geoData.GetNearestStation(source);
            var endStation = geoData.GetNearestStation(destination);
            var startNode = net.MapStationToNode(startStation);
            var endNode = net.MapStationToNode(endStation);

            var path = Dijkstra.FindBestPath(net, startNode, endNode);
            var waypoints = path.Select(nodeId => net.MapNodeToPoint(nodeId)).ToList();
            // Ad start and end points
            waypoints.Insert(0, source);
            waypoints.Add(destination);

            return ConstructRoute(waypoints);
        }

        private IRoute ConstructRoute(List<Point> waypoints)
        {
            IRoute route = new Route(new List<Point>());
            for(int i = 0; i < waypoints.Count - 1; ++i)
                route.Append(geoData.GetRoute(waypoints[i], waypoints[i + 1]));

            return route;
        }
    }
}
