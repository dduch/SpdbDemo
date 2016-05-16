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

namespace LocalConncetionBaseBuilder
{
    class ServiceRouteBuilder : IRouteBuilder
    {
        private Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            ["origin"] = "",
            ["destination"] = "",
            ["mode"] = "bicycling",
            ["units"] = "metric",
        };

        public float[] BuildRoute(Point src, Point dst)
        {
            parameters["origin"] = src.Latitude.ToString(CultureInfo.InvariantCulture) + "," + src.Longitude.ToString(CultureInfo.InvariantCulture);
            parameters["destination"] = dst.Latitude.ToString(CultureInfo.InvariantCulture) + "," + dst.Longitude.ToString(CultureInfo.InvariantCulture);
            var gmapsBase = "https://maps.googleapis.com/maps/api/directions/";
            var responseType = "xml"; // json/xml
            var url = gmapsBase + responseType + "?" + string.Join("&", parameters.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value)));

            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            WebResponse webResp = request.GetResponse();
            XElement xdoc;

            using (var reader = new StreamReader(webResp.GetResponseStream()))
            {
                xdoc = XElement.Parse(reader.ReadToEnd());
            }

            var xstatus = xdoc.Element("status");

            if (xstatus.Value != "OK")
                throw new Exception("Result not OK");


            var xdist = xdoc.Element("route").Element("leg").Element("distance").Element("value");
            var xend = xdoc.Element("route").Element("leg").Element("end_location");
            var xsteps = (from s in xdoc.Element("route").Element("leg").Elements("step") select s.Element("start_location"));

            int i = 0;
            var data = new float[xsteps.Count() * 2 + 2 + 1];
            foreach (var xstep in xsteps)
            {
                data[i++] = (float)Convert.ToDouble(xstep.Element("lat").Value, CultureInfo.InvariantCulture);
                data[i++] = (float)Convert.ToDouble(xstep.Element("lng").Value, CultureInfo.InvariantCulture);
            }
            data[i++] = (float)Convert.ToDouble(xend.Element("lat").Value, CultureInfo.InvariantCulture);
            data[i++] = (float)Convert.ToDouble(xend.Element("lng").Value, CultureInfo.InvariantCulture);
            data[i++] = (float)Convert.ToDouble(xdist.Value, CultureInfo.InvariantCulture);

            return data;
        }
    }
}
