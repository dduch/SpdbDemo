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
        public void BuildGraph()
        {
            var stations = fakeGeoData.GetStations().ToList();

            var dist = stations[12].GetDistanceTo(stations[17]);
        }
    }
}
