namespace Problem01.List
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class List<T> : IAbstractList<T>
    {
        private const int DEFAULT_CAPACITY = 4;
        private T[] items;

        public List()
            : this(DEFAULT_CAPACITY) {
        }

        public List(int capacity)
        {
            items = new T[capacity];
        }

        public T this[int index] 
        {
            get
            {
                IsValid(index);
                return items[index];
            }
            set
            {
                IsValid(index);
                items[index] = value;
            }
        }

        public int Count { get; private set; }

        public void Add(T item)
        {
            if(items.Length == Count)
            {
                Array.Resize(ref items, Count * 2);
            }

            items[Count++] = item;
        }

        public bool Contains(T item)
        {
            if (IndexOf(item) == -1)
                return false;

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
                yield return items[i];
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (items[i].Equals(item))
                    return i;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            IsValid(index);
            if (items.Length == Count)
            {
                Array.Resize(ref items, Count * 2);
            }

            for (int i = Count; i > index; i--)
                items[i] = items[i - 1];

            items[index] = item;
            Count++;
        }

        public bool Remove(T item)
        {
            if (!Contains(item))
                return false;

            var index = IndexOf(item);
            RemoveAt(index);
            return true;
        }

        public void RemoveAt(int index)
        {
            IsValid(index);
            for (int i = index; i < Count - 1; i++)
                items[i] = items[i + 1];

            items[Count - 1] = default;
            Count--;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private void IsValid(int index)
        {
            if(index < 0 || index >= Count) throw new IndexOutOfRangeException();
        }
    }
}