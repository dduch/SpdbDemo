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
            List<Keypoint> keypoints = new List<Keypoint>(stations.Count+2);
            Keypoint k;
            double totalCost = 0.0;
            double lastCost = 0.0;
            double lastLength = 0.0;

            // 1) Create part of route from source to first station:
            k = new Keypoint();
            if (!source.Equals(stations.First().Position))
            {
                // Adding first point of route that is different than first station
                keypoints.Add(new Keypoint()
                {
                    IsStation = false,
                    WaypointIndex = 0,
                    Name = "STARTPOINT",
                    Number = -1,
                    DistanceFromPrevious = 0.0,
                    CostFromPrevious = 0.0
                });

                route.Append(new Route( new List<Point>() { source, stations.First().Position }));
                k.WaypointIndex = route.GetPoints().Count() - 1;
                lastLength = route.GetLength();
                k.DistanceFromPrevious = lastLength;
            }
            else
            {
                k.WaypointIndex = 0;
                k.DistanceFromPrevious = 0.0;
            }

            k.IsStation = true;
            k.Name = stations[0].Name;
            k.Number = stations[0].Id;
            k.CostFromPrevious = 0.0;
            keypoints.Add(k);

            // Create the middle part of the route
            for (int i = 1; i < stations.Count; ++i)
            {
                route.Append(geoData.GetRoute(stations[i - 1].Id, stations[i].Id));
                totalCost += metric.PathCost(route.GetLength() - lastLength);
                k = new Keypoint()
                {
                    IsStation = true,
                    WaypointIndex = route.GetPoints().Count() - 1,
                    Name = stations[i].Name,
                    Number = stations[i].Id,
                    DistanceFromPrevious = route.GetLength() - lastLength,
                    CostFromPrevious = totalCost - lastCost
                };
                lastLength = route.GetLength();
                lastCost = totalCost;
                keypoints.Add(k);
            }

            // Create part of route from last station to destination:
            if (!destination.Equals(stations.Last().Position))
            {
                route.Append(new Route(new List<Point>() { stations.Last().Position, destination }));
                // Adding last point of route that is different than last station
                keypoints.Add(new Keypoint()
                {
                    IsStation = false,
                    WaypointIndex = route.GetPoints().Count() - 1,
                    Name = "ENDPOINT",
                    Number = -1,
                    DistanceFromPrevious = route.GetLength() - lastLength,
                    CostFromPrevious = 0.0
                });
            }

            var waypoints = route.GetPoints().Select(p => new Waypoint() { Latitude = p.Latitude, Longitude = p.Longitude }).ToArray();

            return new NavigationResult()
            {
                Waypoints = waypoints,
                Keypoints = keypoints.ToArray(),
                EstimatedCost = totalCost,
                RouteLength = route.GetLength()
            };
        }
    }
}
