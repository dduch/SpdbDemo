using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Navigation.DataModels;
using System.Xml.Linq;
using Navigation.Properties;
using System.Globalization;

namespace Navigation.DataProviders
{
    public class StationsManager
    {
        private static List<Station> _stations;
        private static object _fetchLock = new object();
        private static object _statusLock = new object();
        private static bool _fetching = false;
        private static Stopwatch _timer = new Stopwatch();

        private enum Status { NEW, OLD, EXPIRED };

        private static Status GetStatus()
        {
            Status status;

            lock(_statusLock)
            {
                if (_timer.IsRunning)
                {
                    if (_timer.ElapsedMilliseconds < 120 * 1000) // 2 min
                        status = Status.NEW;
                    else if (_timer.ElapsedMilliseconds < 240 * 1000) // 4 min
                        status = Status.OLD;
                    else
                        status = Status.EXPIRED;
                }
                else
                    status = Status.EXPIRED;
            }

            return status;
        }

        // Blocking function for fetching stations
        private static void Fetch(bool supressErrors = false)
        {
            try
            {
                lock(_fetchLock)
                {
                    _fetching = true;
                    if (GetStatus() != Status.NEW)
                    {
                        // 1. Fetch stations
                        XElement xdoc = XElement.Load(Settings.Default.MapOfStationsUrl);
                        IEnumerable<XElement> stationsXml = (from e in xdoc.Elements("country")
                                                             where (string)e.Attribute("country") == "PL"
                                                             from c in e.Elements("city")
                                                             where (int)c.Attribute("uid") == Settings.Default.WarsawId ||
                                                                (int)c.Attribute("uid") == Settings.Default.BemowoId
                                                             from p in c.Elements("place")
                                                             select p).ToList();

                        var stationsList = new List<Station>(stationsXml.Count());
                        foreach (XElement node in stationsXml)
                        {
                            var stationId = Convert.ToInt32(node.Attribute("number").Value, CultureInfo.InvariantCulture);

                            stationsList.Add(new Station(
                                node.Attribute("name").Value,
                                stationId,
                                Convert.ToDouble(node.Attribute("lat").Value, CultureInfo.InvariantCulture),
                                Convert.ToDouble(node.Attribute("lng").Value, CultureInfo.InvariantCulture),
                                node.Attribute("bikes").Value != "0"
                            ));
                        }
                        _stations = stationsList;

                        // 2. Restart timer
                        lock (_statusLock)
                        {
                            _timer.Restart();
                        }
                    }
                    _fetching = false;
                }
            }
            catch (Exception ex)
            {
                if(!supressErrors)
                    throw new Exception("Retrieving stations failed. Reason: " + ex.Message, ex);
            }

        }

        // Non blocking function for fetching stations
        public static void Prefetch()
        {
            if (_fetching)
                return;

            Thread fetcher = new Thread(() => Fetch(true));
            fetcher.Start();
        }


        public static IEnumerable<Station> Get()
        {
            switch(GetStatus())
            {
                case Status.NEW: break;
                case Status.OLD: Prefetch(); break;
                case Status.EXPIRED: Fetch();  break;
            }
            return _stations.Take(20);
        }
    }
}
