using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NavigationResolver.Interfaces;
using Priority_Queue;

namespace NavigationResolver.Algorithms
{
    class DijkstraNode : FastPriorityQueueNode
    {
        public int Vertex { get; }

        public DijkstraNode(int v)
        {
            Vertex = v;
        }
    }

    class Dijkstra
    {
        public static List<int> FindBestPath(IGraph graph, int src, int dst)
        {
            if (src == dst)
                return new List<int>() { src };

            int n = graph.AllVertexes();
            double[] dist = new double[n];
            int[] prev = new int[n];
            DijkstraNode[] nodes = new DijkstraNode[n];

            for (int i = 0; i < n; ++i)
            {
                dist[i] = Double.PositiveInfinity;
                prev[i] = -1;
                nodes[i] = new DijkstraNode(i);
            }

            dist[src] = 0.0;

            var Q = new FastPriorityQueue<DijkstraNode>(n);
            Q.Enqueue(nodes[src], 0.0);

            while (Q.Count > 0)
            {
                var u = Q.Dequeue().Vertex;

                if (u == dst)
                {
                    // We reached destination, time to reconstruct the path
                    var path = new LinkedList<int>();
                    for (int v = u; v != -1; v = prev[v])
                        path.AddFirst(v); //Add(v);
                    return path.ToList();
                }

                foreach (var v in graph.Neighbors(u))
                {
                    var alt = dist[u] + graph.EdgeCost(u, v);
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                        if (Q.Contains(nodes[v]))
                            Q.UpdatePriority(nodes[v], alt);
                        else
                            Q.Enqueue(nodes[v], alt);
                    }
                }
            }

            throw new Exception("Path not found");
        }
    }
}
