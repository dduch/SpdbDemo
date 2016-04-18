using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationResolver.DataModels;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;

namespace NavigationTest
{
    class FakeGeoDataProvider : IGeoDataProvider
    {
        List<Point> locations;

        public FakeGeoDataProvider()
        {
            var points = new List<Tuple<double, double>>()
            {
                new Tuple<double,double> (1000.0    , 1000.0),
                new Tuple<double,double> (500.0     , 2500.0),
                new Tuple<double,double> (1000.0    , 3500.0),
                new Tuple<double,double> (2500.0    , 3500.0),
                new Tuple<double,double> (3500.0    , 3500.0),
                new Tuple<double,double> (4000.0    , 3500.0),
                new Tuple<double,double> (5500.0    , 3000.0),
                new Tuple<double,double> (6000.0    , 1500.0),
                new Tuple<double,double> (4500.0    , 1000.0),
                new Tuple<double,double> (4000.0    , 0.0),
                new Tuple<double,double> (3500.0    , 1000.0),
                new Tuple<double,double> (2000.0    , 1000.0),
                new Tuple<double,double> (9000.0    , 3000.0),
                new Tuple<double,double> (9000.0    , 4500.0),
                new Tuple<double,double> (9000.0    , 6000.0),
                new Tuple<double,double> (9000.0    , 7500.0),
                new Tuple<double,double> (9000.0    , 9000.0),
                new Tuple<double,double> (9000.0    , 10500.0),
                new Tuple<double,double> (1000.0    , 10500.0),
                new Tuple<double,double> (2000.0    , 11000.0),
                new Tuple<double,double> (2500.0    , 11000.0),
                new Tuple<double,double> (3000.0    , 11000.0),
                new Tuple<double,double> (4000.0    , 11000.0)
            };

            double offsetX = 4.5;
            double offsetY = 5.5;
            double dX = 0.04044 / offsetX;
            double dY = 0.04976 / offsetY;

            locations = new List<Point>();

            foreach(var coord in points)
            {
                double lat = dX * (coord.Item1 / 1000 - offsetX);
                double lon = dY * (coord.Item2 / 1000 - offsetY);
                locations.Add(new Point(lat, lon));
            }
        }

        public IRoute GetRoute(Point source, Point destination, RouteType prefferedType, double lengthRestriction = double.PositiveInfinity)
        {
            return new Route(new List<Point>() { source, destination });
        }

        public IEnumerable<Point> GetStations()
        {
            return locations;
        }
    }
}
