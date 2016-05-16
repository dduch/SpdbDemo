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
    class Network : IGraph
    {
        private List<Node> nodes;

        public Network(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public IEdgeCost EdgeCost(int vsrc, int vdst)
        {
            var srcNode = nodes[vsrc];
            var toDstArch = srcNode.Archs.Find(arch => arch.To == vdst);  
            return new EdgeCost(toDstArch.Content.GetLength());
        }

        public IEdgeCost EstimateCost(int vsrc, int vdst)
        {
            return new EdgeCost(nodes[vsrc].Position.GetDistanceTo(nodes[vdst].Position));
        }

        public List<int> Neighbors(int v)
        {
            return nodes[v].Archs.Select(arch => arch.To).ToList();
        }

        public int VerticesCount()
        {
            return nodes.Count();
        }

        public int NearestNode(Point position)
        {
            int bestNode = -1;
            double bestDist = double.PositiveInfinity;

            for(int i = 0; i < nodes.Count; ++i)
            {
                var currDist = position.GetDistanceTo(nodes[i].Position);
                if (currDist < bestDist)
                {
                    bestNode = i;
                    bestDist = currDist;
                }
            }

            return bestNode;
        }

        public Route GetArch(int vsrc, int vdst)
        {
            var srcNode = nodes[vsrc];
            var toDstArch = srcNode.Archs.Find(arch => arch.To == vdst);
            return toDstArch.Content;
        }

        public Point GetNodePosition(int nodeIdx)
        {
            return nodes[nodeIdx].Position;
        }
    }
}
