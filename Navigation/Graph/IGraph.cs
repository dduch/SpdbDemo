﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation.Graph
{
    // Interface required by graph searching algorithms
    // Integer number are used to describe vertexes
    public interface IGraph
    {
        // Returns number of vertexes in graph
        // If result is n then graph has vertexes in 0..n-1
        int VerticesCount();

        // Returns all neighbor vertexes of v
        List<int> Neighbors(int v);

        // Returns cost of edge between vertexes vsrc and vdst
        // Edge between vsrc and vdst must exist
        IEdgeCost EdgeCost(int vsrc, int vdst);

        // Returns heuristic cost approximation of getting from vsrc to vdst
        // Edge between vsrc and vdst may not exist
        IEdgeCost EstimateCost(int vsrc, int vdst);
    }
}
