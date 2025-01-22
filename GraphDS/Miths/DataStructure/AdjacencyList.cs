
using System.Text;

namespace Miths.DataStructure
{
    public class AdjacencyList<T> : Adjacency<T> where T : IComparable<T>, IEquatable<T>
    {
        Dictionary<T, ColorType> colors = [];
        Dictionary<T, T> previous = [];
        Dictionary<T, float> distance = [];
        Dictionary<T, (float start, float final)> dfsTime = [];

        Dictionary<T, float> keyPrims = [];
        Dictionary<T, T> parentPrims = [];

        HashSet<((T?, T?), float)> mSTPrimsSet = new();
        HashSet<((T?, T?), float)> mSTKruskalsSet = new();

        public Dictionary<T, Dictionary<T, float>> Edges { get; private set; }
        public override T? FirstNode() => Edges.Count > 0 ? Edges.First().Key : default;
        public AdjacencyList(T[] nodes, bool directed) : base(nodes, directed)
        {
            Edges = new Dictionary<T, Dictionary<T, float>>();
            AddNodes(nodes);
        }

        public override void AddNode(T node)
        {
            if (!Edges.ContainsKey(node))
                Edges.Add(node, new Dictionary<T, float>());
        }

        public override void AddNodes(T[] nodes)
        {
            foreach (T node in nodes)
                AddNode(node);
        }

        public override void RemoveNode(T node)
        {
            Edges.Remove(node);
            foreach (var elem in Edges.Values)
            {
                while (elem.ContainsKey(node))
                    elem.Remove(node);
            }
        }
        public override void AddAdjacencies(List<(T node, T adjc, float weight)> adjacencies) {
            foreach (var adj in adjacencies)
                AddAdjacency(adj.node, adj.adjc, adj.weight);
        } 
        public override void AddAdjacency(T node, T adjc, float weight)
        {
            AddNode(node);
            AddNode(adjc);
            
            if (!Edges[node].ContainsKey(adjc))
                Edges[node].Add(adjc, weight);
            else
                Edges[node][adjc] = weight;
            if (!Directed)
            {
                if (!Edges[adjc].ContainsKey(node))
                    Edges[adjc].Add(node, weight);
                else
                    Edges[adjc][node] = weight;
            }
        }

        public override void RemoveAdjacency(T node, T adjc)
        {
            if (Edges.TryGetValue(node, out Dictionary<T, float>? value)) {
                value.Remove(adjc);
            }
            if (!Directed) {
                if (Edges.TryGetValue(adjc, out value))
                    value.Remove(node);
            }
        }
        public override List<T>? BFS(T root)
        {
            if (root == null || !Edges.ContainsKey(root)) return null;
            colors = [];
            previous = [];
            distance = [];
            bfsLst = [];
            foreach (var edge in Edges)
            {
                colors.Add(edge.Key, ColorType.WHITE);
                previous.Add(edge.Key, default);
                distance.Add(edge.Key, float.PositiveInfinity);
            }
            colors[root] = ColorType.GRAY;
            distance[root] = 0;
            Queue<T> que = new Queue<T>();
            que.Enqueue(root);
            while (que.Count > 0)
            {
                T u = que.Dequeue();
                bfsLst.Add(u);
                Dictionary<T, float>? adjs = Edges[u];

                if (adjs.Count > 0)
                {
                    foreach (var v in adjs)
                    {
                        if (colors[v.Key] == ColorType.WHITE)
                        {
                            colors[v.Key] = ColorType.GRAY;
                            distance[v.Key] = distance[u] + v.Value;
                            previous[v.Key] = u;
                            que.Enqueue(v.Key);
                        }
                    }
                }
                colors[u] = ColorType.BLACK;
            }
            return bfsLst;
        }

        public override List<T>? GetPath(T from, T to)
        {
            if (from == null) return null;
            if (to == null) return new List<T>();
            List<T> path = [];
            if (to.Equals(from)) path.Add(from);
            else
            {
                List<T>? subPath = GetPath(from, previous[to]);
                if (subPath != null)
                {
                    foreach (var v in subPath)
                        path.Add(v);
                }
                path.Add(to);
            }
            return path;
        }

        public override bool IsBipartite(bool forceMake) => IsBipartite(FirstNode(), forceMake);

