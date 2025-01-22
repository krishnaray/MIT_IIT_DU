using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miths.DataTypes.Tree
{
    public class BinaryTree<T> where T : IComparable<T>, IEquatable<T>
    {
        public int RootIndex { get; private set; }
        public int Count { get; private set; }
        public int MaxIndex => values.Count;
        List<T> values = [];
        public T NullValue { get; private set; }
        public BinaryTree(T root) {
            values = [];
            values[0] = root;
            RootIndex = 0;
        }
        public BinaryTree(T root, T[] arr, T nullVal)
        {
            if (arr == null || arr.Length < 1 || root == null) throw new ArgumentNullException(nameof(arr));
            int height = (int)MathF.Log2(arr.Length) + 1;
            int size = (int)MathF.Pow(2, height + 1) + 1;
            values = new List<T>();
            NullValue = nullVal;
            values.Insert(0, root);
            RootIndex = 0;
            foreach (var item in arr)
                Insert(item);
        }
        public int Insert(T? val) => insert(val, RootIndex);
        int insert(T? val, int nodeInd) {
            if(nodeInd < 0) 
                return -1;
            if (val == null) return -1;
            int ind = -1;
            if (nodeInd < values.Count)
            {
                if (values[nodeInd] == null || values[nodeInd].Equals(NullValue))
                    ind = nodeInd;
                else if (values[nodeInd].CompareTo(val) < 0)
                    ind = insertRight(val, nodeInd);
                else if (values[nodeInd].CompareTo(val) > 0)
                    ind = insertLeft(val, nodeInd);
                else
                    ind = nodeInd;
            }else
                ind = nodeInd;
            if (ind < 0) 
                return -1;
            if (values.Count > ind)
                values[ind] = val;
            else
            {
                while (ind > values.Count) {
                    values.Add(NullValue);
                }
                values.Insert(ind, val);
            }
            if(nodeInd == 0)
                Count++;
            return ind;
        }
        int insertLeft(T? val, int nodeInd) => insert(val, LeftIndex(nodeInd));
        int insertRight(T? val, int nodeInd) => insert(val, RightIndex(nodeInd));

        public int LeftIndex(int nodeInd) => (nodeInd * 2) + 1;
        public int RightIndex(int nodeInd) => (nodeInd * 2) + 2;

        public T? LeftValue(int nodeInd) => Value(LeftIndex(nodeInd));
        public T? RightValue(int nodeInd) => Value(RightIndex(nodeInd));
        public T? Value(int nodeInd) => InRange(nodeInd) ? values[nodeInd] : NullValue;

        public bool InRange(int nodeInd) => nodeInd >= 0 && nodeInd < values.Count;

        public bool IsValid(int nodeInd) => IsValidValue(Value(nodeInd));
        public bool IsValidValue(T? val) => val != null && !val.Equals(NullValue);

        public static List<T> PreorderTravers(BinaryTree<T> tree, int root, List<int> stack) {
            if(tree == null) 
                throw new ArgumentNullException(nameof(tree));
            List<T> output = new List<T>();
            stack.Clear();
            stack.Add(-1);

            int top = 0, ptr = root;
            while (tree.IsValid(ptr)) {
                output.Add(tree.Value(ptr));
                int rightInd = tree.RightIndex(ptr);
                int leftInd = tree.LeftIndex(ptr);
                if (tree.IsValid(rightInd))
                {
                    stack.Add(rightInd);
                    top++;
                }
                if (tree.IsValid(leftInd))
                {
                    ptr = leftInd;
                }
                else {
                    //if (top < stack.Count)
                    //{
                    ptr = stack[top];
                    stack.RemoveAt(top);
                    top--;
                    //}
                }
            }
            return output;
        }
        public static List<T> InorderTravers(BinaryTree<T> tree, int root, List<int> stack) {
            if (tree == null)
                throw new ArgumentNullException(nameof(tree));
            List<T> output = new List<T>();
            stack.Clear();
            stack.Add(-1);
            int top = 0, ptr = root;
            STEP2:
            while (tree.IsValid(ptr)) {
                stack.Add(ptr);
                top++;
                ptr = tree.LeftIndex(ptr);
            }
            ptr = stack[top];
            stack.RemoveAt(top);
            top--;
            while (tree.IsValid(ptr)) {
                output.Add(tree.Value(ptr));
                //If Ptr->Right ≠NULL then set Ptr=Ptr->Right and go to step 2.
                int rightInd = tree.RightIndex(ptr);
                if (tree.IsValid(rightInd)) {
                    ptr = rightInd;
                    goto STEP2;
                }
                //Set Ptr=Stack[Top] and Top=Top-1
                ptr = stack[top];
                stack.RemoveAt(top);
                top--;
            }
            return output;
        }
        
        public static Stack<T> PostorderTravers(BinaryTree<T> tree, int root)
        {
            if (tree == null)
                throw new ArgumentNullException(nameof(tree));
            //List<T> result = new List<T>();
            Stack<T> output = new Stack<T>();
            Stack<int> stack = new Stack<int>();

            stack.Push(root);
            int ptr = root, lastPtr = -1;
            while(stack.Count > 0) {
                ptr = stack.Pop();
                if (!tree.IsValid(ptr)) 
                    continue;
                output.Push(tree.Value(ptr));

                int left = tree.LeftIndex(ptr);
                int right = tree.RightIndex(ptr);
                if (tree.IsValid(left))
                    stack.Push(left);
                if (tree.IsValid(right))
                    stack.Push(right);

            }

            return output;
        }
        public string TostringInorder()
        {
            return ToString(InorderTravers(this, RootIndex, new List<int>()));
        }
        public string TostringPreorder()
        {
            return ToString(PreorderTravers(this, RootIndex, new List<int>()));
        }
        public string TostringPostorder() {
            return ToString(PostorderTravers(this, RootIndex).ToList());
        }
        public string ToString(List<T> values)
        {
            return string.Join(',', values.ToArray());
        }

    }
}
