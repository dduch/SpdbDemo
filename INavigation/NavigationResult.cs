using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INavigation
{
    public class Waypoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class ChangeStation
    {
        public int WaypointIndex { get; set; } // Index of station in waypoints array
        public string Name { get; set; } // Station specific name
        public int Number { get; set; } // Station unique identifier
    }

    // Describes route found by navigation
    public class NavigationResult
    {
        public Waypoint[] Waypoints { get; set; } // Geolocations of subsequent points of route 
        public ChangeStation[] Stations { get; set; } // Describes which points of route are visited bike change stations
        public double RouteLength { get; set; } // Route total length in meters
        public double EstimatedCost { get; set; } // Route total cost in PLN
    }
}
