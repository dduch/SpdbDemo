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
        private FakeGeoDataProvider fakeGeoData = new FakeGeoDataProvider();

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
    }
}
