using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using INavigation;
using System.IO;
using Navigation.DataModels;
using Navigation.DataProviders;

namespace LocalConncetionBaseBuilder
{
    class BaseBuilder
    {
        private bool process = false;
        private string statusfile = "status.txt";
        private string dbfile;
        private double progress = 0.0;
        private Station[] stations;
        private Thread downloader = null;
        private IRouteBuilder routeBuilder;
        private readonly int postExceptionSleepTime = 2 * 1000; // 2 s
        private bool retry;

        public BaseBuilder(Station[] stations, string dbfile, IRouteBuilder routeBuilder, bool retry = true)
        {
            this.stations = stations;
            this.dbfile = dbfile;
            this.routeBuilder = routeBuilder;
            this.retry = retry;
        }

        public void StartConstruction()
        {
            // Reset dbfile and progress
            File.Delete(dbfile);
            //File.Create(dbfile);
            File.WriteAllText(statusfile, "0 0");
            progress = 0.0;

            process = true;
            downloader = new Thread(() => Construct());
            downloader.Start();
        }

        public void StopConstruction()
        {
            process = false;
            if (downloader != null && (downloader.ThreadState == ThreadState.Running || downloader.ThreadState == ThreadState.WaitSleepJoin))
                downloader.Join();
            downloader = null;
        }

        public void ContinueConstruction()
        {
            if (downloader == null)
            {
                process = true;
                downloader = new Thread(() => Construct());
                downloader.Start();
            }
        }

        public void Update()
        {
            string tmpfile = dbfile + ".old";
            HashSet<KeyValuePair<int, int>> downloadedConnections = new HashSet<KeyValuePair<int, int>>();
            File.Copy(dbfile, tmpfile, true);
            File.Delete(dbfile);
            BinaryReader reader = new BinaryReader(new FileStream(tmpfile, FileMode.Open, FileAccess.Read));
            BinaryWriter writer = new BinaryWriter(new FileStream(dbfile, FileMode.Create, FileAccess.Write));
            int redundantConnectionsCnt = 0;

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                int idOfsett = 6000;
                int id1 = (int)reader.ReadUInt16() + idOfsett;
                int id2 = (int)reader.ReadUInt16() + idOfsett;
                int n = (int)reader.ReadUInt16();
                float[] points = new float[n];
                for (int k = 0; k < n; ++k)
                {
                    points[k] = reader.ReadSingle();
                }

                var con = new KeyValuePair<int, int>(id1, id2);
                if (!downloadedConnections.Contains(con))
                {
                    writer.Write(Convert.ToUInt16(id1 - idOfsett));
                    writer.Write(Convert.ToUInt16(id2 - idOfsett));
                    writer.Write(Convert.ToUInt16(n));
                    for (int k = 0; k < n; ++k)
                        writer.Write(points[k]);

                    downloadedConnections.Add(new KeyValuePair<int, int>(id1, id2));
                }
                else
                    ++redundantConnectionsCnt;           
            }
            reader.Close();
            writer.Close();

            Console.WriteLine("Cleanup completed. Found " + redundantConnectionsCnt + " redundant connections");

            int allConnections = stations.Length * stations.Length - stations.Length;
            Console.WriteLine("Constructing " + (allConnections - downloadedConnections.Count) + " missing connections");

            File.WriteAllText(statusfile, "0 0");
            progress = 0.0;
            process = true;
            downloader = new Thread(() => Construct(downloadedConnections));
            downloader.Start();
        }

        public Tuple<bool,double> Status()
        {
            return new Tuple<bool, double>(process, progress);
        }


        private void Construct(HashSet<KeyValuePair<int, int>> downloadedConnections = null)
        {
            string line = string.Empty;
            int n = stations.Length;
            var status = File.ReadAllText(statusfile).Split(new char[] { ' ' });
            int lastI = Convert.ToInt32(status[0]);
            int lastJ = Convert.ToInt32(status[1]);
            int j = (lastJ + 1) % n;
            int i = (lastJ + 1) >= n ? lastI + 1 : lastI;

            var wr = new BinaryWriter(new FileStream(dbfile, FileMode.Append, FileAccess.Write));

            Console.WriteLine("Started constructing from [" + i + "] [" + j + "]");

            bool completed = false;
            while (process || !completed)
            {
                try
                {
                    while (i < n)
                    {
                        while (j < n)
                        {
                            if (i != j)
                            {
                                if (!process)
                                {
                                    i = (j - 1) >= 0 ? i : i - 1;
                                    j = (j - 1) >= 0 ? j - 1 : n - 1;
                                    completed = true;
                                    throw new ThreadInterruptedException();
                                }

                                var src = stations[i];
                                var dst = stations[j];

                                if (downloadedConnections != null && downloadedConnections.Contains(new KeyValuePair<int, int>(src.Id, dst.Id)))
                                {
                                    ++j;
                                    continue; // We already have this connection db and can skip it
                                }

                                var route = routeBuilder.BuildRoute(src.Position, dst.Position);

                                int idOffset = 6000;
                                wr.Write(Convert.ToUInt16(src.Id - idOffset));
                                wr.Write(Convert.ToUInt16(dst.Id - idOffset));
                                wr.Write(Convert.ToUInt16(route.Length));
                                for (int k = 0; k < route.Length; ++k)
                                    wr.Write(route[k]);

                                progress = 100.0 * (i * n + (j + 1)) / ((n * n));
                            }
                            ++j;
                        }
                        j = 0;
                        ++i;
                    }
                    process = false;
                    completed = true;
                    progress = 100.0;
                    Console.WriteLine("Construction completed");
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Construction interrupted after [" + i + "] [" + j + "]");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception for connection [" + i + "] [" + j + "] : " + ex.Message);

                    if(retry)
                    {
                        // Sleep some time after exception occured
                        // and then try to get connection again
                        Thread.Sleep(postExceptionSleepTime);
                    }
                    else
                    {
                        // Proceed to next connection
                        ++j;
                    }                  
                }
            }
            wr.Close();
            File.WriteAllText(statusfile, i + " " + j);
            Console.WriteLine("Progress saved successfully");
        }
    }
}
