using System;
using NavigationResolver.Interfaces;
using NavigationResolver.DataModels;
using NavigationResolver.Types;
using NUnit.Framework;

namespace NavigationTest
{
    [TestFixture]
    public class RoutingTests
    {
        [Test]
        public void SampleTest()
        {
            INetwork net = new Network();
            Point src = new Point(), dst = new Point();
            Assert.Throws(typeof(NotImplementedException), () => net.GetBestRoute(src, dst));
        }
    }
}
