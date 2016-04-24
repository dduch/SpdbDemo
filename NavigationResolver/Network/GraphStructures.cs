using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INavigation;

/* Structures for organizing data in graphs */

namespace Navigation.Network
{
    class Vertex
    {
        public Point Position;
        public List<int> Neighbors;
        public List<IRoute> Archs;

        public Vertex(Point position)
        {
            Position = position;
            Neighbors = new List<int>();
            Archs = new List<IRoute>();
        }
    }

    class IntergraphEdge
    {
        public int StartVertex;
        public int EndVertex;
        public IRoute Arch;

        public IntergraphEdge(int startV, int endV, IRoute arch)
        {
            StartVertex = startV;
            EndVertex = endV;
            Arch = arch;
        }
    }
}
