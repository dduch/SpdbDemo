using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using INavigation;
using Navigation;
using Navigation.DataProviders;
using System.Diagnostics;

namespace NavigationTest
{
    [TestFixture]
    class NavigationTestWithRealData
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            GeoDataProvider.Initialize(TestContext.CurrentContext.TestDirectory + "\\..\\..\\..\\SpdbDemo\\App_Data\\stationsRoutesDB");
        }

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

        [Test]
        public void IntenseNavigation()
        {
            double baseVelocity = 2.778;
            INavigationResolver navigation = new NavigationResolver(new GeoDataProvider());
            StationsManager.Get();
            Point start, end;
            NavigationResult result;
            int runs = 10;
            Random rnd = new Random();

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < runs; ++i)
            {
                start = new Point(52.2693319 + (2 * rnd.NextDouble() - 1), 20.9833518 + +(2 * rnd.NextDouble() - 1));
                end = new Point(52.2184572 + (2 * rnd.NextDouble() - 1), 21.0153582 + (2 * rnd.NextDouble() - 1));

                result = navigation.GetBestRoute(start, end, baseVelocity + 4 * rnd.NextDouble());
            }
            watch.Stop();
            string msg = "Mesured time for " + runs + " runs: " + watch.ElapsedMilliseconds + " ms";
            Assert.Pass(msg);
        }
    }
}
