using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using INavigation;
using Navigation;
using Navigation.DataProviders;
using SpdbDemo.Controllers;
using SpdbDemo.Models;


namespace SpdbDemo.Tests.Controllers
{
    [TestClass]
    public class RouteControllerTest
    {
        [TestMethod]
        public void FindRoute()
        {
            INavigationResolver navigation = new NavigationResolver(new GeoDataProvider());

            RouteController controller = new RouteController(navigation);

            var request = new RequestDTO();
            request.StartPosition = new Point(52.2693319, 20.9833518);
            request.DestinationPosition = new Point(52.2184572, 21.0153582);
            request.Speed = 4.1667;

            var result = controller.FindRoute(request);

            Assert.IsNotNull(result);
        }
    }
}
