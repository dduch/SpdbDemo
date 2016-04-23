using NavigationResolver.DataModels;
using NavigationResolver.DataProviders;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;
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
        private INetwork routeFinder;
        private IGeoDataProvider geoDataProvider;

        public RouteController(INetwork routeFinder, IGeoDataProvider geoDataProvider)
        {
            this.routeFinder = routeFinder;
            this.geoDataProvider = geoDataProvider;
        }

        [Route("api/Route/FindRoute")]
        public IHttpActionResult FindRoute(RequestDTO request)
        {
            try
            {
                IRoute route = routeFinder.GetBestRoute(new Point(52.2693319, 20.9833518), new Point(52.2184572, 21.0153582));
                return Json(route.GetPoints().ToList());
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
