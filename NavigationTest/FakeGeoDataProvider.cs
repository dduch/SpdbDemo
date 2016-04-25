using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;
using Navigation.DataModels;
using Navigation.DataProviders;

namespace NavigationTest
{
    class FakeGeoDataProvider : IGeoDataProvider
    {
        List<Point> locations;

        public FakeGeoDataProvider(List<Point> locations)
        {
            this.locations = locations;
        }

        public int GetNearestStation(Point p)
        {
            var bestDistance = double.PositiveInfinity;
            var closestStation = -1;
            for (int i = 0; i < locations.Count(); ++i)
            {
                var currentDist = p.GetDistanceTo(locations[i]);
                if (currentDist < bestDistance)
                {
                    closestStation = i;
                    bestDistance = currentDist;
                }
            }

            return closestStation;
        }

        public double GetPathLength(int startStation, int endStation)
        {
            return locations[startStation].GetDistanceTo(locations[endStation]);
        }

        public IRoute GetRoute(Point source, Point destination)
        {
            return new Route(new List<Point>() { source, destination });
        }

        IEnumerable<Station> IGeoDataProvider.GetStations()
        {
            List<Station> stations = new List<Station>(locations.Count);
            for(int i = 0; i < locations.Count; ++i)
            {
                stations.Add(new Station(i.ToString(), i, locations[i].Latitude, locations[i].Longitude));
            }
            return stations;
        }

        public int MapPositionToStation(Point pos)
        {
            return locations.FindIndex(loc => loc == pos);
        }
    }
}
