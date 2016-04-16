using System.Collections.Generic;
using NavigationResolver.Types;

namespace NavigationResolver.Interfaces
{
    public interface IGeoDataProvider
    {
        IRoute GetRoute(Point source, Point destination, RouteType prefferedType, double lengthRestriction = double.PositiveInfinity);
        IEnumerable<Point> GetStations();
    }
}
