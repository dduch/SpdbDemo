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

        public NavigationResult GetBestRoute(Point source, Point destination, double avgSpeed = 15.0)
        {
            var metric = new TravelMetric(avgSpeed);

            var net = new Network(geoData, metric);

            var startStation = geoData.GetNearestStation(source, true);
            var endStation = geoData.GetNearestStation(destination);
            // For efficiency of graph algorithms we map stations numbers to indexes in nodes array
            var startNode = net.MapStationToNode(startStation);
            var endNode = net.MapStationToNode(endStation);

            var path = Dijkstra.FindBestPath(net, startNode, endNode);
            var stations = path.Select(nodeId => net.MapNodeToStation(nodeId)).ToList();

            return ConstructRoute(source, stations, destination, metric);
        }

        private NavigationResult ConstructRoute(Point source, List<Station> stations, Point destination, TravelMetric metric)
        {
            IRoute route = new Route(new List<Point>());
            ChangeStation[] changes = new ChangeStation[stations.Count];
            double totalCost = 0.0;
            double lastLength = 0.0;

            // Create part of route from source to first station:
            changes[0] = new ChangeStation();
            if (!source.Equals(stations.First().Position))
            {
                route.Append(geoData.GetRoute(source, stations.First().Position));
                changes[0].WaypointIndex = route.GetPoints().Count() - 1;
                lastLength = route.GetLength();
            }
            else
                changes[0].WaypointIndex = 0;

            changes[0].Name = stations[0].Name;
            changes[0].Number = stations[0].Id;

            // Create the middle part of the route
            for (int i = 1; i < stations.Count; ++i)
            {
                route.Append(geoData.GetRoute(stations[i - 1].Id, stations[i].Id));
                changes[i] = new ChangeStation()
                {
                    WaypointIndex = route.GetPoints().Count() - 1,
                    Name = stations[i].Name,
                    Number = stations[i].Id
                };
                totalCost += metric.PathCost(route.GetLength() - lastLength);
                lastLength = route.GetLength();
            }

            // Create part of route from last station to destination:
            if (!destination.Equals(stations.Last().Position))
                route.Append(geoData.GetRoute(stations.Last().Position, destination));

            var waypoints = route.GetPoints().Select(p => new Waypoint() { Latitude = p.Latitude, Longitude = p.Longitude }).ToArray();

            return new NavigationResult()
            {
                Waypoints = waypoints,
                Stations = changes,
                EstimatedCost = totalCost,
                RouteLength = route.GetLength()
            };
        }
    }
}
