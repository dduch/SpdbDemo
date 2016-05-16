using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;

namespace LocalConncetionBaseBuilder
{
    interface IRouteBuilder
    {
        // Returns array of floats:
        // [p(0).Latitude, p(0).Longitude, p(1).Latitude, p(1).Longitude, ... , p(n-1).Latitude, p(n-1).Longitude, routeLength]
        // where p(i) is i-th point of route and routeLength is full length of route (in meters)
        float[] BuildRoute(Point src, Point dst);
    }
}
