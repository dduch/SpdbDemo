using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;

namespace NavigationResolver.DataProviders
{
    public class GeoDataProvider : IGeoDataProvider
    {
        public IRoute GetRoute(Point source, Point destination, RouteType prefferedType, double lengthRestriction = double.PositiveInfinity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Point> GetStations()
        {
            throw new NotImplementedException();
        }
    }
}
