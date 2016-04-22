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
    }
}
