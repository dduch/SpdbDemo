using INavigation;
using SpdbDemo.Models;
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
        private INavigationResolver routeFinder;

        public RouteController(INavigationResolver routeFinder)
        {
            this.routeFinder = routeFinder;
        }

        [Route("api/Route/FindRoute")]
        public IHttpActionResult FindRoute(RequestDTO request)
        {
            try
            {
                NavigationResult result = routeFinder.GetBestRoute(new Point(52.2693319, 20.9833518), new Point(52.2184572, 21.0153582));
                return Json(result);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
