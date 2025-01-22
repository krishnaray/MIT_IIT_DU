using Miths.DataTypes.Tree;

namespace BinaryTreeImp
{
    internal class Program
    {
        //static int[] treeData = new int[] {20, 19, 18, 15, 12, 11, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        static int[] treeData = new int[] { 5, 15, 2, 7, 11, 18 };//, 20, 21, 22, 25, 30, 40, 50};
        static void Main(string[] args)
        {
            BinaryTree<int> binaryTree = new BinaryTree<int>(10, treeData, int.MinValue);
            
            Console.WriteLine("10, " + string.Join(',', treeData));
            Console.WriteLine("Preorder: " + binaryTree.TostringPreorder());
            Console.WriteLine("Inorder: " + binaryTree.TostringInorder());
            Console.WriteLine("Postorder: " + binaryTree.TostringPostorder());
        }
    }
}
