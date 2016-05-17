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
using INavigation;
using Navigation.DataProviders;
using System.Diagnostics;
using LocalConncetionBaseBuilder.Net;

namespace LocalConncetionBaseBuilder
{
    class Program
    {

        static void Main(string[] args)
        {
            Station[] stations = StationsManager.SafeGet().ToArray();
            BaseBuilder builder = new BaseBuilder(stations, "dbfile", new ServiceRouteBuilder());

            Console.WriteLine("Connection builder started");
            Console.WriteLine(stations.Length + " stations loaded");
            

            while (true)
            {
                try
                {

                    var cmd = Console.ReadLine().Split(new char[] { ' ' });

                    if (cmd[0] == "download")
                    {
                        builder = new BaseBuilder(stations, "dbfile", new ServiceRouteBuilder());
                    }
                    else if (cmd[0] == "start")
                    {
                        builder.StartConstruction();
                    }
                    else if (cmd[0] == "continue")
                    {
                        builder.ContinueConstruction();
                    }
                    else if (cmd[0] == "pause")
                    {
                        builder.StopConstruction();
                    }
                    else if (cmd[0] == "update")
                    {
                        builder.Update();
                    }
                    else if (cmd[0] == "status")
                    {
                        var status = builder.Status();
                        Console.WriteLine((status.Item1 ? "Running" : "Stopped") + ". Progress: " + status.Item2 + "%");
                    }
                    else if (cmd[0] == "exit")
                    {
                        var status = builder.Status();
                        if (status.Item1 == true) // download is running
                        {
                            Console.WriteLine("Download is running. Are you sure (Y/N)?");
                            var response = Console.ReadLine();
                            if (response == "Y")
                            {
                                builder.StopConstruction();
                                break;
                            }
                        }
                        else
                            break;
                    }
                    else if (cmd[0] == "help")
                    {
                        Console.WriteLine("Avaliable commands:");
                        Console.WriteLine("* from <resource> - switches program to build connections db from local resource");
                        Console.WriteLine("* download - switches program to build connections uisng web service");
                        Console.WriteLine("* start - starts building new db");
                        Console.WriteLine("* continue - continues paused build");
                        Console.WriteLine("* pause - stops build");
                        Console.WriteLine("* update - updates and cleans existing database");
                        Console.WriteLine("* status - shows current build progress");
                        Console.WriteLine("* exit - exists programm, stops download if running");
                        Console.WriteLine("* help - displays this help");
                    }
                    else if (cmd[0] == "from")
                    {
                        var xmlBuilder = new NetworkBuilder();
                        var net = xmlBuilder.BuildNetworkFromXml(cmd[1]);
                        builder = new BaseBuilder(stations, "dbfile", new NetworkRouteBuilder(net, stations), false);
                        xmlBuilder = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Console.WriteLine("Resource loaded successfully. You can start building.");
                    }
                    else if (cmd[0] == "test")
                    {
                        var xmlBuilder = new NetworkBuilder();
                        var net = xmlBuilder.BuildNetworkFromXml("mapfile.osm");
                        xmlBuilder = null;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        builder = new BaseBuilder(stations, "dbfile", new NetworkRouteBuilder(net, stations), false);
                        Console.WriteLine("Resource loaded successfully. Starting build ...");
                        builder.StartConstruction();
                    }
                    else
                    {
                        Console.WriteLine("Unknown command: '" + cmd[0] + "'. Try 'help'");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception was caught: " + ex.Message);
                }
            }
        }

    //    static void Special(Station[] stations)
    //    {
    //        var xmlBuilder = new NetworkBuilder();
    //        var net = xmlBuilder.BuildNetworkFromXml("mapfile.osm");
    //        var builder = new BaseBuilder(stations, "dbfile", new NetworkRouteBuilder(net), false);
    //        Console.WriteLine("Resource loaded successfully. Starting build ...");
    //        builder.StartConstruction();
    //    }
    }
}
