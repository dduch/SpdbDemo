using LocalConncetionBaseBuilder.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Navigation.DataModels;
using System.Threading;

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
            ["v"] = "bicycle",
            ["fast"] = "0",
            ["format"] = "geojson",
            ["geometry"] = "1",
            ["distance"] = "v",
            ["instructions"] = "0",
            ["lang"] = "pl",
        };

        static bool process = false;
        static string strfile = "data.txt";
        static string binfile = "data";
        static string statusfile = "status.txt";
        static double progress = 0.0;
        static StationInfo[] stations;

        static void Main(string[] args)
        {
            LoadStations();
            Thread worker = null;

            Console.WriteLine("Connection builder started");
            Console.WriteLine(stations.Length + " stations loaded");

            while (true)
            {
                var cmd = Console.ReadLine();

                if(cmd == "start")
                {
                    File.Delete(strfile);
                    File.WriteAllText(statusfile, "0 0");
                    process = true;
                    worker = new Thread(DownloadData);
                    worker.Start();
                }
                else if (cmd == "continue")
                {
                    process = true;
                    worker = new Thread(DownloadData);
                    worker.Start();
                }
                else if (cmd == "pause")
                {
                    process = false;
                    worker = null;
                }
                else if (cmd == "status")
                {
                    Console.WriteLine((process ? "Running" : "Stopped") + ". Progress: " + progress + "%");
                }
                else if (cmd == "stop")
                {
                    process = false;
                    if (worker != null && (worker.ThreadState == ThreadState.Running || worker.ThreadState == ThreadState.WaitSleepJoin))
                        worker.Join();
                    break;
                }
                else if (cmd == "abort")
                {
                    process = false;
                    break;
                }
                else if (cmd == "convert")
                {
                    ConvertData();
                }
                else if (cmd == "help")
                {
                    Console.WriteLine("Avaliable commands:\n* start\n* stop\n* pause\n* continue\n* status\n* convert\n* abort");
                }
                else
                {
                    Console.WriteLine("Unknown command: " + cmd + " Try 'help'");
                }

            }
        }

        static void LoadStations()
        {
            VeturiloStations veturiloStations;
            JavaScriptSerializer js = new JavaScriptSerializer();
            string text = Encoding.UTF8.GetString(Resources.stations);
            veturiloStations = (VeturiloStations)js.Deserialize(text, typeof(VeturiloStations));

            stations = veturiloStations.Stations;
        }


        static void DownloadData()
        {
            bool completed = false;
            string line = string.Empty;

            var status = File.ReadAllText(statusfile).Split(new char [] { ' ' });
            int lastI = Convert.ToInt32(status[0]);
            int lastJ = Convert.ToInt32(status[1]);
            int j = (lastJ + 1) % stations.Length;
            int i = (lastJ + 1) >= stations.Length ? lastI + 1 : lastI;

            Console.WriteLine("Started downloading from: " + i + " " + j);

            while (!completed && process)
            {
                try
                {
                    while (i < stations.Length)
                    {
                        while (j < stations.Length)
                        {

                            if (i != j)
                            {
                                var baseStation = stations[i];
                                var destinationStation = stations[j];

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
                                    var js = new JavaScriptSerializer();
                                    var jsonResponse = reader.ReadToEnd();
                                    route = (Rootobject)js.Deserialize(jsonResponse, typeof(Rootobject));
                                }

                                line = string.Concat(baseStation.Stationnumber.Value.ToString(), " ", destinationStation.Stationnumber.Value.ToString(), " ", route.properties.distance);
                                File.AppendAllText(strfile, line + Environment.NewLine);

                                progress = 100.0 * (i * stations.Length + (j + 1)) / ((stations.Length * stations.Length));

                                if (!process)
                                    throw new ThreadInterruptedException();

                                if ((i * stations.Length + (j + 1)) % 10 == 0)
                                    Thread.Sleep(1000 * 1);
                            }
                            ++j;
                        }
                        j = 0;
                        ++i;
                    }
                    completed = true;
                    Console.WriteLine("Download completed");
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Download interrupted after: " + i + " " + j);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured: " + ex.Message);
                    Thread.Sleep(1000 * 30);
                }
            }
            File.WriteAllText(statusfile, i + " " + j);
            Console.WriteLine("Progress saved successfully");
        }

        static void ConvertData()
        {
            BinaryWriter wr = new BinaryWriter(new FileStream(binfile, FileMode.Create));

            using (var readStream = File.OpenRead(strfile))
            using (var streamReader = new StreamReader(readStream))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] content = line.Split(' ');
                    int id1 = Convert.ToInt32(content[0]);
                    int id2 = Convert.ToInt32(content[1]);
                    double dist = Convert.ToDouble(content[2], new System.Globalization.NumberFormatInfo()) * 1000.0; // Convert [km] to [m]
                    wr.Write(id1);
                    wr.Write(id2);
                    wr.Write(dist);
                }              
             }

            wr.Close();

            Console.WriteLine("Conversion successful");
        }
    }
}
