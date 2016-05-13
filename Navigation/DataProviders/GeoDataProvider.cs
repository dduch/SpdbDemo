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
            ["v"] = "bicycle",
            ["fast"] = "0",
            ["format"] = "geojson",
            ["geometry"] = "1",
            ["distance"] = "v",
            ["instructions"] = "0",
            ["lang"] = "pl",
        };

        private static Dictionary<KeyValuePair<int, int>, Route> StationsRoutes = new Dictionary<KeyValuePair<int, int>, Route>();

        static GeoDataProvider()
        {
            BinaryReader reader = new BinaryReader(new MemoryStream(Resources.stationsRoutesDB));
            Dictionary<int, bool> handledStations = StationsManager.Get().ToDictionary(
                station => station.Id,
                station => false
            );

            int count = Resources.stationsRoutesDB.Length;
            while(count > 0)
            {
                int idOfsett = 6000;
                int id1 = (int)reader.ReadUInt16() + idOfsett;
                int id2 = (int)reader.ReadUInt16() + idOfsett;
                int n = (int)reader.ReadUInt16();
                float[] points = new float[n];
                for(int k = 0; k < n; ++k)
                {
                    points[k] = reader.ReadSingle();
                }
                count -= 3*2 + n*4;

                if (handledStations.ContainsKey(id1) && handledStations.ContainsKey(id2))
                {
                    StationsRoutes.Add(new KeyValuePair<int, int>(id1, id2), new Route(points));
                    handledStations[id1] = true;
                    handledStations[id2] = true;
                }
            }

            //if (handledStations.ContainsValue(false))
            //    throw new Exception("Critical error: connection base does not contain all existing stations!");

        }

        public static void PrefetchStations()
        {
            StationsManager.Prefetch();
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
                foundedRoute.Add(new Point(coordinate[1], coordinate[0]));
            }

            return new Route(foundedRoute, Convert.ToDouble(route.properties.distance, CultureInfo.InvariantCulture));
        }

        public int GetNearestStation(Point p, bool nonempty = false)
        {

            double bestDistance = Double.PositiveInfinity;
            int nearestStation = -1;

            foreach (var station in StationsManager.Get())
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

            return nearestStation;   
        }

        public IEnumerable<Station> GetStations()
        { 
            return StationsManager.Get();
        }

        public double GetPathLength(int startStation, int endStation)
        {
            return StationsRoutes[new KeyValuePair<int, int>(startStation, endStation)].GetLength();
        }

        public IRoute GetRoute(int source, int destination)
        {
            return StationsRoutes[new KeyValuePair<int, int>(source, destination)];
        }
    }
}
