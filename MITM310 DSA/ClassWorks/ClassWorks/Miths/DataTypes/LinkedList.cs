using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassWorks.Miths.DataTypes
{
    public class ListItem<T> : IComparable<T> where T : IComparable<T>, INullable
    { 
        public T? Item { get; internal set; }
        public ListItem<T>? Next { get; internal set; }
        public ListItem(T? item) { Item = item; }

        public int CompareTo(T? other) => other != null ? other.CompareTo(Item) : -1;
        public bool Equals(T? other) => other != null ? other.CompareTo(Item) == 0 : false;
    }
    public class LinkedList<T> where T : IComparable<T>, INullable
    {
        public ListItem<T>? First { get; private set; }
        public ListItem<T>? Last { get; private set; }
        //ListItem<T>[] listItems;
        public int Count { get; private set; }
        public LinkedList() {
            First = null;
            Count = 0;
            //listItems = new ListItem<T>[0];
        }

        public int Add(T element) {
            ListItem<T> newNode = new ListItem<T>(element);
            int i = -1;
            if (First == null)
            {
                First = newNode;
                Last = newNode;
                i = 0;
            }
            else
            {
                Last.Next = newNode;
                Last = newNode;
                i = Count;
            }
            Count = i + 1;
            return i;
        }
        public int IndexOf(T element) {
            if(First == null) return -1;
            ListItem<T> current = First;
            if (current.Equals(element))
                return 0;
            int i = 0;
            while (current.Next != null) {
                i++;
                if (current.Equals(element))
                    return i;
                current = current.Next;
            }
            return -1;
        }
        public void Delete(ListItem<T>? item)
        {
            if (item != null)
                Delete(item.Item);
            else Delete(null);
        }
        public int Delete(T? element) {
            if (First == null) return -1;
            if (First.Equals(element)) {
                First = First.Next;
                Count--;
                return 0;
            }
            ListItem<T> current = First;
            int i = 0;
            while (current.Next != null)
            {
                i++;
                if (current.Next != null && current.Next.Equals(element))
                {
                    current.Next = current.Next.Next;
                    Count--;
                    return i;
                }
                current = current.Next;
            }
            return -1;
        }

        public void Insert(int index, T? element) {
            for (int i = 0; i < Count; i++) {
                
            }
        }
    }
}
