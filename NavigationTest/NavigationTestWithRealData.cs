using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using INavigation;
using Navigation;
using Navigation.DataProviders;

namespace NavigationTest
{
    [TestFixture]
    class NavigationTestWithRealData
    {
        [Test]
        public void BasicNavigationTest()
        {
            double baseVelocity = 15.0;
            INavigationResolver navigation = new NavigationResolver(new GeoDataProvider());
            Point start, end;
            NavigationResult result;

            // ---------- Test case 1 ----------

            start = new Point(52.2693319, 20.9833518);
            end = new Point(52.2184572, 21.0153582);

            result = navigation.GetBestRoute(start, end, baseVelocity);
        }
    }
}
