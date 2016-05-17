using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;
using System.Globalization;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using System.Web.Script.Serialization;

namespace LocalConncetionBaseBuilder
{
    class ServiceRouteBuilder : IRouteBuilder
    {
        private Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            ["flat"] = "",
            ["flon"] = "",
            ["tlat"] = "",
            ["tlon"] = "",
            ["v"] = "bicycle",
            ["fast"] = "1",
            ["format"] = "geojson",
            ["geometry"] = "1",
            ["distance"] = "v",
            ["instructions"] = "0",
            ["lang"] = "pl",
        };

        private readonly int sleepTime = 100;

        public float[] BuildRoute(Point src, Point dst)
        {
            parameters["flat"] = src.Latitude.ToString(CultureInfo.InvariantCulture);
            parameters["flon"] = src.Longitude.ToString(CultureInfo.InvariantCulture);
            parameters["tlat"] = dst.Latitude.ToString(CultureInfo.InvariantCulture);
            parameters["tlon"] = dst.Longitude.ToString(CultureInfo.InvariantCulture);

            var basrUrl = "http://www.yournavigation.org/api/1.0/gosmore.php?";
            var url = basrUrl + string.Join("&", parameters.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)));

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

            float[] data = new float[route.coordinates.Length * 2 + 1];

            for (int i = 0; i < route.coordinates.Length; ++i)
            {
                float[] coordinate = route.coordinates[i];
                data[2 * i] = coordinate[1];
                data[2 * i + 1] = coordinate[0];
            }

            data[data.Length - 1] = Convert.ToSingle(route.properties.distance, CultureInfo.InvariantCulture);

            // Sleep to not overload service
            Thread.Sleep(sleepTime);

            return data;
        }
    }
}
