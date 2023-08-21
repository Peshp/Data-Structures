namespace Problem01.CircularQueue
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class CircularQueue<T> : IAbstractQueue<T>
    {
        private const int DEFAULT_CAPACITY = 4;

        private T[] items;
        private int startIndex;
        private int endIndex;

        public CircularQueue(int capacity = DEFAULT_CAPACITY)
        {
            items = new T[capacity];
        }

        public int Count { get;private set; }

        public T Dequeue()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            var current = items[startIndex];
            startIndex = (startIndex + 1) % items.Length;

            Count--;
            return current;
        }

        public void Enqueue(T item)
        {
            if(items.Length <= Count)
            {
                Array.Resize(ref items, Count * DEFAULT_CAPACITY);
            }

            items[endIndex] = item;
            endIndex = (endIndex + 1) % items.Length;

            Count++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                var index = (startIndex + i) % items.Length;
                yield return items[index];
            }
        }

        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            return items[endIndex&items.Length];
        }

        public T[] ToArray()
        {
            if (Count == 0)
                return new T[0];
            var array = new T[Count];

            for (int i = 0; i < Count; i++)
            {
                array[i] = items[(startIndex + i) % items.Length];
            }

            return array;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }

}
