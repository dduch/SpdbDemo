using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Navigation.Properties;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using System.Web.Script.Serialization;
using INavigation;
using Navigation.DataModels;
using Navigation;

namespace Navigation.DataProviders
{
    public class GeoDataProvider : IGeoDataProvider
    {
        private Dictionary<string, string> Parameters = new Dictionary<string, string>
        {
            ["flat"] = "",
            ["flon"] = "",
            ["tlat"] = "",
            ["tlon"] = "pl",
            ["v"] = "",
            ["fast"] = "0",
            ["format"] = "geojson",
            ["geometry"] = "1",
            ["distance"] = "v",
            ["instructions"] = "0",
            ["lang"] = "pl",
        };

        private static Dictionary<KeyValuePair<int, int>, double> StationsRoutes = new Dictionary<KeyValuePair<int, int>, double>();
        private VeturiloStations veturiloStations;

        static GeoDataProvider()
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(Resources.stationsRoutesDB));

            int count = Resources.stationsRoutesDB.Length;
            while(count > 0)
            {
                int id1 = reader.ReadInt32();
                int id2 = reader.ReadInt32();
                double dist = reader.ReadDouble();
                count -= 16;
                StationsRoutes.Add(new KeyValuePair<int, int>(id1, id2), dist);
            }
        }

        public GeoDataProvider()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string text = Encoding.UTF8.GetString(Resources.stations);
            veturiloStations = (VeturiloStations)js.Deserialize(text, typeof(VeturiloStations));
        }

        public IRoute GetRoute(Point source, Point destination, RouteType prefferedType, double lengthRestriction = double.PositiveInfinity)
        {
            //return new Route(new List<Point>() { source, destination });

            Parameters["flat"] = source.Latitude.ToString(CultureInfo.InvariantCulture);
            Parameters["flon"] = source.Longitude.ToString(CultureInfo.InvariantCulture);
            Parameters["tlat"] = destination.Latitude.ToString(CultureInfo.InvariantCulture);
            Parameters["tlon"] = destination.Longitude.ToString(CultureInfo.InvariantCulture);

            var url = string.Format(Settings.Default.RouteService,
                            string.Join("&", Parameters.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value))));

            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            WebResponse webResp = request.GetResponse();
            Rootobject route = null;
            using (var reader = new StreamReader(webResp.GetResponseStream()))
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var jsonResponse = reader.ReadToEnd();
                route = (Rootobject)js.Deserialize(jsonResponse, typeof(Rootobject));
            }

            List<Point> foundedRoute = new List<Point>();

            foreach(float[] coordinate in route.coordinates)
            {
                foundedRoute.Add(new Point(coordinate[0], coordinate[1]));
            }

            return new Route(foundedRoute, Convert.ToDouble(route.properties.distance, CultureInfo.InvariantCulture));
        }

        public IRoute GetRouteToNearestStation(Point p, bool direction)
        {
            double distance = Double.PositiveInfinity;
            Point nearestStation = null;

            foreach(StationInfo station in veturiloStations.Stations)
            {
                Point dest = new Point(station.Latitude.Value, station.Longitude.Value);
                double tempDist = p.GetDistanceTo(dest);
                if (tempDist < distance)
                {
                    distance = tempDist;
                    nearestStation = dest;
                } 
            }

            return GetRoute(p, nearestStation, RouteType.Cycle);
        }

        public IEnumerable<Point> GetStations()
        {
            XElement xdoc = XElement.Load(Settings.Default.MapOfStationsUrl);
            IEnumerable<XElement> stations = (from e in xdoc.Elements("country")
                                              where (string)e.Attribute("country") == "PL"
                                              from c in e.Elements("city")
                                              where (int)c.Attribute("uid") == Settings.Default.WarsawId
                                              from p in c.Elements("place")
                                              select p).ToList();

            List<Point> coordinates = new List<Point>();
            foreach (XElement node in stations)
            {
                coordinates.Add(new Point((double)node.Attribute("lat"), (double)node.Attribute("lng")));
            }

            return coordinates.Take(10);
        }
    }
}
