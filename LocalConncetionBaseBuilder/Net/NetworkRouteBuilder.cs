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

            Route route = new Route(new List<Point>() { src });
            for(int i = 0; i < path.Count - 1; ++i)
            {
                route.Append(_net.GetArch(path[i], path[i + 1]));
            }
            route.Append(new Route(new List<Point>() { dst }));

            return route.Serialize();
        }

    }
}
