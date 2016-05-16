using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Priority_Queue;

namespace Navigation.Graph
{
    public class Dijkstra
    {
        class DijkstraNode : FastPriorityQueueNode
        {
            public int Vertex { get; }

            public DijkstraNode(int v)
            {
                Vertex = v;
            }
        }

        private static List<int> ReconstructPath(int u, int[] predecessors)
        {
            var path = new LinkedList<int>();
            for (int v = u; v != -1; v = predecessors[v])
                path.AddFirst(v);
            return path.ToList();
        }

        public static List<int>[] FindBestPaths(IGraph graph, int src, int[] dst)
        {
            var dstToFound = new HashSet<int>(dst);
            var paths = new List<int>[dst.Length];

            int n = graph.VerticesCount();
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

                if (dstToFound.Contains(u))
                {
                    dstToFound.Remove(u);
                    if(dstToFound.Count == 0)
                    {
                        // We reached all destinations, time to reconstruct the paths
                        paths = new List<int>[dst.Length];
                        for(int i = 0; i < paths.Length; ++i)
                            paths[i] = ReconstructPath(dst[i], prev);

                        return paths;
                    }               
                }

                foreach (var v in graph.Neighbors(u))
                {
                    var alt = dist[u] + graph.EdgeCost(u, v).Value();
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

            // Reaching this point means that some paths were not found
            for (int i = 0; i < paths.Length; ++i)
            {
                var pathTmp = ReconstructPath(dst[i], prev);
                if (pathTmp.First() != src)
                    paths[i] = null;
                else
                    paths[i] = pathTmp;
            }

            return paths;
        }

        public static List<int> FindBestPath(IGraph graph, int src, int dst)
        {
            var result = FindBestPaths(graph, src, new int[] { dst })[0];
            if (result == null)
                throw new Exception("Path does not exists!");

            return result;
        }
    }
}
