using NavigationResolver.DataModels;
using NavigationResolver.DataProviders;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpdbDemo.Controllers
{
    public class RouteController : ApiController
    {     
        private INetwork routeFinder;
        private IGeoDataProvider geoDataProvider;

        public RouteController(INetwork routeFinder, IGeoDataProvider geoDataProvider)
        {
            this.routeFinder = routeFinder;
            this.geoDataProvider = geoDataProvider;
        }

        [Route("api/Route/FindRoute")]
        public IHttpActionResult FindRoute(Point point)
        {
            geoDataProvider.GetStations();
            IRoute route = geoDataProvider.GetRoute(new Point(52.2693319, 20.9833518), new Point(52.2184572, 21.0153582), RouteType.Cycle);
            return Json(route.GetPoints().ToList());
        }
    }
}
