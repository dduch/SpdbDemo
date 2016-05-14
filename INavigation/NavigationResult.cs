using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INavigation
{
    // Signle point on the found route
    public class Waypoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    // Metadata about special waypoints - stations or start/end points
    public class Keypoint
    {
        public bool IsStation { get; set; } // True if keypoint is station
        public int WaypointIndex { get; set; } // Index of keypoint in waypoints array
        public string Name { get; set; } // Keypoint specific name
        public int Number { get; set; } // Station unique identifier (-1 if keypoint is not station)
        public double DistanceFromPrevious { get; set; } // Distance from previous keypoint to this in meters
        public double CostFromPrevious { get; set; } // Cost of getting to this keypoint from previous in PLN
    }

    // Describes route found by navigation
    public class NavigationResult
    {
        public Waypoint[] Waypoints { get; set; } // Geolocations of subsequent points of route 
        public Keypoint[] Keypoints { get; set; } // Metadata aobut special waypoints (change stations, startpoint, endpoint)
        public double RouteLength { get; set; } // Route total length in meters
        public double EstimatedCost { get; set; } // Route total cost in PLN
    }
}
