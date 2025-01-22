
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Miths.DataStructure
{
    public class AdjacencyMatrix<T> : Adjacency<T> where T : IComparable<T>, IEquatable<T>
    {
        ColorType[] colors = [];
        int[] previous = [];
        float[] distance = [];
        (float start, float final)[] dfsTime = [];
        internal List<T> Nodes { get; private set; } = new List<T>();
        public float[,]? Adjacency { get; private set; }
        float[] keyPrims = [];
        int[] parentPrims = [];


        HashSet<((T?, T?), float)> mSTKruskalsSet;

        public override T? FirstNode() => Nodes.Count > 0 ? Nodes.First() : default;
        public AdjacencyMatrix(T[] nodes, bool directed) : base(nodes, directed)
        {
            AddNodes(nodes);
        }
        public override void AddNode(T node)
        {
            if (!Nodes.Contains(node)) Nodes.Add(node);
            Adjacency ??= new float[Nodes.Count, Nodes.Count];
            if (Adjacency.Length != Nodes.Count * Nodes.Count)
            {

                float[,]? adjacency = new float[Nodes.Count, Nodes.Count];
                Array.Copy(Adjacency, adjacency, Adjacency.Length);
                Adjacency = adjacency;
            }
        }

        public override void AddNodes(T[] nodes)
        {
            Nodes = new List<T>(nodes);
            Adjacency = new float[nodes.Length, nodes.Length];
        }
        public override void AddAdjacencies(List<(T node, T adjc, float weight)> adjacencies)
        {
            foreach (var adj in adjacencies)
                AddAdjacency(adj.node, adj.adjc, adj.weight);
        }
        public override void AddAdjacency(T node, T adjc, float weight)
        {
            if (Adjacency == null) return;
            if (!Nodes.Contains(node)) AddNode(node);
            if (!Nodes.Contains(adjc)) AddNode(adjc);
            
            int indNode = Nodes.IndexOf(node);
            int indAdjc = Nodes.IndexOf(adjc);
            Adjacency[indNode, indAdjc] = weight;
            if (!Directed)
                Adjacency[indAdjc, indNode] = weight;
        }

        public override void RemoveAdjacency(T node, T adjc)
        {
            if (Adjacency == null) return;
            int indNode = Nodes.IndexOf(node);
            int indAdjc = Nodes.IndexOf(adjc);
            Adjacency[indNode, indAdjc] = 0;
            if (!Directed)
                Adjacency[indAdjc, indNode] = 0;
        }
        public override void RemoveNode(T node)
        {
            if (!Nodes.Contains(node)) return;
            int indNode = Nodes.IndexOf(node);
        }

        public override List<T>? BFS(T root)
        {
            bfs(indexOf(root));
            return bfsLst;
        }

        public void bfs(int root)
        {
            colors = new ColorType[Nodes.Count];
            previous = new int[Nodes.Count];
            distance = new float[Nodes.Count];
            bfsLst = new List<T>();
            for (int i = 0; i < Nodes.Count; i++)
            {
                colors[i] = ColorType.WHITE;
                previous[i] = -1;
                distance[i] = float.PositiveInfinity;
            }
            colors[root] = ColorType.GRAY;
            distance[root] = 0;
            Queue<int> que = new Queue<int>();
            que.Enqueue(root);
            while (que.Count > 0)
            {
                int u = que.Dequeue();
                bfsLst.Add(valueAt(u));
                List<(int index, float weight)>? adjs = getAdjancts(u);
                if (adjs.Count > 0)
                {
                    foreach (var v in adjs)
                    {
                        if (colors[v.index] == ColorType.WHITE)
                        {
                            colors[v.index] = ColorType.GRAY;
                            distance[v.index] = distance[u] + v.weight;
                            previous[v.index] = u;
                            que.Enqueue(v.index);
                        }
                    }
                }
                colors[u] = ColorType.BLACK;
            }
        }
        int indexOf(T node) => Nodes.IndexOf(node);
        T? valueAt(int index) => index >= 0 && index < Nodes.Count ? Nodes[index] : default;
        List<(int index, float weight)>? getAdjancts(int node)
        {
            if (node < 0 || Adjacency == null || Adjacency.GetLength(1) <= node) return null;
            List<(int index, float weight)> adjs = new List<(int index, float weight)>();
            int len = Adjacency.GetLength(0);
            for (int i = 0; i < len; i++)
            {
                if (Adjacency[node, i] > 0)
                {
                    adjs.Add((i, Adjacency[node, i]));
                }
            }
            return adjs;
        }

        public override List<T>? GetPath(T from, T to)
        {
            List<int>? path = getPath(indexOf(from), indexOf(to));
            if (path == null) return null;
            List<T> pathT = new List<T>();
            for (int i = 0; i < path.Count; i++)
            {
                T? node = valueAt(path[i]);
                pathT.Add(node);
            }
            return pathT;
        }
        public List<int>? getPath(int from, int to)
        {
            if (to < 0 || to >= Nodes.Count || from < 0 || from >= Nodes.Count) return null;
            List<int> path = [];
            if (to == from) path.Add(from);
            else
            {
                List<int>? subPath = getPath(from, previous[to]);
                if (subPath != null)
                {
                    foreach (int val in subPath)
                        path.Add(val);
                }
                path.Add(to);
            }
            return path;
        }

        public override bool IsBipartite(bool forceMake) => IsBipartite(FirstNode(), forceMake);

        public override bool IsBipartite(T root, bool forceMake = false) => isBipartite(indexOf(root), forceMake);
        bool isBipartite(int root, bool forceMake) {
            if (root < 0 || root >= Nodes.Count) return false;
            ColorType[] colors = new ColorType[Nodes.Count];
            for (int i = 0; i < Nodes.Count; i++)
            {
                colors[i] = ColorType.WHITE;
            }
            colors[root] = ColorType.GRAY;
            Queue<int> que = new Queue<int>();
            que.Enqueue(root);
            while (que.Count > 0)
            {
                int u = que.Dequeue();
                List<(int index, float weight)>? adjs = getAdjancts(u);
                if (adjs.Count > 0)
                {
                    foreach (var v in adjs)
                    {
                        if (colors[v.index] == colors[u])
                        {

                            if (forceMake)
                                RemoveAdjacency(valueAt(u), valueAt(v.index));
                            else
                                return false;
                        }
                        if (colors[v.index] == ColorType.WHITE)
                        {
                            colors[v.index] = colors[u] == ColorType.GRAY ? ColorType.BLACK : ColorType.GRAY;
                            que.Enqueue(v.index);
                        }
                    }
                }
            }
            return true;
        }

        public override List<T>? DFS()
        {
            InitDFS();
            int time = 0;
            for (int i = 0; i < Nodes.Count; i++) {
                if (colors[i] == ColorType.WHITE)
                    dfsVisit(i, ref time);
            }
            return dfsLst;
        }

        private void dfsVisit(int i, ref int time)
        {
            colors[i] = ColorType.GRAY;
            time++;
            dfsTime[i] = (time, float.NegativeInfinity);
            dfsLst.Add(valueAt(i));
            List<(int index, float weight)>? adjs = getAdjancts(i);
            foreach (var v in adjs)
            {
                if (colors[v.index] == ColorType.WHITE)
                {
                    previous[v.index] = i;
                    dfsVisit(v.index, ref time);
                }
            }
            colors[i] = ColorType.BLACK;
            time++;
            dfsTime[i] = (dfsTime[i].start, time);
        }

        public override List<T> TopologicalSort(T node) {
            int ind = indexOf(node);
            if(ind < 0 || ind >= Nodes.Count) return new List<T>();
            InitDFS();
            int time = 0;
            if (colors[ind] == ColorType.WHITE)
                dfsVisit(ind, ref time);
            return dfsLst;
        }

        protected override void InitDFS()
        {
            colors = new ColorType[Nodes.Count];
            previous = new int[Nodes.Count];
            dfsTime = new (float start, float final)[Nodes.Count];
            dfsLst = new List<T>();

            for (int i = 0; i < Nodes.Count; i++)
            {
                colors[i] = ColorType.WHITE;
                previous[i] = -1;
                dfsTime[i] = (float.PositiveInfinity, float.NegativeInfinity);
            }
        }
        public void MSTPrims(int root) {
            List<int> que = new List<int>();
            keyPrims = new float[Nodes.Count];
            parentPrims = new int[Nodes.Count];
            for (int i = 0; i < Nodes.Count; i++)
            {
                que.Add(i);
                keyPrims[i] = float.PositiveInfinity;
                parentPrims[i] = -1;
            }

            keyPrims[root] = 0;
            while (que.Count > 0)
            {
                int u = extractMinKey();
                List<(int Key, float Value)>? adjs = getAdjancts(u);
                if (adjs == null) continue;
                foreach (var v in adjs)
                {
                    if (que.Contains(v.Key) && v.Value < keyPrims[v.Key])
                    {
                        keyPrims[v.Key] = v.Value;
                        parentPrims[v.Key] = u;
                    }
                }
            }
            int extractMinKey()
            {
                float min = float.PositiveInfinity;
                int minInd = -1;
                for (int i = 0; i < que.Count; i++)
                {
                    if (keyPrims[que[i]] < min)
                    {
                        min = keyPrims[que[i]];
                        minInd = que[i];
                    }
                }
                que.Remove(minInd);
                return minInd;
            }
        }
        public override void MSTPrims(T root) => MSTPrims(indexOf(root));

        public override Dictionary<T, (float start, float final)>? GetDFSTimes()
        {
            Dictionary<T, (float start, float final)> tm = [];
            for (int i = 0; i < Nodes.Count; i++)
            {
                if(dfsTime.Length > i)
                    tm.Add(valueAt(i), dfsTime[i]);
            }
            return tm;
        }

        public override string TostringMSTPrims()=> TostringMSTPrims(parentPrims, keyPrims);
        string TostringMSTPrims(int[] parent, float[] key)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Edge \tWeight" + Environment.NewLine);
            for (int i = 0; i < parent.Length; i++)
            {
                stringBuilder.Append($"{valueAt(parent[i])} - {valueAt(i)} \t{key[i]}" + Environment.NewLine);
            }
            stringBuilder.Append("Sum: " + key.Sum());
            return stringBuilder.ToString();
        }
        public override HashSet<((T?, T?), float)> MSTKruskals()
        {
            mSTKruskalsSet = new HashSet<((T?, T?), float)>();
            PriorityQueue<(int, int), float> edgeQue = new();
            Dictionary<int, HashSet<int>> sets = [];
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (!sets.ContainsKey(i))
                    sets.Add(i, new HashSet<int>() { i });
                (int, int) pair = (i, default);
                List<(int Key, float Value)>? adjs = getAdjancts(i);
                if (adjs != null)
                {
                    foreach (var adj in adjs)
                    {
                        pair.Item2 = adj.Key;
                        edgeQue.Enqueue(pair, adj.Value);
                    }
                }
            }
            while (edgeQue.Count > 0)
            {
                if (edgeQue.TryDequeue(out (int u, int v) edge, out float w))
                {
                    if (!sets[edge.u].Equals(sets[edge.v]))
                    {
                        union(edge.u, edge.v);
                        mSTKruskalsSet.Add(((valueAt(edge.u), valueAt(edge.v)), w));
                    }
                }
            }
            void union(int u, int v)
            {
                HashSet<int> set = sets[u];
                set.UnionWith(sets[v]);
                foreach (var elm in sets[u])
                {
                    sets[elm] = set;
                }
            }
            return mSTKruskalsSet;
        }

        public override string TostringMSTKruskals() => AdjacencyMatrix<T>.TostringMSTKruskals(mSTKruskalsSet);
        private static string TostringMSTKruskals(HashSet<((T? u, T? v) edge, float w)>? edgeSet)
        {
            if(edgeSet == null) return "";
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Edge \tWeight" + Environment.NewLine);
            foreach (var item in edgeSet)
            {
                stringBuilder.Append($"{item.edge.u} - {item.edge.v} \t{item.w}" + Environment.NewLine);
            }
            stringBuilder.Append("Sum: " + edgeSet.Sum(x => x.w));
            return stringBuilder.ToString();
        }


    }

}
