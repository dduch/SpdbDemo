using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;
using Navigation.Graph;
using Navigation.DataModels;

namespace LocalConncetionBaseBuilder.Net
{
    class NetworkRouteBuilder : IRouteBuilder
    {
        private Network _net;
        private List<Point> _locations;
        private int[] _nearestNodes;
        private Point _lastSrc = null;
        private List<int>[] _resultsCache = null;

        public NetworkRouteBuilder(Network net, Station[] stations)
        {
            _net = net;
            _locations = stations.Select(s => s.Position).ToList();
            _nearestNodes = new int[_locations.Count];
            for(int i = 0; i < _nearestNodes.Length; ++i)
                _nearestNodes[i] = _net.NearestNode(_locations[i]);
        }

        public float[] BuildRoute(Point src, Point dst)
        {
            var dstKey = _locations.FindIndex(p => dst.Equals(p));

            // Cache miss
            if (!src.Equals(_lastSrc))
            {
                var srcKey = _locations.FindIndex(p => src.Equals(p));
                _lastSrc = src;
                _resultsCache = Dijkstra.FindBestPaths(_net, _nearestNodes[srcKey], _nearestNodes);
            }

            var path = _resultsCache[dstKey];

            if (path == null)
                throw new Exception("Path not found");

            Route route = new Route(new List<Point>() { src });
            for (int i = 0; i < path.Count - 1; ++i)
            {
                route.Append(_net.GetArch(path[i], path[i + 1]));
            }
            route.Append(new Route(new List<Point>() { dst }));

            return route.Serialize();
        }

    }
}
