using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Miths.DataStructure
{
    public enum GraphType { MATRIX, LIST }
    public enum ColorType { WHITE, GRAY, BLACK }
    public class Graph<T> where T : IComparable<T>, IEquatable<T>
    {
        public GraphType GraphType { get; private set; } = GraphType.LIST;
        public bool Directed => adjacency.Directed;
        Adjacency<T> adjacency;
        public Graph(T[] nodes, GraphType type = GraphType.LIST, bool directed = true) {
            this.GraphType = type;
            switch (type) {
                case GraphType.LIST:
                    adjacency = new AdjacencyList<T>(nodes, directed);
                    break;
                case GraphType.MATRIX:
                    adjacency = new AdjacencyMatrix<T>(nodes, directed);
                    break;
            }
        }
        public void AddEdges(List<(T node, T adjc, float weight)> adjacencies) => adjacency?.AddAdjacencies(adjacencies);
        public void AddEdge(T node, T adjc, float weight) => adjacency?.AddAdjacency(node, adjc, weight);
        public void AddEdge(T node, T adjc) => adjacency?.AddAdjacency(node, adjc, 1);
        public List<T>? BFS(T root) => adjacency?.BFS(root);
        public List<T>? DFS() => adjacency?.DFS();
        public List<T>? GetPath(T from, T to) => adjacency?.GetPath(from, to);
        public virtual string PathToString(T from, T to)
        {
            List<T>? path = GetPath(from, to);
            if (path == null) return "";
            return string.Join(",", path.ToArray());
        }
        public bool IsBipartite(bool forceMake) => IsBipartite(adjacency.FirstNode(), forceMake);
        public bool IsBipartite(T root, bool forceMake = false) => adjacency.IsBipartite(root, forceMake);
        public bool MakeBipartite() => IsBipartite(true);

        public List<T> TopologicalSort(T node) => adjacency.TopologicalSort(node);
        public void MSTPrims(T root) => adjacency?.MSTPrims(root);
        public T[] GetBFS() => adjacency.GetBFS();
        public T[] GetDFS() => adjacency.GetDFS();
        public Dictionary<T, (float start, float final)>? GetDFSTimes() => adjacency.GetDFSTimes();
        public string TostringMSTPrims() => adjacency.TostringMSTPrims();
        public HashSet<((T, T), float)>? MSTKruskals() => adjacency?.MSTKruskals();
        public string? TostringMSTKruskals() => adjacency?.TostringMSTKruskals();
    }    
}
