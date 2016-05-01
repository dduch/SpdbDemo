using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using INavigation;
using Navigation;

namespace NavigationTest
{
    [TestFixture]
    public class NavigationTestWithPerformaceData
    {
        private List<Point> GenerateRandomLocations(Point bottomLeftPoint, Point topRightPoint, int count)
        {
            double deltaLat = topRightPoint.Latitude - bottomLeftPoint.Latitude;
            double deltaLng = topRightPoint.Longitude - bottomLeftPoint.Longitude;
            double latMin = bottomLeftPoint.Latitude;
            double lngMin = bottomLeftPoint.Longitude;

            var rnd = new Random();
            var locations = new List<Point>(count);

            for(int i = 0; i < count; ++i)
            {
                locations.Add(new Point(latMin + rnd.NextDouble() * deltaLat, lngMin + rnd.NextDouble() * deltaLng));
            }

            return locations;
        }

        private void MeasurePerformance(INavigationResolver nav, Point startPoint, Point endPoint, double speed)
        {
            IRoute route;

            var watch = Stopwatch.StartNew();
            route = nav.GetBestRoute(startPoint, endPoint, speed);
            watch.Stop();

            var executionTime = watch.ElapsedMilliseconds;
            string msg = "Execution time: " + executionTime + " ms\nWaypoints: " + route.GetPoints().Count() + "\nRouteLength: " + route.GetLength() + "m";
            Assert.Pass(msg);
        }

        [Test]
        public void NavigationWith250Stations()
        {
            double speed = 1.666;
            Point p1 = new Point(52.10, 20.85);
            Point p2 = new Point(52.35, 21.25);
            int stationsCount = 250;
            FakeGeoDataProvider fakeGeoData = new FakeGeoDataProvider(GenerateRandomLocations(p1, p2, stationsCount));
            INavigationResolver navigation = new NavigationResolver(fakeGeoData);

            MeasurePerformance(navigation, p1, p2, speed);
        }

        [Test]
        public void NavigationWith500Stations()
        {
            double speed = 1.666;
            Point p1 = new Point(52.10, 20.85);
            Point p2 = new Point(52.35, 21.25);
            int stationsCount = 500;
            FakeGeoDataProvider fakeGeoData = new FakeGeoDataProvider(GenerateRandomLocations(p1, p2, stationsCount));
            INavigationResolver navigation = new NavigationResolver(fakeGeoData);

            MeasurePerformance(navigation, p1, p2, speed);
        }

        [Test]
        public void NavigationWith1000Stations()
        {
            double speed = 1.666;
            Point p1 = new Point(52.10, 20.85);
            Point p2 = new Point(52.35, 21.25);
            int stationsCount = 1000;
            FakeGeoDataProvider fakeGeoData = new FakeGeoDataProvider(GenerateRandomLocations(p1, p2, stationsCount));
            INavigationResolver navigation = new NavigationResolver(fakeGeoData);

            MeasurePerformance(navigation, p1, p2, speed);
        }

        [Test]
        public void NavigationWith2000Stations()
        {
            double speed = 1.666;
            Point p1 = new Point(52.10, 20.85);
            Point p2 = new Point(52.35, 21.25);
            int stationsCount = 2000;
            FakeGeoDataProvider fakeGeoData = new FakeGeoDataProvider(GenerateRandomLocations(p1, p2, stationsCount));
            INavigationResolver navigation = new NavigationResolver(fakeGeoData);

            MeasurePerformance(navigation, p1, p2, speed);
        }
    }
}
