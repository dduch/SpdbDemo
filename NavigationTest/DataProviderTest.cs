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
        [Test]
        public void CheckGeoProvider()
        {
            IGeoDataProvider geoProvider = new GeoDataProvider();
            geoProvider.GetStations();
            geoProvider.GetRoute(new Point(52.2693319, 20.9833518), new Point(52.2184572, 21.0153582));
        }
    }
}
