namespace Problem02.DoublyLinkedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class DoublyLinkedList<T> : IAbstractLinkedList<T>
    {
        private class Node
        {
            public Node(T element)
            {
                Element = element;
            }

            public T Element { get; set; }

            public Node Next { get; set; }

            public Node Previous { get; set; }
        }

        private Node head; // First
        private Node tail; // Last

        public int Count { get; private set; }

        public void AddFirst(T item)
        {
            var newElement = new Node(item);
            if(head == null)
                head = tail = newElement;
            else
            {
                head.Previous = newElement;
                newElement.Next = head;
                head = newElement;
            }

            Count++;
        }

        public void AddLast(T item)
        {
            var newElement = new Node(item);
            if (head == null)
                head = tail = newElement;
            else
            {
                tail.Next = newElement;
                newElement.Previous = tail;
                head = newElement;
            }

            Count++;
        }

        public T GetFirst()
        {
            if(head == null)
                throw new InvalidOperationException();

            return head.Element;
        }

        public T GetLast()
        {
            if (head == null)
                throw new InvalidOperationException();

            return tail.Element;
        }

        public T RemoveFirst()
        {           
            if (head == null)
                throw new InvalidOperationException();

            var current = head;
            if (head.Next == null)
                head = tail = null;
            else
            {
                head = head.Next;
                head.Previous = null;
            }

            Count--;
            return current.Element;
        }

        public T RemoveLast()
        {
            if (tail.Previous == null)
                throw new InvalidOperationException();

            var current = head;
            if (head.Next == null)
                head = tail = null;
            else
            {
                tail = tail.Previous;
                tail.Next = null;
            }

            Count--;
            return current.Element;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            while (current != null)
            {
                yield return head.Element;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}