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
                NavigationResult route = routeFinder.GetBestRoute(request.StartPosition, request.DestinationPosition, request.Speed);
                return Json(route);
            }
            catch(Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
