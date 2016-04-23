using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationResolver.DataModels;
using NavigationResolver.Interfaces;
using NavigationResolver.Types;

namespace NavigationResolver.DataModels
{
    public class SuperGraph : IGraph
    {
        TravelMetric metric;

        List<SubGraph> subGraphs;

        public SuperGraph() { }

        public SuperGraph(List<SubGraph> subGraphs, TravelMetric metric)
        {
            this.metric = metric;
            this.subGraphs = subGraphs;
        }

        public int AllVertexes()
        {
            return subGraphs.Count;
        }

        public double EdgeCost(int vsrc, int vdst)
        {
            var archId = subGraphs[vsrc].NeighborGraphs.FindIndex(v => v == vdst);
            var archLenght = subGraphs[vsrc].IntergraphArchs[archId].Arch.GetLength();
            var t = archLenght / metric.Velocity;
            return metric.CostFunction(t);
        }

        public double EstimateCost(int vsrc, int vdst)
        {
            // TODO: Does edge always exist?
            return EdgeCost(vsrc, vdst);
        }

        public List<int> Neighbors(int v)
        {
            return subGraphs[v].NeighborGraphs;
        }
    }
}
