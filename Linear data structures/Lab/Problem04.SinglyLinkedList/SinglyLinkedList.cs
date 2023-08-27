namespace Problem04.SinglyLinkedList
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class SinglyLinkedList<T> : IAbstractLinkedList<T>
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

        public void AddFirst(T item)
        {
            head = new Node(item, head);
            Count++;
        }

        public void AddLast(T item)
        {
            if(Count == 0)
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

        public IEnumerator<T> GetEnumerator()
        {
            var current = head;
            while (current != null)
            {
                yield return current.Element;
                current = current.Next;
            }
        }

        public T GetFirst()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            return head.Element;
        } 

        public T GetLast()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            var current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            return current.Element;
        }

        public T RemoveFirst()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            var current = head;
            head = current.Next;
            Count--;

            return current.Element;
        }

        public T RemoveLast()
        {
            if (Count == 0)
                throw new InvalidOperationException();

            if (head.Next == null)
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
            current.Next = null;
            Count--;

            return current.Element;
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}