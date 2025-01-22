using Miths.DataStructure;
using System.Text;

namespace GraphDS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            List<string> nodeLst = new List<string>();
            string[] nodes = ["r", "s", "t", "u", "v", "w", "x", "y"];
            List<(string node, string adjc, float weight)> adjacencies =
            [
                ("s", "r", 1),
                ("s", "w", 1),
                ("w", "t", 1),
                ("t", "x", 1),
                ("x", "y", 1),
                ("u", "y", 1),
                ("t", "u", 1),
                ("w", "x", 1),
                ("r", "v", 1),
            ];

            Console.WriteLine("GraphNodes: " + ToString(nodes));
            Graph<string> graphMat = new Graph<string>(nodes, GraphType.MATRIX, false);
            graphMat.AddEdges(adjacencies);

            Graph<string> graphList = new Graph<string>(nodes, GraphType.LIST, false);
            graphList.AddEdges(adjacencies);

            //graphMat.MakeBipartite();
            //graphList.MakeBipartite();
            Console.WriteLine("IsBipartiteMat: " + graphMat.IsBipartite("s"));
            Console.WriteLine("IsBipartiteList: " + graphList.IsBipartite("s"));
            Console.WriteLine();
            nodeLst = graphMat.BFS("s");
            Console.WriteLine("BFSMat: " + ToString(nodeLst));
            Console.WriteLine("(graphMat)s->y: " + graphMat.PathToString("s", "y"));
            Console.WriteLine();
            nodeLst = graphList.BFS("s");
            Console.WriteLine("BFSList: " + ToString(nodeLst));
            Console.WriteLine("(graphList)s->y: " + graphList.PathToString("s", "y"));


            nodes = ["s", "a", "b", "c", "d", "e", "f", "g"];
            

            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("GraphNodes: " + ToString(nodes));
            adjacencies =
            [
                ("s", "a", 1),
                ("a", "b", 1),
                ("a", "c", 1),
                ("b", "s", 1),
                ("s", "c", 1),
                ("s", "d", 1),
                ("c", "b", 1),
                ("d", "c", 1),
                ("d", "e", 1),
                ("e", "c", 1),
                ("f", "d", 1),
                ("f", "e", 1),
                ("f", "g", 1),
                ("g", "c", 1),
            ];

            graphMat = new Graph<string>(nodes, GraphType.MATRIX);
            graphMat.AddEdges(adjacencies);
            graphList = new Graph<string>(nodes, GraphType.LIST);
            graphList.AddEdges(adjacencies);

            //graphMat.MakeBipartite();
            //graphList.MakeBipartite();
            Console.WriteLine("IsBipartiteMat: " + graphMat.IsBipartite("s"));
            Console.WriteLine("IsBipartiteList: " + graphList.IsBipartite("s"));
            Console.WriteLine();
            nodeLst = graphMat.DFS();
            Console.WriteLine("DFSMat: " + ToString(nodeLst));
            Console.WriteLine("DFSMat: " + ToString(graphMat.GetDFSTimes()));
            nodeLst = graphMat.TopologicalSort("s");
            Console.WriteLine("TopologicalSortMat(s): " + ToString(nodeLst));

            Console.WriteLine();
            nodeLst = graphList.DFS();
            Console.WriteLine("DFSList: " + ToString(nodeLst));
            Console.WriteLine("DFSList: " + ToString(graphList.GetDFSTimes()));
            nodeLst = graphList.TopologicalSort("s");
            Console.WriteLine("TopologicalSortList(s): " + ToString(nodeLst));


            nodes = ["a", "b", "c", "d", "e", "f", "g", "h", "i"];
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("GraphNodes: " + ToString(nodes));

            adjacencies =
            [
                ("a", "b", 4),
                ("a", "h", 8),
                ("b", "h", 11),
                ("b", "c", 8),
                ("c", "d", 7),
                ("c", "f", 4),
                ("c", "i", 2),
                ("d", "e", 9),
                ("d", "f", 14),
                ("e", "f", 10),
                ("f", "g", 2),
                ("g", "i", 6),
                ("g", "h", 1),
                ("i", "h", 7),
            ];

            graphMat = new Graph<string>(nodes, GraphType.MATRIX, false);
            graphMat.AddEdges(adjacencies);
            graphList = new Graph<string>(nodes, GraphType.LIST, false);
            graphList.AddEdges(adjacencies);


            //graphMat.MakeBipartite();
            //graphList.MakeBipartite();
            Console.WriteLine("IsBipartiteMat: " + graphMat.IsBipartite("a"));
            Console.WriteLine("IsBipartiteList: " + graphList.IsBipartite("a"));
            Console.WriteLine();
            graphMat.MSTPrims("a");
            graphList.MSTPrims("a");
            Console.WriteLine("MSTPrimsMat: " + Environment.NewLine + graphMat.TostringMSTPrims());
            Console.WriteLine("MSTPrimsList: " + Environment.NewLine + graphList.TostringMSTPrims());

            Console.WriteLine("----------------------------------------------------------");
            nodes = ["1", "2", "3", "4", "5", "6", "7", "8"];
            Console.WriteLine("GraphNodes: " + ToString(nodes));

            adjacencies =
            [
                ("1", "6", 54),
                ("1", "3", 47),
                ("1", "5", 80),
                ("6", "4", 74),
                ("6", "3", 75),
                ("3", "4", 88),
                ("3", "2", 55),
                ("3", "7", 66),
                ("3", "5", 23),
                ("5", "2", 32),
                ("5", "7", 93),
                ("4", "8", 29),
                ("4", "2", 31),
                ("2", "8", 78),
                ("2", "7", 74),
                ("7", "8", 68),
            ];

            graphMat = new Graph<string>(nodes, GraphType.MATRIX);
            graphMat.AddEdges(adjacencies);
            graphList = new Graph<string>(nodes, GraphType.LIST);
            graphList.AddEdges(adjacencies);


            //graphMat.MakeBipartite();
            //graphList.MakeBipartite();
            Console.WriteLine("IsBipartiteMat: " + graphMat.IsBipartite("1"));
            Console.WriteLine("IsBipartiteList: " + graphList.IsBipartite("1"));
            Console.WriteLine();
            graphMat.MSTKruskals();
            graphList.MSTKruskals();
            Console.WriteLine("MSTKruskalsMat: " + Environment.NewLine + graphMat.TostringMSTKruskals());
            Console.WriteLine("MSTKruskalsList: " + Environment.NewLine + graphList.TostringMSTKruskals());
        }

        public static string ToString(List<string> values) => ToString(values.ToArray());
        public static string ToString(string[] values) => string.Join(',', values);
        public static string ToString(int[] values) => string.Join(',', values);

        
        public static string ToString(Dictionary<string, (float start, float final)>? values)
        {
            if(values == null) return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in values)
            {
                stringBuilder.Append("(" + item.Value.start + "|" + item.Key + "|" + item.Value.final + "),");
            }
            return stringBuilder.ToString();
        }
    }
}
