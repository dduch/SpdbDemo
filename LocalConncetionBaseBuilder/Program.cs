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

namespace LocalConncetionBaseBuilder
{
    class Program
    {

        static void Main(string[] args)
        {
            Station[] stations = StationsManager.Get().ToArray();
            BaseBuilder builder = new BaseBuilder(stations, "dbfile");

            Console.WriteLine("Connection builder started");
            Console.WriteLine(stations.Length + " stations loaded");
            

            while (true)
            {
                try
                {

                    var cmd = Console.ReadLine();

                    if (cmd == "start")
                    {
                        builder.StartDownload();
                    }
                    else if (cmd == "continue")
                    {
                        builder.ContinueDownload();
                    }
                    else if (cmd == "pause")
                    {
                        builder.StopDownload();
                    }
                    else if (cmd == "update")
                    {
                        builder.Update();
                    }
                    else if (cmd == "status")
                    {
                        var status = builder.Status();
                        Console.WriteLine((status.Item1 ? "Running" : "Stopped") + ". Progress: " + status.Item2 + "%");
                    }
                    else if (cmd == "exit")
                    {
                        var status = builder.Status();
                        if (status.Item1 == true) // download is running
                        {
                            Console.WriteLine("Download is running. Are you sure (Y/N)?");
                            var response = Console.ReadLine();
                            if (response == "Y")
                            {
                                builder.StopDownload();
                                break;
                            }
                        }
                        else
                            break;
                    }
                    else if (cmd == "help")
                    {
                        Console.WriteLine("Avaliable commands:");
                        Console.WriteLine("* start - starts new download");
                        Console.WriteLine("* continue - continues paused download");
                        Console.WriteLine("* pause - stops download");
                        Console.WriteLine("* update - updates and cleans existing database");
                        Console.WriteLine("* status - shows current download progress");
                        Console.WriteLine("* exit - exists programm, stops download if running");
                        Console.WriteLine("* help - displays this help");
                    }
                    else if (cmd == "special")
                    {
                        Special(stations);
                    }
                    else
                    {
                        Console.WriteLine("Unknown command: " + cmd + " Try 'help'");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception was caught: " + ex.Message);
                }
            }
        }

        static void Special(Station[] stations)
        {

            //int quant = 1;
            //int shownCategories = 25 / quant;
            //int allCategories = 50 / quant;
            //int[] categories = new int[allCategories];

            //int n = stations.Length;
            //for(int i = 0; i < n; ++i)
            //    for(int j = 0; j < n; ++j)
            //    {
            //        if (i == j)
            //            continue;

            //        var dist = stations[i].Position.GetDistanceTo(stations[j].Position);

            //        var id = (int)(dist / 1000) / quant;

            //        for(int k = id; k < allCategories; ++k)
            //            ++categories[k];
            //    }

            //int all = n * n - n;
            //for(int i = 0; i < shownCategories; ++i)
            //{
            //    Console.WriteLine("Up to " + (i+1)*quant + "km ---> " + categories[i]*100.0/all);
            //}

            int lcnt = 0;
            int gcnt = 0;
            for(int i = 0; i < stations.Length; ++i)
            {
                if (stations[i].Id < 0 + 6000) ++lcnt;
                if (stations[i].Id > 64000 + 6000) ++gcnt;
            }

            Console.WriteLine("lcnt = " + lcnt);
            Console.WriteLine("gcnt = " + gcnt);

        }
    }
}
