using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using INavigation;
using Navigation.DataProviders;

namespace NavigationTest
{
    [TestFixture]
    class DataProviderTest
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            GeoDataProvider.Initialize(TestContext.CurrentContext.TestDirectory + "\\..\\..\\..\\SpdbDemo\\App_Data\\stationsRoutesDB");
        }

        [Test]
        public void CheckGeoProvider()
        {
            IGeoDataProvider geoProvider = new GeoDataProvider();
            var stations = geoProvider.GetStations();
            geoProvider.GetRoute(stations.First().Id, stations.Last().Id);
        }
    }
}
