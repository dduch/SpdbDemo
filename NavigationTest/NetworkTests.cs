using System;
using System.Collections.Generic;
using System.Linq;
using System.Device;
using NUnit.Framework;
using NavigationResolver.DataModels;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;

namespace NavigationTest
{
    [TestFixture]
    public class NetworkTests
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
        public void BuildSubGraph()
        {
            double baseVelocity = 1.666;
            TravelMetric testMetric;
            NetworkBuilder builder;
            List<SubGraph> graphs;

            // ---------- Test case 1 ----------

            testMetric = new TravelMetric(baseVelocity, 100.0);
            builder = new NetworkBuilder(fakeGeoData, testMetric);
            graphs = builder.BuildSubGraphs();

            Assert.AreEqual(3, graphs.Count);
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 12));
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 6));
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 5));

            // ---------- Test case 2 ----------

            testMetric = new TravelMetric(baseVelocity * 1.5, 100.0);
            builder = new NetworkBuilder(fakeGeoData, testMetric);
            graphs = builder.BuildSubGraphs();

            Assert.AreEqual(3, graphs.Count);
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 12));
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 6));
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 5));

            // ---------- Test case 3 ----------

            testMetric = new TravelMetric(baseVelocity * 2.0, 100.0);
            builder = new NetworkBuilder(fakeGeoData, testMetric);
            graphs = builder.BuildSubGraphs();

            Assert.AreEqual(2, graphs.Count);
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 18));
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 5));

            // ---------- Test case 4 ----------

            testMetric = new TravelMetric(baseVelocity * 4.0, 100.0);
            builder = new NetworkBuilder(fakeGeoData, testMetric);
            graphs = builder.BuildSubGraphs();

            Assert.AreEqual(1, graphs.Count);
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 23));

            // ---------- Test case 4 ----------

            testMetric = new TravelMetric(baseVelocity * 0.375, 100.0);
            builder = new NetworkBuilder(fakeGeoData, testMetric);
            graphs = builder.BuildSubGraphs();

            Assert.AreEqual(20, graphs.Count);
            Assert.IsTrue(graphs.FindAll(g => g.Vertices.Count == 1).Count == 18);
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 2));
            Assert.IsTrue(graphs.Exists(g => g.Vertices.Count == 3));
        }

        [Test]
        public void BuildSuperGraph()
        {
            double baseVelocity = 1.666;
            TravelMetric testMetric;
            NetworkBuilder builder;
            SuperGraph graph;

            // ---------- Test case 1 ----------

            testMetric = new TravelMetric(baseVelocity, 100.0);
            builder = new NetworkBuilder(fakeGeoData, testMetric);
            graph = builder.BuildSuperGraph(builder.BuildSubGraphs());

            Assert.AreEqual(3, graph.SubGraphs.Count);
            foreach (var subGraph in graph.SubGraphs)
                Assert.AreEqual(2, subGraph.IntergraphArchs.Count);

            // ---------- Test case 2 ----------

            testMetric = new TravelMetric(baseVelocity * 1.5, 100.0);
            builder = new NetworkBuilder(fakeGeoData, testMetric);
            graph = builder.BuildSuperGraph(builder.BuildSubGraphs());

            Assert.AreEqual(3, graph.SubGraphs.Count);
            foreach (var subGraph in graph.SubGraphs)
                Assert.AreEqual(2, subGraph.IntergraphArchs.Count);

            // ---------- Test case 3 ----------

            testMetric = new TravelMetric(baseVelocity * 2.0, 100.0);
            builder = new NetworkBuilder(fakeGeoData, testMetric);
            graph = builder.BuildSuperGraph(builder.BuildSubGraphs());

            Assert.AreEqual(2, graph.SubGraphs.Count);
            foreach (var subGraph in graph.SubGraphs)
                Assert.AreEqual(1, subGraph.IntergraphArchs.Count);

            // ---------- Test case 4 ----------

            testMetric = new TravelMetric(baseVelocity * 4.0, 100.0);
            builder = new NetworkBuilder(fakeGeoData, testMetric);
            graph = builder.BuildSuperGraph(builder.BuildSubGraphs());

            Assert.AreEqual(1, graph.SubGraphs.Count);
            foreach (var subGraph in graph.SubGraphs)
                Assert.AreEqual(0, subGraph.IntergraphArchs.Count);

            // ---------- Test case 4 ----------

            testMetric = new TravelMetric(baseVelocity * 0.375, 100.0);
            builder = new NetworkBuilder(fakeGeoData, testMetric);
            graph = builder.BuildSuperGraph(builder.BuildSubGraphs());

            Assert.AreEqual(20, graph.SubGraphs.Count);
            foreach (var subGraph in graph.SubGraphs)
                Assert.AreEqual(19, subGraph.IntergraphArchs.Count);
        }

        [Test]
        public void RoutingTest()
        {
            double baseVelocity = 1.666;
            TravelMetric testMetric;
            INetwork net;
            Point start, end;
            IRoute result;

            // ---------- Test case 1 ----------

            testMetric = new TravelMetric(baseVelocity, 100.0);
            net = Network.Build(fakeGeoData, testMetric);
            start = ConvertCartesianToPoint(0.0, 0.0);
            end = ConvertCartesianToPoint(4.0, 4.0);

            result = net.GetBestRoute(start, end);
            Assert.AreEqual(5 + 2, result.GetPoints().ToList().Count);

            // ---------- Test case 2 ----------

            testMetric = new TravelMetric(baseVelocity, 100.0);
            net = Network.Build(fakeGeoData, testMetric);
            start = ConvertCartesianToPoint(0.0, 10.0);
            end = ConvertCartesianToPoint(5.0, 11.0);

            result = net.GetBestRoute(start, end);
            Assert.AreEqual(3 + 2, result.GetPoints().ToList().Count);

            // ---------- Test case 3 ----------

            testMetric = new TravelMetric(baseVelocity, 100.0);
            net = Network.Build(fakeGeoData, testMetric);
            start = ConvertCartesianToPoint(0.0, 0.0);
            end = ConvertCartesianToPoint(2.5, 12.0);

            result = net.GetBestRoute(start, end);
            Assert.AreEqual(13 + 2, result.GetPoints().ToList().Count);

            // ---------- Test case 4 ----------

            testMetric = new TravelMetric(baseVelocity*1.5, 100.0);
            net = Network.Build(fakeGeoData, testMetric);
            start = ConvertCartesianToPoint(0.0, 0.0);
            end = ConvertCartesianToPoint(2.5, 12.0);

            result = net.GetBestRoute(start, end);
            Assert.AreEqual(4 + 2, result.GetPoints().ToList().Count);

            // ---------- Test case 5 ----------

            testMetric = new TravelMetric(baseVelocity*2.0, 100.0);
            net = Network.Build(fakeGeoData, testMetric);
            start = ConvertCartesianToPoint(0.0, 0.0);
            end = ConvertCartesianToPoint(2.5, 12.0);

            result = net.GetBestRoute(start, end);
            Assert.AreEqual(8 + 2, result.GetPoints().ToList().Count);

            // ---------- Test case 6 ----------

            testMetric = new TravelMetric(baseVelocity * 0.375, 100.0);
            net = Network.Build(fakeGeoData, testMetric);
            start = ConvertCartesianToPoint(0.0, 0.0);
            end = ConvertCartesianToPoint(2.5, 12.0);

            result = net.GetBestRoute(start, end);
            Assert.AreEqual(3 + 2, result.GetPoints().ToList().Count);


        }
    }
}
