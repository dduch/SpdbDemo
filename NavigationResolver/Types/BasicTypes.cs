using System.Device.Location;

namespace NavigationResolver.Types
{
    public enum RouteType
    {
        All,
        Cycle
    }

    public class Point : GeoCoordinate
    {
        public Point(double latitude, double longitude) : base(latitude, longitude)
        {

        }
    }
}