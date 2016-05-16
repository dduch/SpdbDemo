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

        public NetworkRouteBuilder(Network net)
        {
            _net = net;
        }

        public float[] BuildRoute(Point src, Point dst)
        {
            var vsrc = _net.NearestNode(src);
            var vdst = _net.NearestNode(dst);
            var path = Dijkstra.FindBestPath(_net, vsrc, vdst);
            var points = path.Select(node => _net.GetNodePosition(node)).ToList();

            if (!points.First().Equals(src))
                points.Insert(0, src);
            if (!points.Last().Equals(dst))
                points.Add(dst);

            var route = new Route(points);
            return route.Serialize();
        }

    }
}
