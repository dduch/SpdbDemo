using System.Collections.Generic;
using NavigationResolver.Types;

namespace NavigationResolver.Interfaces
{
    /// <summary>
    /// Connects to Open Street Maps and returns specified data
    /// </summary>
    public interface IGeoDataProvider
    {

        /// <summary>
        /// Returns object representing route between two point describing by coordinates
        /// </summary>
        /// <param name="source"> start place coordinates</param>
        /// <param name="destination"> destination place coordinates</param>
        /// <param name="prefferedType"> preffered type of transport bike/car</param>
        /// <param name="lengthRestriction">restricion of length</param>
        IRoute GetRoute(Point source, Point destination, RouteType prefferedType, double lengthRestriction = double.PositiveInfinity);


        /// <summary>
        /// Returns collection of Points which representin coordinates of available veturillo stations
        /// </summary>
        IEnumerable<Point> GetStations();

        /// <summary>
        /// Finds nearest veturilo station to given location.
        /// <param name="p"> given location</param>
        /// <param name="direction"> determines if route should be form p to station (direction == true) or reverse </param>
        /// </summary>
        IRoute GetRouteToNearestStation(Point p, bool direction);
    }
}
