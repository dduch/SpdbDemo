using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpdbDemo.Navigation
{
    enum RouteType
    {
        All,
        Cycle
    }

    public struct Point
    {
        double Lat;
        double Long;
    }

    public interface IRoute
    {
        IEnumerable<Point> GetPoints();

        double GetLength();

        // This method will require reconsideration - is it really needed?
        double GetCycleLenght();

        IRoute Append(IRoute toAppend);
    }

    interface IGeoDataProvider
    {
        IRoute GetRoute(Point source, Point destination, RouteType prefferedType,  double lengthRestriction);

        IEnumerable<Point> GetStations();
    }

    interface INetwork
    {
        IRoute GetBestRoute(Point source, Point destination);
    }


}
