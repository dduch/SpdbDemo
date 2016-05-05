using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;
using Navigation.Graph;
using Navigation.DataProviders;

namespace Navigation.DataModels
{
    class Network : IGraph
    {
        private IGeoDataProvider geoData;

        private TravelMetric metric;

        private Station[] nodes;

        // This is a precomputation for optimization
        // In 1st model we assume that each node is neighbor to all,
        // so to avoid computing all neighbors of vertex we do it once
        private List<int> neighbors;


        public Network(IGeoDataProvider geoData, TravelMetric metric)
        {
            this.geoData = geoData;
            this.metric = metric;
            nodes = geoData.GetStations().ToArray();

            neighbors = new List<int>(nodes.Length);
            for (int i = 0; i < nodes.Length; ++i)
                neighbors.Add(i);
        }


        public int VerticesCount()
        {
            return nodes.Length;
        }

        public Cost EdgeCost(int vsrc, int vdst)
        {
            var srcId = nodes[vsrc].Id;
            var dstId = nodes[vdst].Id;
            var pathLength = geoData.GetPathLength(srcId, dstId);
            var pathCost = metric.PathCost(pathLength);
            return new Cost(pathCost, pathLength + metric.ChangeDistanceEquivalent);
        }

        public Cost EstimateCost(int vsrc, int vdst)
        {
            var srcId = nodes[vsrc].Id;
            var dstId = nodes[vdst].Id;
            var pathLength = geoData.GetPathLength(srcId, dstId);
            // As heuristic cost function has to be admissible
            // we must assume lowest possible fee = 0.0
            return new Cost(0.0, pathLength); 
        }

        public List<int> Neighbors(int v)
        {
            var n = neighbors.Where(x => x != v).ToList();
            return n;
        }

        public Station MapNodeToStation(int nodeId)
        {
            return nodes[nodeId];
        }

        public int MapStationToNode(int stationId)
        {
            for (int i = 0; i < nodes.Length; ++i)
                if (nodes[i].Id == stationId)
                        return i;
            throw new ArgumentException("Unknown station id: " + stationId);
        }
    }
}
