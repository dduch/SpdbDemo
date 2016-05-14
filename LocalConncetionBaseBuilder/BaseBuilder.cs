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
        private RouteBuilder routeBuilder = new RouteBuilder();
        private readonly int sleepInterval = 1;
        private readonly int sleepTime = 250; // 250 ms
        private readonly int postExceptionSleepTime = 2 * 1000; // 2 s

        public BaseBuilder(Station[] stations, string dbfile)
        {
            this.stations = stations;
            this.dbfile = dbfile;
        }

        public void StartDownload()
        {
            // Reset dbfile and progress
            File.Delete(dbfile);
            //File.Create(dbfile);
            File.WriteAllText(statusfile, "0 0");
            progress = 0.0;

            process = true;
            downloader = new Thread(DownloadData);
            downloader.Start();
        }

        public void StopDownload()
        {
            process = false;
            if (downloader != null && (downloader.ThreadState == ThreadState.Running || downloader.ThreadState == ThreadState.WaitSleepJoin))
                downloader.Join();
            downloader = null;
        }

        public void ContinueDownload()
        {
            if (downloader == null)
            {
                process = true;
                downloader = new Thread(DownloadData);
                downloader.Start();
            }
        }

        public Tuple<bool,double> Status()
        {
            return new Tuple<bool, double>(process, progress);
        }


        private void DownloadData()
        {
            string line = string.Empty;
            int n = stations.Length;
            var status = File.ReadAllText(statusfile).Split(new char[] { ' ' });
            int lastI = Convert.ToInt32(status[0]);
            int lastJ = Convert.ToInt32(status[1]);
            int j = (lastJ + 1) % n;
            int i = (lastJ + 1) >= n ? lastI + 1 : lastI;

            var wr = new BinaryWriter(new FileStream(dbfile, FileMode.Append, FileAccess.Write));

            Console.WriteLine("Started downloading from: " + i + " " + j);

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
                                    j = (j - 1) >= 0 ? j : n - 1;
                                    completed = true;
                                    throw new ThreadInterruptedException();
                                }

                                var src = stations[i];
                                var dst = stations[j];

                                var route = routeBuilder.BuildRoute(src.Position, dst.Position);

                                int idOffset = 6000;
                                wr.Write(Convert.ToUInt16(src.Id - idOffset));
                                wr.Write(Convert.ToUInt16(dst.Id - idOffset));
                                wr.Write(Convert.ToUInt16(route.Length));
                                for (int k = 0; k < route.Length; ++k)
                                    wr.Write(route[k]);

                                progress = 100.0 * (i * n + (j + 1)) / ((n * n));

                                // Sleep to not overload service
                                if ((i * n + (j + 1)) % sleepInterval == 0)
                                    Thread.Sleep(sleepTime);
                            }
                            ++j;
                        }
                        j = 0;
                        ++i;
                    }
                    process = false;
                    completed = true;
                    progress = 100.0;
                    Console.WriteLine("Download completed");
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("Download interrupted after: " + i + " " + j);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception occured: " + ex.Message);
                    // Sleep some time after exception occured
                    Thread.Sleep(postExceptionSleepTime);
                }
            }
            wr.Close();
            File.WriteAllText(statusfile, i + " " + j);
            Console.WriteLine("Progress saved successfully");
        }
    }
}
