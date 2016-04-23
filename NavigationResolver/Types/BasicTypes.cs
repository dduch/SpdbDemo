using System;
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

        // Only for testing!!!
        //public double GetDistanceTo(Point p)
        //{
        //    var xdiff = (Latitude - p.Latitude) * 1000.0;
        //    var ydiff = (Longitude - p.Longitude) * 1000.0;
        //    return Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        //}
    }

    // Class containing basic configuration for creating graph
    public class TravelMetric
    {
        // Cyclist average velocity in [m/s]
        public double Velocity { get; }

        // Average time of changing bike in [s]
        public double ChangeTime { get; }

        // Free of charge riding time in [s]
        public double FreeOfChargeTime { get; }

        // Returns fee value in [PLN] basing on riding time in [s] 
        public double CostFunction(double time)
        {
            if (time < 20 * 60)
                return 0;

            if (time < 60 * 60)
                return 1;

            if (time < 2 * 60 * 60)
                return 1 + 3;

            if (time < 3 * 60 * 60)
                return 1 + 3 + 5;

            return (1 + 3 + 5) + Math.Ceiling((time - 3.0 * 60.0 * 60.0) / (60.0 * 60.0)) + ((time > 12 * 60 * 60) ? 200 : 0);
        }

        public TravelMetric()
        {
            Velocity = 15.0 * 1000.0 / 3600.0;
            ChangeTime = 2 * 60;
            FreeOfChargeTime = 20 * 60;
        }

        public TravelMetric(double velocity, double changeTime)
        {
            Velocity = velocity;
            ChangeTime = changeTime;
            FreeOfChargeTime = 20 * 60;
        }

    }
}