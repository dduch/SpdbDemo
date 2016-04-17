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
        private IGeoDataProvider networkBulider;

        public RouteController(INetwork routeFinder, IGeoDataProvider networkBulider)
        {
            this.routeFinder = routeFinder;
            this.networkBulider = networkBulider;
        }

        [Route("api/Route/GetRoute")]
        public IHttpActionResult FindRoute(Point point)
        {
            return Json("OK");
        }
    }
}
