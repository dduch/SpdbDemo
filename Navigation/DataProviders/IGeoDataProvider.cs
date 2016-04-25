using System.Collections.Generic;
using INavigation;
using Navigation.DataModels;

namespace Navigation.DataProviders
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
        IRoute GetRoute(Point source, Point destination);


        /// <summary>
        /// Returns collection of Points which representin coordinates of available veturillo stations
        /// </summary>
        IEnumerable<Station> GetStations();

        /// <summary>
        /// Finds nearest veturilo station to given location.
        /// <param name="p"> given location </param>
        /// <param name="nonempty"> determines if there must be some bicycles on station </param>
        /// </summary>
        int GetNearestStation(Point p, bool nonempty = false);


        /// <summary>
        /// Returns path length between 2 stations.
        /// <param name="startStation"> Starting station id</param>
        /// <param name="endStation"> Ending station id </param>
        /// </summary>
        double GetPathLength(int startStation, int endStation);
    }
}
