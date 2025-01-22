
namespace Miths.DataStructure
{
    public abstract class Adjacency<T> where T : IComparable<T>, IEquatable<T>
    {
        protected List<T> bfsLst = [];
        protected List<T> dfsLst = [];

        public bool Directed { get; private set; } = true;
        public abstract void AddNodes(T[] nodes);
        public abstract void AddNode(T node);
        public abstract void RemoveNode(T node);
        public abstract void AddAdjacency(T node, T adjc, float weight);
        public virtual void AddAdjacency(T node, T adjc) => AddAdjacency(node, adjc, 1);
        public abstract void RemoveAdjacency(T node, T adjc);
        public abstract void AddAdjacencies(List<(T node, T adjc, float weight)> adjacencies);
        public abstract List<T>? BFS(T root);
        public abstract List<T>? DFS();
        public abstract List<T>? GetPath(T from, T to);
        public abstract bool IsBipartite(bool forceMake);
        public abstract bool IsBipartite(T root, bool forceMake = false);
        public abstract List<T> TopologicalSort(T node);
        public abstract T? FirstNode();
        protected abstract void InitDFS();
        public abstract Dictionary<T, (float start, float final)>? GetDFSTimes();
        public T[] GetBFS() => bfsLst.ToArray();
        public T[] GetDFS() => dfsLst.ToArray();
        public abstract void MSTPrims(T root);
        public abstract HashSet<((T?, T?), float)> MSTKruskals();
        public abstract string TostringMSTPrims();
        public abstract string TostringMSTKruskals();
        public Adjacency(T[] nodes, bool directed)
        {
            Directed = directed;
        }
    }
}
