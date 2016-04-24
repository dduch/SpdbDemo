using LocalConncetionBaseBuilder.Properties;
using NavigationResolver.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LocalConncetionBaseBuilder
{
    class Program
    {
        private static Dictionary<string, string> Parameters = new Dictionary<string, string>
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
        

        static void Main(string[] args)
        {
            VeturiloStations veturiloStations;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string text = Encoding.UTF8.GetString(Resources.stations);
            veturiloStations = (VeturiloStations)js.Deserialize(text, typeof(VeturiloStations));

            StringBuilder content = new StringBuilder();
            string line = string.Empty;

            foreach(StationInfo baseStation in veturiloStations.Stations)
            {
                foreach(StationInfo destinationStation in veturiloStations.Stations)
                {
                    if(baseStation.Stationnumber != destinationStation.Stationnumber)
                    {
                        Parameters["flat"] = baseStation.Latitude.Value.ToString(CultureInfo.InvariantCulture);
                        Parameters["flon"] = baseStation.Longitude.Value.ToString(CultureInfo.InvariantCulture);
                        Parameters["tlat"] = destinationStation.Latitude.Value.ToString(CultureInfo.InvariantCulture);
                        Parameters["tlon"] = destinationStation.Longitude.Value.ToString(CultureInfo.InvariantCulture);

                        var url = string.Format(Settings.Default.RouteService,
                                        string.Join("&", Parameters.Select(kvp => string.Format("{0}={1}", kvp.Key, kvp.Value))));

                        WebRequest request = WebRequest.Create(url);
                        request.Method = "GET";
                        WebResponse webResp = request.GetResponse();
                        Rootobject route = null;
                        using (var reader = new StreamReader(webResp.GetResponseStream()))
                        {
                            js = new JavaScriptSerializer();
                            var jsonResponse = reader.ReadToEnd();
                            route = (Rootobject)js.Deserialize(jsonResponse, typeof(Rootobject));
                        }
                        line = string.Concat(baseStation.Stationnumber.Value.ToString(), "  ", destinationStation.Stationnumber.Value.ToString(), "  ", route.properties.distance);
                        File.AppendAllText(@"data.txt", line + Environment.NewLine);
                    }           
                }            
            }
        }
    }
}