        public override bool IsBipartite(T root, bool forceMake = false)
        {
            if (root == null || !Edges.ContainsKey(root)) return false;
            Dictionary<T, ColorType> colors = [];
            foreach (var edge in Edges)
            {
                colors.Add(edge.Key, ColorType.WHITE);
            }
            colors[root] = ColorType.GRAY;
            Queue<T> que = new Queue<T>();
            que.Enqueue(root);
            while (que.Count > 0) {
                T u = que.Dequeue();
                Dictionary<T, float>? adjs = Edges[u];

                if (adjs.Count > 0) {
                    foreach (var v in adjs) {
                        if (colors[v.Key] == colors[u])
                        {
                            if (forceMake)
                                RemoveAdjacency(u, v.Key);
                            else
                                return false; 
                        }
                        if (colors[v.Key] == ColorType.WHITE)
                        {
                            colors[v.Key] = colors[u] != ColorType.GRAY ? ColorType.GRAY : ColorType.BLACK;
                            que.Enqueue(v.Key);
                        }
                    }
                }
            }
            return true;
        }
        void dfsVisit(T node, ref int time) {
            colors[node] = ColorType.GRAY;
            time++;
            dfsTime[node] = (time, float.NegativeInfinity);
            dfsLst.Add(node);
            Dictionary<T, float>? adjs = Edges[node];
            foreach (var v in adjs) {
                if (colors[v.Key] == ColorType.WHITE)
                {
                    previous[v.Key] = node;
                    dfsVisit(v.Key, ref time);
                }
            }
            colors[node] = ColorType.BLACK;
            time++;
            dfsTime[node] = (dfsTime[node].start, time);
        }
        public override List<T>? DFS()
        {
            InitDFS();
            int time = 0;
            foreach (var edge in Edges)
            {
                if (colors[edge.Key] == ColorType.WHITE)
                    dfsVisit(edge.Key, ref time);
            }
            return dfsLst;
        }
        protected override void InitDFS() {
            colors = [];
            previous = [];
            dfsTime = [];
            dfsLst = [];
            foreach (var edge in Edges)
            {
                colors.Add(edge.Key, ColorType.WHITE);
                previous.Add(edge.Key, default);
                dfsTime.Add(edge.Key, (float.PositiveInfinity, float.NegativeInfinity));
            }
        }
        public override List<T> TopologicalSort(T node)
        {
            if(!Edges.ContainsKey(node)) return new List<T>();
            InitDFS();
            int time = 0; 
            if (colors[node] == ColorType.WHITE)
                dfsVisit(node, ref time);
            return  dfsLst;
        }

        public override void MSTPrims(T root)
        {
            List<T> que = new List<T>();
            keyPrims = [];
            parentPrims = [];
            foreach (var edge in Edges) {
                que.Add(edge.Key);
                keyPrims.Add(edge.Key, float.PositiveInfinity);
                parentPrims.Add(edge.Key, default);
            }
            keyPrims[root] = 0;
            while (que.Count > 0)
            {
                T u = extractMinKey();
                Dictionary<T, float>? adjs = Edges[u];
                foreach (var v in adjs) {
                    if (que.Contains(v.Key) && v.Value < keyPrims[v.Key]) {
                        keyPrims[v.Key] = v.Value;
                        parentPrims[v.Key] = u;                        
                    }
                }
            }

            T extractMinKey()
            {
                float min = float.PositiveInfinity;
                T minT = default(T);
                foreach (var node in que)
                {
                    if (keyPrims.ContainsKey(node) && keyPrims[node] < min)
                    {
                        min = keyPrims[node];
                        minT = node;
                    }
                }
                que.Remove(minT);
                return minT;
            }
        }


        public override Dictionary<T, (float start, float final)>? GetDFSTimes() => dfsTime;


        public override HashSet<((T?, T?), float)> MSTKruskals()
        {
            mSTKruskalsSet = [];
            PriorityQueue <(T, T), float> edgeQue = new();
            Dictionary<T, HashSet<T>> sets = [];
            foreach (var edge in Edges) {
                if(!sets.ContainsKey(edge.Key))
                    sets.Add(edge.Key, new HashSet<T>() { edge.Key});
                (T, T) pair = (edge.Key, default);
                foreach (var adj in edge.Value)
                {
                    pair.Item2 = adj.Key;
                    edgeQue.Enqueue(pair, adj.Value);
                }
            }
            //sets.OrderBy(x => x.Key);
            while (edgeQue.Count > 0) {
                if (edgeQue.TryDequeue(out (T u, T v) edge, out float w))
                {
                    if (!sets[edge.u].Equals(sets[edge.v]))
                    {
                        union(edge.u, edge.v);
                        mSTKruskalsSet.Add((edge, w));
                    }
                }
            }
            void union(T u, T v) {
                HashSet<T> set = sets[u];
                set.UnionWith(sets[v]);
                foreach (var elm in sets[u]) {
                    sets[elm] = set;
                }
            }
            return mSTKruskalsSet;
        }
        public override string TostringMSTPrims() => AdjacencyList<T>.TostringMSTPrims(parentPrims, keyPrims);

        static string TostringMSTPrims(Dictionary<T, T> parent, Dictionary<T, float> key)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Edge \tWeight" + Environment.NewLine);
            foreach (var edge in parent)
            {
                stringBuilder.Append($"{edge.Value} - {edge.Key} \t{key[edge.Key]}" + Environment.NewLine);
            }
            stringBuilder.Append("Sum: " + key.Sum(x => x.Value));
            return stringBuilder.ToString();
        }
        public override string TostringMSTKruskals() => AdjacencyList<T>.TostringMSTKruskals(mSTKruskalsSet);

        private static string TostringMSTKruskals(HashSet<((T? u, T? v) edge, float w)> edgeSet)
        {
            if (edgeSet == null) return "";
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
