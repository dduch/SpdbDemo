using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;

namespace NavigationResolver.DataModels
{
    public sealed class Network : INetwork
    {
        private static volatile Network instance;
        private static object syncRoot = new Object();

        private SuperGraph graph;
        private IGeoDataProvider geoData;
        private TravelMetric metric;

        public Network(SuperGraph graph, IGeoDataProvider geoData, TravelMetric metric)
        {
            this.graph = graph;
            this.geoData = geoData;
            this.metric = metric;       
        }

        // Creates new instance of network with given parameters.
        public static INetwork Build(IGeoDataProvider geoData, TravelMetric metric)
        {
            var builder = new NetworkBuilder(geoData, metric);
            var subGraphs = builder.BuildSubGraphs();
            var superGraph = new SuperGraph(subGraphs, metric);
            return new Network(superGraph, geoData, metric);
        }

        // Gets existing instance of network
        // or creates new one with given parameters if no exists.
        // Locking prevents from unintentional creation of multiple default insances.
        public static INetwork Get(IGeoDataProvider geoData, TravelMetric metric)
        {
            if (instance == null)
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = Build(geoData, metric) as Network;
                }
            return instance;
        }

        public IRoute GetBestRoute(Point source, Point destination)
        {
            throw new NotImplementedException();
        }
    }
}
