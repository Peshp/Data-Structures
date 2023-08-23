namespace Problem02.Stack
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Stack<T> : IAbstractStack<T>
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

        private Node top;

        public int Count { get; private set; }

        public void Push(T item)
        {
            top = new Node(item, top);
            Count++;
        }

        public T Pop()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            var current = top;
            top = current.Next;
            Count--;
            return current.Element;
        }

        public T Peek()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            return top.Element;
        }

        public bool Contains(T item)
        {
            var node = top;
            while (node != null)
            {
                if (node.Element.Equals(item))
                    return true;

                node = node.Next;
            }
            return false;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = top;
            while (node != null)
            {
                yield return top.Element;
                node = node.Next;                
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}