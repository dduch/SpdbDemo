using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using INavigation;
using Navigation;
using Navigation.DataModels;

namespace NavigationTest
{
    [TestFixture]
    public class NavigationTestWithFakeData
    {
        private FakeGeoDataProvider fakeGeoData;

        private Point ConvertCartesianToPoint(double x, double y)
        {
            double offsetX = 4.5;
            double offsetY = 5.5;
            double dX = 0.04044 / offsetX;
            double dY = 0.04976 / offsetY;
            double lat = dX * (x - offsetX);
            double lon = dY * (y - offsetY);
            return (new Point(lat, lon));
            //return new Point(x, y);
        }

        private List<Point> BuildFixedLocation()
        {
            var points = new List<Tuple<double, double>>()
            {
                new Tuple<double,double> (1.0    , 1.0),  // 1
                new Tuple<double,double> (0.5    , 2.5),  // 2
                new Tuple<double,double> (1.0    , 3.5),  // 3
                new Tuple<double,double> (2.5    , 3.5),  // 4
                new Tuple<double,double> (3.5    , 3.5),  // 5
                new Tuple<double,double> (4.0    , 3.5),  // 6
                new Tuple<double,double> (5.5    , 3.0),  // 7
                new Tuple<double,double> (6.0    , 1.5),  // 8
                new Tuple<double,double> (4.5    , 1.0),  // 9
                new Tuple<double,double> (4.0    , 0.0),  // 10
                new Tuple<double,double> (3.5    , 1.0),  // 11
                new Tuple<double,double> (2.0    , 1.0),  // 12
                new Tuple<double,double> (9.0    , 3.0),  // 13
                new Tuple<double,double> (9.0    , 4.5),  // 14
                new Tuple<double,double> (9.0    , 6.0),  // 15
                new Tuple<double,double> (9.0    , 7.5),  // 16
                new Tuple<double,double> (9.0    , 9.0),  // 17
                new Tuple<double,double> (9.0    , 10.5),  // 18
                new Tuple<double,double> (1.0    , 10.5),  // 19
                new Tuple<double,double> (2.0    , 11.0),  // 20
                new Tuple<double,double> (2.5    , 11.0),  // 21
                new Tuple<double,double> (3.0    , 11.0),  // 22
                new Tuple<double,double> (4.0    , 11.0)   // 23
            };

            var locations = new List<Point>();

            foreach (var coord in points)
            {
                locations.Add(ConvertCartesianToPoint(coord.Item1, coord.Item2));
            }

            return locations;
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            fakeGeoData = new FakeGeoDataProvider(BuildFixedLocation());
        }

        [Test]
        public void NavigationWithFakeData()
        {
            double baseVelocity = 1.666;
            INavigationResolver navigation = new NavigationResolver(fakeGeoData);
            Point start, end;
            Waypoint[] waypoints;
            List<int> path;

            // ---------- Test case 1 ----------

            start = ConvertCartesianToPoint(0.0, 0.0);
            end = ConvertCartesianToPoint(4.0, 4.0);

            waypoints = navigation.GetBestRoute(start, end, baseVelocity).Waypoints;
            Assert.AreEqual(5 + 2, waypoints.Length);

            path = waypoints.Select(p => fakeGeoData.MapPositionToStation(p)).ToList();



            // ---------- Test case 2 ----------

            start = ConvertCartesianToPoint(0.0, 10.0);
            end = ConvertCartesianToPoint(5.0, 11.0);

            waypoints = navigation.GetBestRoute(start, end, baseVelocity).Waypoints;
            Assert.AreEqual(3 + 2, waypoints.Length);

            path = waypoints.Select(p => fakeGeoData.MapPositionToStation(p)).ToList();

            // ---------- Test case 3 ----------

            start = ConvertCartesianToPoint(0.0, 0.0);
            end = ConvertCartesianToPoint(2.5, 12.0);

            waypoints = navigation.GetBestRoute(start, end, baseVelocity).Waypoints;
            Assert.AreEqual(10 + 2, waypoints.Length);

            path = waypoints.Select(p => fakeGeoData.MapPositionToStation(p)).ToList();

            // ---------- Test case 4 ----------

            start = ConvertCartesianToPoint(0.0, 0.0);
            end = ConvertCartesianToPoint(2.5, 12.0);

            waypoints = navigation.GetBestRoute(start, end, baseVelocity * 1.5).Waypoints;
            Assert.AreEqual(3 + 2, waypoints.Length);

            path = waypoints.Select(p => fakeGeoData.MapPositionToStation(p)).ToList();

            // ---------- Test case 5 ----------

            start = ConvertCartesianToPoint(0.0, 0.0 );
            end = ConvertCartesianToPoint(2.5, 12.0);

            waypoints = navigation.GetBestRoute(start, end, baseVelocity * 2.0).Waypoints;
            Assert.AreEqual(2 + 2, waypoints.Length);

            path = waypoints.Select(p => fakeGeoData.MapPositionToStation(p)).ToList();

            // ---------- Test case 6 ----------

            start = ConvertCartesianToPoint(0.0, 0.0);
            end = ConvertCartesianToPoint(2.5, 12.0);

            waypoints = navigation.GetBestRoute(start, end, baseVelocity * 0.375).Waypoints;
            Assert.AreEqual(2 + 2, waypoints.Length);

            path = waypoints.Select(p => fakeGeoData.MapPositionToStation(p)).ToList();

        }
    }
}
