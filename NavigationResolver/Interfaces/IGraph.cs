using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationResolver.Interfaces
{
    // Interface required by graph searching algorithms
    // Integer number are used to describe vertexes
    public interface IGraph
    {
        // Returns number of vertexes in graph
        // If result is n then graph has vertexes in 0..n-1
        int AllVertexes();

        // Returns all neighbor vertexes of v
        List<int> Neighbors(int v);

        // Returns cost of edge between vertexes vsrc and vdst
        // Edge between vsrc and vdst must exist
        double EdgeCost(int vsrc, int vdst);

        // Returns heuristic cost approximation of getting from vsrc to vdst
        // Edge between vsrc and vdst may not exist
        double EstimateCost(int vsrc, int vdst);
    }
}
