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
        //private VeturiloStations veturiloStations;
        private List<Station> stationsList; 

        // TODO: This must be used, because for the moment we do not handle all stations.
        // When all stations are handled remove this dictionary
        private static Dictionary<int, bool> handledStations = new Dictionary<int, bool>();

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

                if (!handledStations.ContainsKey(id1))
                    handledStations.Add(id1, true);
            }
        }

        public GeoDataProvider()
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            string text = Encoding.UTF8.GetString(Resources.stations);
            //veturiloStations = (VeturiloStations)js.Deserialize(text, typeof(VeturiloStations));

            XElement xdoc = XElement.Load(Settings.Default.MapOfStationsUrl);
            IEnumerable<XElement> stationsXml = (from e in xdoc.Elements("country")
                                                 where (string)e.Attribute("country") == "PL"
                                                 from c in e.Elements("city")
                                                 where (int)c.Attribute("uid") == Settings.Default.WarsawId
                                                 from p in c.Elements("place")
                                                 select p).ToList();

            stationsList = new List<Station>(stationsXml.Count());

            foreach (XElement node in stationsXml)
            {
                var stationId = Convert.ToInt32(node.Attribute("number").Value, CultureInfo.InvariantCulture);
                if (!handledStations.ContainsKey(stationId))
                    continue;

                stationsList.Add(new Station(
                    node.Attribute("name").Value,
                    stationId,
                    Convert.ToDouble(node.Attribute("lat").Value, CultureInfo.InvariantCulture),
                    Convert.ToDouble(node.Attribute("lng").Value, CultureInfo.InvariantCulture),
                    node.Attribute("bikes").Value != "0" // TODO: I am not sure this handles all cases, if only someone can confirm this
                ));
            }

        }

        public IRoute GetRoute(Point source, Point destination)
        {
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

        public int GetNearestStation(Point p, bool nonempty = false)
        {

            double bestDistance = Double.PositiveInfinity;
            int nearestStation = -1;

            foreach (var station in stationsList)
            {
                if (nonempty && !station.Bikes)
                    continue;

                double tempDist = p.GetDistanceTo(station.Position);
                if (tempDist < bestDistance)
                {
                    bestDistance = tempDist;
                    nearestStation = station.Id;
                }
            }

            //foreach(StationInfo station in veturiloStations.Stations)
            //{
            //    if (!handledStations.ContainsKey(station.Stationnumber ?? -1))
            //        continue;

            //    Point dest = new Point(station.Latitude.Value, station.Longitude.Value);
            //    double tempDist = p.GetDistanceTo(dest);
            //    if (tempDist < bestDistance && station.Stationnumber != null)
            //    {
            //        bestDistance = tempDist;
            //        nearestStation = (int)station.Stationnumber;
            //    } 
            //}

            return nearestStation;   
        }

        public IEnumerable<Station> GetStations()
        { 
            return stationsList;
        }

        public double GetPathLength(int startStation, int endStation)
        {
            return StationsRoutes[new KeyValuePair<int, int>(startStation, endStation)];
        }
    }
}
