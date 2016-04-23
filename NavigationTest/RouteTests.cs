using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NavigationResolver.DataModels;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;
using NavigationResolver.DataProviders;

namespace NavigationTest
{
    [TestFixture]
    public class RouteTests
    {
        double[] lats =
        {
                51.765684,  // 51°45'56.5"N
                52.231146,  // 52°13'52.1"N
                52.322855,  // 52°19'22.3"N
                52.654057,  // 52°39'14.6"N
                54.365640,  // 54°21'56.3"N

        };

        double[] longs =
        {
                19.454698,  // 19°27'16.9"E
                21.001029,  // 21°00'03.7"E
                21.104876,  // 21°06'17.6"E
                20.315792,  // 20°18'56.9"E
                18.625094,  // 18°37'30.3"E
        };

        double from0to4length = 0;

        List<Point> testpoints;

        [OneTimeSetUp]
        public void SetUp()
        {
            testpoints = new List<Point>();
            int n = lats.Length;
            for (int i = 0; i < n; ++i)
                testpoints.Add(new Point(lats[i], longs[i]));

            for (int i = 0; i < n - 1; ++i)
                from0to4length += testpoints[i].GetDistanceTo(testpoints[i + 1]);
        }


        [Test]
        public void RouteLengthCheck()
        {
            int count = 5;
            IRoute route = new Route(testpoints.GetRange(0, count));

            Assert.AreEqual(from0to4length, route.GetLength());
            Assert.AreEqual(count, route.GetPoints().Count());

        }

        [Test]
        public void AppendingDisjointRoutes()
        {
            int count = 5;
            IRoute route1 = new Route(testpoints.GetRange(0, 3));
            IRoute route2 = new Route(testpoints.GetRange(3, 2));
            IRoute route = route1.Append(route2);

            Assert.AreEqual(from0to4length, route.GetLength());
            Assert.AreEqual(count, route.GetPoints().Count());
        }

        [Test]
        public void AppendingOverlappingRoutes()
        {
            int count = 5;
            IRoute route1 = new Route(testpoints.GetRange(0, 3));
            IRoute route2 = new Route(testpoints.GetRange(2, 3));
            IRoute route = route1.Append(route2);

            Assert.AreEqual(from0to4length, route.GetLength());
            Assert.AreEqual(count, route.GetPoints().Count());
        }

        [Test]
        public void AppendingEmptyRoutes()
        {
            int count = 5;
            IRoute route = new Route(testpoints.GetRange(0, count));
            IRoute empty = new Route(new List<Point>());

            route.Append(empty);
            Assert.AreEqual(from0to4length, route.GetLength());
            Assert.AreEqual(count, route.GetPoints().Count());

            empty.Append(route);
            Assert.AreEqual(from0to4length, empty.GetLength());
            Assert.AreEqual(count, empty.GetPoints().Count());
        }

        [Test]
        public void CheckGeoProvider()
        {
            IGeoDataProvider geoProvider = new GeoDataProvider();
            geoProvider.GetStations();
            geoProvider.GetRoute(new Point(52.2693319, 20.9833518), new Point(52.2184572, 21.0153582), RouteType.Cycle);
        } 
    }
}
