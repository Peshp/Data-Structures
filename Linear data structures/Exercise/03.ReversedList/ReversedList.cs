namespace Problem03.ReversedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class ReversedList<T> : IAbstractList<T>
    {
        private const int DefaultCapacity = 4;

        private T[] items;

        public ReversedList()
            : this(DefaultCapacity) { }

        public ReversedList(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            this.items = new T[capacity];
        }

        public T this[int index]
        {
            get
            {
                if (IsValid(index))
                    throw new IndexOutOfRangeException();

                return items[Count - 1 - index];
            }
            set
            {
                if (IsValid(index))
                    throw new IndexOutOfRangeException();

                items[Count - 1 - index] = value;
            }
        }

        public int Count { get; private set; }

        public void Add(T item)
        {
            if(Count >= items.Length)
            {
                Array.Resize(ref items, Count * DefaultCapacity);
            }

            items[Count++] = item;
        }

        public bool Contains(T item)
        {
            if (Count == 0)
                throw new InvalidOperationException();

            for (int i = 0; i < Count; i++)
            {
                if (items[i].Equals(item))
                    return true;
            }
            return false;
        }

        public int IndexOf(T item)
        {
            if (Count == 0)
                throw new InvalidOperationException();

            for (int i = 0; i < Count; i++)
            {
                if (items[i].Equals(item))
                    return i;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            for (int i = Count; i < Count - index; i--)
            {
                items[i] = items[i - 1];
            }
            items[Count - index] = item;
        }

        public bool Remove(T item)
        {
            var toRemove = IndexOf(item);

            if (IsValid(toRemove))
            {
                RemoveAt(toRemove);
                Count--;
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (!IsValid(index)) 
                throw new IndexOutOfRangeException();

            index = this.Count - 1 - index;
            for (int i = index; i < this.Count; i++)
            {
                this.items[i] = this.items[i + 1];
            }

            this.items[this.Count - 1] = default;
            this.Count--;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        private bool IsValid(int index) 
            => index >= 0 && index < this.Count;
    }
}