namespace Problem03.Queue
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Queue<T> : IAbstractQueue<T>
    {
        private class Node
        {
            public Node(T element, Node next)
            {
                Element = element;
                Next = next;
            }

            public Node(T element)
            {
                Element = element;
            }

            public T Element { get; set; }
            public Node Next { get; set; }
        }

        private Node head;

        public int Count { get; private set; }

        public void Enqueue(T item)
        {
            if (Count == 0)
            {
                head = new Node(item);
                Count++;
                return;
            }                

            var current = head;
            while (current != null)
            {
                current = current.Next;
            }
            current = new Node(item);
            head = current;

            Count++;
        }

        public T Dequeue()
        {
            if(Count == 0)
                throw new InvalidOperationException();

            if(head.Next == null)
            {
                var x = head;
                head = x.Next;
                Count--;

                return x.Element;
            }

            var current = head;
            while (head.Next != null)
            {
                head = head.Next;
            }
            var nodeToRemove = current.Next;
            current.Next = null;
            Count--;

            return nodeToRemove.Element;
        }

        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            return head.Element;
        }

        public bool Contains(T item)
        {
            var current = head;
            while (current != null)
            {
                if(current.Element.Equals(item))
                    return true;

                current = current.Next;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            while (current != null)
            {
                yield return current.Element;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}