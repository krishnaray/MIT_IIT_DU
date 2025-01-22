
namespace Miths.DataType
{
    public class Stack<T>
    {
        public int Size => values != null ? values.Length : 0;
        public T DefaultValue { get; private set; } = default;
        public int TopIndex { get; private set; } = -1;
        T?[] values;
        
        public Stack(int size, T defaultValue) {
            DefaultValue = defaultValue;
            values = new T[size];
            for (int i = 0; i < size; i++) {
                values[i] = DefaultValue;
            }
        }
        public void Push(T? item)
        {
            if(TopIndex >= -1 && TopIndex < values.Length -1)
                values[++TopIndex] = item;
            else
                throw new OverflowException(nameof(values));

        }
        public T? Pop() {
            if (TopIndex >= 0 && TopIndex < values.Length)
            {
                T? itm = values[TopIndex];
                values[TopIndex] = DefaultValue;
                TopIndex--;
                return itm;
            }
            throw new IndexOutOfRangeException();
        }
    }
}
