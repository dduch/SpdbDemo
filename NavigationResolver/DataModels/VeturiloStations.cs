using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.DataModels
{
    public class VeturiloStations
    {
        public StationInfo[] Stations { get; set; }
    }

    public class StationInfo
    {
        public string Localization { get; set; }
        public int? Stationnumber { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string District { get; set; }
    }
}