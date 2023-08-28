namespace _03.MaxHeap
{
    using System;
    using System.Collections.Generic;

    public class MaxHeap<T> : IAbstractHeap<T> where T : IComparable<T>
    {
        private List<T> elements;

        public MaxHeap()
        {
            this.elements = new List<T>();
        }
        public int Size => this.elements.Count;

        public void Add(T element)
        {
            this.elements.Add(element);
            var index = this.elements.Count - 1;
            var parentIndex = (index - 1) / 2;

            while(index > 0 && 
                this.elements[index].CompareTo(this.elements[parentIndex]) > 0)
            {
                this.Swap(index, parentIndex);
                index = parentIndex;
                parentIndex = (index - 1) / 2;
            }
        }

        private void Swap(int index, int parentIndex)
        {
            var temp = this.elements[index];
            this.elements[index] = this.elements[parentIndex];
            this.elements[parentIndex] = temp;
        }

        public T ExtractMax()
        {
            if (this.elements.Count == 0)
                throw new InvalidOperationException();

            var element = this.elements[0];
            this.Swap(0, this.elements.Count - 1);
            this.elements.RemoveAt(this.elements.Count - 1);
            var index = 0;
            
            var biggestChildIndex = this.GetBiggerChildIndex(index);
            while(biggestChildIndex >= 0 && biggestChildIndex < elements.Count
                && elements[biggestChildIndex].CompareTo(elements[index]) > 0)
            {
                this.Swap(biggestChildIndex, index);

                index = biggestChildIndex;
                biggestChildIndex = GetBiggerChildIndex(index);
            }

            return element;
        }

        private int GetBiggerChildIndex(int index)
        {
            int firstChildIndex = index * 2 + 1;
            int secondChildIndex = index * 2 + 2;
            
            if(firstChildIndex < elements.Count)
                return firstChildIndex;
            else if (secondChildIndex >= elements.Count)
                return -1;

            return Math.Max(int.Parse(elements[firstChildIndex].ToString()), 
                int.Parse(elements[secondChildIndex].ToString()));
        }

        public T Peek()
        {
            if(this.elements.Count == 0)
                throw new NullReferenceException();

            return this.elements[0];
        }
    }
}
