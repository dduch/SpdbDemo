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
using System.Web;

namespace Navigation.DataProviders
{
    public class GeoDataProvider : IGeoDataProvider
    {
        //private Dictionary<string, string> Parameters = new Dictionary<string, string>
        //{
        //    ["flat"] = "",
        //    ["flon"] = "",
        //    ["tlat"] = "",
        //    ["tlon"] = "",
        //    ["v"] = "foot",
        //    ["fast"] = "0",
        //    ["format"] = "geojson",
        //    ["geometry"] = "1",
        //    ["distance"] = "v",
        //    ["instructions"] = "0",
        //    ["lang"] = "pl",
        //};

        private static Dictionary<KeyValuePair<int, int>, float[]> StationsRoutes = null;

        public static void Initialize(string pathToConnectionsBase)
        {
            StationsRoutes = new Dictionary<KeyValuePair<int, int>, float[]>();
            BinaryReader reader = new BinaryReader(new FileStream(pathToConnectionsBase, FileMode.Open, FileAccess.Read));
            Dictionary <int, bool> handledStations = StationsManager.SafeGet().ToDictionary(
                station => station.Id,
                station => false
            );

            while(reader.BaseStream.Position != reader.BaseStream.Length)
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

                if (handledStations.ContainsKey(id1) && handledStations.ContainsKey(id2))
                {
                    StationsRoutes.Add(new KeyValuePair<int, int>(id1, id2), points);
                    handledStations[id1] = true;
                    handledStations[id2] = true;
                }
            }

            if (handledStations.ContainsValue(false))
                throw new Exception("Connections database requires update!");

        }

        public static void PrefetchStations()
        {
            StationsManager.Prefetch();
        }

        //public IRoute GetRoute(Point source, Point destination)
        //{
        //    Parameters["flat"] = source.Latitude.ToString(CultureInfo.InvariantCulture);
        //    Parameters["flon"] = source.Longitude.ToString(CultureInfo.InvariantCulture);
        //    Parameters["tlat"] = destination.Latitude.ToString(CultureInfo.InvariantCulture);
        //    Parameters["tlon"] = destination.Longitude.ToString(CultureInfo.InvariantCulture);

        //    var url = string.Format(Settings.Default.RouteService,
        //                    string.Join("&", Parameters.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value))));
        //    Rootobject route = null;

        //    try
        //    {
        //        WebRequest request = WebRequest.Create(url);
        //        request.Method = "GET";
        //        WebResponse webResp = request.GetResponse();
                
        //        using (var reader = new StreamReader(webResp.GetResponseStream()))
        //        {
        //            JavaScriptSerializer js = new JavaScriptSerializer();
        //            var jsonResponse = reader.ReadToEnd();
        //            route = (Rootobject)js.Deserialize(jsonResponse, typeof(Rootobject));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Failed to construct route: " + ex.Message, ex);
        //    }

        //    List<Point> foundedRoute = new List<Point>();

        //    foreach(float[] coordinate in route.coordinates)
        //    {
        //        foundedRoute.Add(new Point(coordinate[1], coordinate[0]));
        //    }

        //    return new Route(foundedRoute, 1000.0 * Convert.ToDouble(route.properties.distance, CultureInfo.InvariantCulture));
        //}

        public int GetNearestStation(Point p, bool nonempty = false)
        {

            double bestDistance = Double.PositiveInfinity;
            int nearestStation = -1;

            foreach (var station in StationsManager.SafeGet())
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

            if (nearestStation < 0)
                throw new Exception("Cannot find relevant station near " + p.Latitude + ";" + p.Longitude);

            return nearestStation;   
        }

        public IEnumerable<Station> GetStations()
        { 
            return StationsManager.SafeGet();
        }

        public double GetPathLength(int startStation, int endStation)
        {
            return StationsRoutes[new KeyValuePair<int, int>(startStation, endStation)].Last();
        }

        public IRoute GetRoute(int source, int destination)
        {
            return new Route(StationsRoutes[new KeyValuePair<int, int>(source, destination)]);
        }
    }
}
