using System.Device.Location;

namespace INavigation
{
    public class Point : GeoCoordinate
    {
        public Point(double latitude, double longitude) : base(latitude, longitude)
        {

        }

        // Only for testing!!!
        //public double GetDistanceTo(Point p)
        //{
        //    var xdiff = (Latitude - p.Latitude) * 1000.0;
        //    var ydiff = (Longitude - p.Longitude) * 1000.0;
        //    return Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        //}
    }
}
