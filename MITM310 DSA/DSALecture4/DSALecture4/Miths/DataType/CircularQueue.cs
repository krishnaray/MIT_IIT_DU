using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miths.DataType
{
    public class CircularQueue<T>
    {
        private T?[] elements;
        public int Front { get; private set; } = 0;
        public int Rear { get; private set; } = 0;
        public int Max { get; private set; } = 0;
        public int Count { get; private set; } = 0;
        public T DefaultValue { get; private set; } = default;
        public CircularQueue(int size, T defaultValue)
        {
            elements = new T[size];
            Front = 0;
            Rear = -1;
            Max = size;
            Count = 0;
        }
        public void Insert(T? item)
        {
            if (Count < Max)
            {
                Rear = (Rear + 1) % Max;
                elements[Rear] = item;
                Count++;
            }
        }
        public void Delete()
        {
            if (Count > 0)
            {
                Front = (Front + 1) % Max;
                Count--;
            }
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = Front; i < Rear; i++)
                builder.Append((elements[i] != null ? elements[i]?.ToString() : "NULL") + ",");
            return builder.ToString();
        }
    }
}
