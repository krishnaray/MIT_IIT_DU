using System.Text;

namespace Miths.DataType
{
    public class LinkedList<T> where T : IComparable<T>, IEquatable<T>
    {
        public T? First => _first == null ? default : _first.Data;
        public T? Last => _last == null ? default : _last.Data;
        Node<T>? _first { get; set; } = null;
        Node<T>? _last { get; set; } = null;
        public int Count { get; private set; } = 0;
        public LinkedList() { }
        public LinkedList(T element) {
            Add(element);
        }
        public LinkedList(T[] arr) {
            AddRange(arr, 0, arr.Length);
        }

        (int idx, Node<T>? node) this[T? element] {
            get {
                if (_first == null) return (-1, null);
                Node<T>? current = _first;
                int i = 0;
                while (current != null && !current.Equals(element)) {
                    current = current.Next;
                    ++i;
                }
                return (current != null ? i : -1, current);
            }
        }
        public T? this[int ind] {
            get {
                Node<T>? node = getNodeAt(ind);
                return node != null? node.Data : default;
            }
            set {
                Node<T>? node = getNodeAt(ind);
                if(node != null)
                    node.Data = value;
            }
        }
        Node<T>? getNodeAt(int ind)
        {
            if (ind < 0 || ind >= Count) throw new ArgumentOutOfRangeException(nameof(ind));
            if (ind == 0) return _first;
            if (_first == null)
                return default;
            if (ind == (Count - 1)) return _last;
            Node<T>? node = _first;
            for (int i = 0; i < ind; i++)
            {
                if (node == null) break;
                node = node.Next;
            }
            return node;
        }
        public void Insert(int ind, T? element) => insert(ind, new Node<T>(element));
        void insert(int ind, Node<T>? value) {

            if (value == null) throw new ArgumentNullException(nameof(value));
            if (ind < 0 || ind >= Count) throw new ArgumentOutOfRangeException(nameof(ind));
            Node<T>? valueNode = value;
            if (ind == 0)
            {
                if (_first == null)
                    _first = valueNode;
                else
                {
                    valueNode.Next = _first.Next;
                    _first = valueNode;
                }
            }
            Node<T>? nodePrev = getNodeAt(ind - 1);
            if (nodePrev == null) return;
            nodePrev.Next = valueNode;
            Node<T>? node = getNodeAt(ind);
            if (node == null) return;
            valueNode.Next = node.Next;
        }
        int Add(Node<T> element) {
            if (element == null) throw new ArgumentNullException(null, nameof(element));
            if (_first == null)
                _first = element;
            else
            {
                if (_last == null)
                {
                    _last = element;
                    _first.Next = _last;
                }
                else
                {
                    _last.Next = element;
                    _last = element;
                }
            }
            return ++Count;
        }
        public int Add(T element) => Add(new Node<T>(element));
        public void AddRange(T[] arr, int start, int cnt) {
            if (arr == null) throw new ArgumentNullException(nameof(arr));
            for (int i = start; i < (start + cnt) && i < arr.Length; i++) { 
                Add(arr[i]);
            }
        }
        bool Remove(T? node) => Remove(this[node].node);
        bool Remove(Node<T>? node) {
            if (_first == null) 
                return false; 
            if (_first == node) 
            {
                _first = _first.Next;
                Count--;
                return true; 
            }
            Node<T> current = _first; 
            while (current.Next != null && current.Next != node) 
            { 
                current = current.Next; 
            }
            if (current.Next == null) 
                return false; 
            current.Next = current.Next.Next;
            Count--;
            return true;
        }
        bool Remove(int ind) {
            Node<T>? nodePrev = getNodeAt(ind - 1);
            if (nodePrev == null) return false;
            Node<T>? node = getNodeAt(ind);
            if (node == null) return false;
            nodePrev.Next = node.Next;
            Count--;
            return true;
        }
        public override string ToString()
        {
            if (_first == null) return string.Empty;
            StringBuilder builder = new StringBuilder();
            Node<T>? current = _first;
            while (current != null)
            {
                builder.Append((current.Data != null ? current.Data?.ToString() : "NULL") + ",");
                current = current.Next;
            }
            return builder.ToString();
        }
        internal class Node<R>(R? data) : IComparable<R>, IEquatable<R>, IEquatable<Node<R>>
            where R : IComparable<R>, IEquatable<R>
        {
            internal R? Data { get; set; } = data;
            internal Node<R>? Next { get; set; } = null;

            public bool Equals(Node<R>? other) {
                if (other == null) return false;
                if (this == other) return true;
                bool nexEql = Next == other.Next;
                if (!nexEql) return false;
                if(Data == null && other.Data == null) return true;
                if (Data != null)
                {
                    return Data.Equals(other.Data);
                }
                return false;
            }
            public int CompareTo(R? other) => Data == null ? 1 : Data.CompareTo(other);
            public bool Equals(R? other) => CompareTo(other) == 0;
        }
    }
}
