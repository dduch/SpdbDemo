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

        public IRoute GetRoute(Point source, Point destination, RouteType prefferedType, double lengthRestriction = double.PositiveInfinity)
        {
            return new Route(new List<Point>() { source, destination });
        }

        public IRoute GetRouteToNearestStation(Point p, bool direction)
        {
            var locationsWithDist = locations.Select(loc => new Tuple<Point,double> (loc, loc.GetDistanceTo(p))).OrderBy(loc => loc.Item2); ;
            var closest = locationsWithDist.First().Item1;

            if (direction)
                return new Route(new List<Point>() { p, closest });
            else
                return new Route(new List<Point>() { closest, p });
        }

        public IEnumerable<Point> GetStations()
        {
            return locations;
        }
    }
}
