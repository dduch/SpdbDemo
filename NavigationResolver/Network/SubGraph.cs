using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;
using Navigation.Graph;

namespace Navigation.Network
{
    class SubGraph : IGraph
    {
        double velocity;

        double changePenalty;

        public List<Vertex> Vertices { get; }

        public List<int> NeighborGraphs { get; }

        public List<IntergraphEdge> IntergraphArchs { get; }

        // TODO: Add implementation of mbb mechanism for graph
        //public Mbb { get; set; }

        public SubGraph(double velocity, double changePenalty)
        {
            this.velocity = velocity;
            this.changePenalty = changePenalty;
            Vertices = new List<Vertex>();
            NeighborGraphs = new List<int>();
            IntergraphArchs = new List<IntergraphEdge>();
        }


        public int AllVertexes()
        {
            return Vertices.Count;
        }

        public double EdgeCost(int vsrc, int vdst)
        {
            var archId = Vertices[vsrc].Neighbors.FindIndex(v => v == vdst);
            var archLenght = Vertices[vsrc].Archs[archId].GetLength();
            var totalCost = archLenght / velocity + changePenalty;
            return totalCost;
        }

        public double EstimateCost(int vsrc, int vdst)
        {
            // If egde from vsrc to vdest exists return exact cost
            var archId = Vertices[vsrc].Neighbors.FindIndex(v => v == vdst);
            if (archId >= 0)
            {
                var archLenght = Vertices[vsrc].Archs[archId].GetLength();
                var exactCost = archLenght / velocity + changePenalty;
                return exactCost;
            }
            // Otherwise return heuristic best case approximation
            var directDistance = Vertices[vsrc].Position.GetDistanceTo(Vertices[vdst].Position);
            var approximateCost = directDistance / velocity + changePenalty;
            return approximateCost;
        }

        public List<int> Neighbors(int v)
        {
            return Vertices[v].Neighbors;
        }

        public int GetVertexByPosition(Point pos)
        {
            return Vertices.FindIndex(vrtx => vrtx.Position.Equals(pos));
        }
    }
}
