using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INavigation;
using Navigation.DataModels;
using Navigation.Graph;

namespace Navigation.Network
{
    class SuperGraph : IGraph
    {
        TravelMetric metric;

        public List<SubGraph> SubGraphs { get; }

        public SuperGraph() { }

        public SuperGraph(List<SubGraph> subGraphs, TravelMetric metric)
        {
            this.metric = metric;
            this.SubGraphs = subGraphs;
        }

        public int AllVertexes()
        {
            return SubGraphs.Count;
        }

        public double EdgeCost(int vsrc, int vdst)
        {
            var archId = SubGraphs[vsrc].NeighborGraphs.FindIndex(v => v == vdst);
            var archLenght = SubGraphs[vsrc].IntergraphArchs[archId].Arch.GetLength();
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
            return SubGraphs[v].NeighborGraphs;
        }

        public Tuple<int,int> GetVertexByPosition(Point pos)
        {
            var gid = -1;
            var vid = -1;
            for(int i = 0; i < SubGraphs.Count; ++i)
            {
                vid = SubGraphs[i].GetVertexByPosition(pos);
                if (vid >= 0)
                {
                    gid = i;
                    break;
                }
            }

            return new Tuple<int, int>(gid, vid);
        }
    }
}
