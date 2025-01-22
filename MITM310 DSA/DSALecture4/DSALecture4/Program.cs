namespace DSALecture4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            Miths.DataType.LinkedList<int> list = new Miths.DataType.LinkedList<int>(arr);
            Console.WriteLine(list.ToString());
            
            float res = calculate("( ( ( ( 7 + 8 ) * ( 9 - 5 ) ) / 2 ) )");
            Console.WriteLine("Stack Res: " + res.ToString());

            Miths.DataType.CircularQueue<int> circularQueue = new Miths.DataType.CircularQueue<int>(arr.Length, int.MinValue);
            foreach (int val in arr) {
                circularQueue.Insert(val);
            }
            Console.WriteLine("circularQueue: " + circularQueue.ToString());
            foreach (int val in arr) {
                Console.WriteLine("index of " + val.ToString() + ": " + binarySearch<int>(val, arr, 0, arr.Length  -1));
            }
        }
        static int binarySearch<T>(T item, T[] arr, int first, int last) where T : IComparable<T>, IEquatable<T>
        { 
            int mid = (last + first) / 2;
            if (mid < 0 || mid > arr.Length) return -1;
            if (arr[mid].Equals(item)) return mid;
            if(arr[mid].CompareTo(item) < 0)
                return binarySearch(item, arr, mid + 1, last);
            else
                return binarySearch(item, arr, first, mid - 1);
        }
        static float calculate(string eqn) {
            if(string.IsNullOrEmpty(eqn)) throw new ArgumentNullException("IsNullOrEmpty: " + nameof(eqn));
            Miths.DataType.Stack<string> operators = new Miths.DataType.Stack<string>(eqn.Length, "");
            Miths.DataType.Stack<float> operand = new Miths.DataType.Stack<float>(eqn.Length, float.MinValue);
            foreach (char c in eqn) {
                if (c == ' ') continue;
                if (c == ')') {
                    if (operand.TopIndex >= 1)
                    {
                        float oprY = operand.Pop();
                        float oprX = operand.Pop();
                        operand.Push(calculate(oprX, oprY, operators.Pop()?[0]));
                    }
                    operators.Pop();//first bracket
                    continue;
                }
                string s = c.ToString();
                if (float.TryParse(s, out float oprnd))
                    operand.Push(oprnd);
                else operators.Push(s);
            }
            return operand.Pop();
        }
        static float calculate(float oprX, float oprY, char? operatorXY) {
            switch (operatorXY) {
                case '+':
                    return oprX + oprY;
                case '-':
                    return oprX - oprY; 
                case '*':
                    return oprX * oprY;
                case '/':
                    if (oprY == 0) throw new DivideByZeroException(oprX.ToString() + "/" + oprY.ToString());
                    return oprX / oprY;
                default:
                    return 0;
            }
        }
    }
}
