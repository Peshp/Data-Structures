﻿namespace _02.BinarySearchTree
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class BinarySearchTree<T> : IBinarySearchTree<T> where T : IComparable
    {
        private class Node
        {
            public Node(T value)
            {
                this.Value = value;
            }

            public T Value { get; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            public int Count { get; set; }
        }

        private Node root;

        private BinarySearchTree(Node node)
        {
            this.PreOrderCopy(node);
        }

        public BinarySearchTree()
        {
        }

        public void Insert(T element)
        {
            this.root = this.Insert(element, this.root);
        }
        private Node Insert(T element, Node node)
        {
            if (node == null)
            {
                node = new Node(element);
            }
            else if (element.CompareTo(node.Value) < 0)
            {
                node.Left = this.Insert(element, node.Left);
            }
            else if (element.CompareTo(node.Value) > 0)
            {
                node.Right = this.Insert(element, node.Right);
            }

            return node;
        }

        public bool Contains(T element)
        {
            Node current = this.FindElement(element);

            return current != null;
        }

        public void EachInOrder(Action<T> action)
        {
            this.EachInOrder(this.root, action);
        }
        private void EachInOrder(Node node, Action<T> action)
        {
            if (node == null)
            {
                return;
            }

            this.EachInOrder(node.Left, action);
            action(node.Value);
            this.EachInOrder(node.Right, action);
        }

        public IBinarySearchTree<T> Search(T element)
        {
            Node current = this.FindElement(element);

            return new BinarySearchTree<T>(current);
        }
        private Node FindElement(T element)
        {
            Node current = this.root;

            while (current != null)
            {
                if (current.Value.CompareTo(element) > 0)
                {
                    current = current.Left;
                }
                else if (current.Value.CompareTo(element) < 0)
                {
                    current = current.Right;
                }
                else
                {
                    break;
                }
            }

            return current;
        }

        private void PreOrderCopy(Node node)
        {
            if (node == null)
            {
                return;
            }

            this.Insert(node.Value);
            this.PreOrderCopy(node.Left);
            this.PreOrderCopy(node.Right);
        }

        public void Delete(T element)
        {
            throw new NotImplementedException();
        }

        public void DeleteMax()
        {
            throw new NotImplementedException();
        }

        public void DeleteMin()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public int Rank(T element)
        {
            throw new NotImplementedException();
        }

        public T Select(int rank)
        {
            throw new NotImplementedException();
        }

        public T Ceiling(T element)
        {
            throw new NotImplementedException();
        }

        public T Floor(T element)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> Range(T startRange, T endRange)
        {
            var result = new Queue<T>();

            this.Range(root, result, startRange, endRange);

            return result;
        }

        private void Range(Node node, Queue<T> queue, T startRange, T endRange)
        {
            if (node == null)
                return;

            if (startRange.CompareTo(node.Value) < 0)
                this.Range(node.Left, queue, startRange, endRange);
            if (startRange.CompareTo(node.Value) <= 0 &&
                endRange.CompareTo(node.Value) >= 0)
                queue.Enqueue(node.Value);
            if (endRange.CompareTo(node.Value) > 0)
                this.Range(node.Right, queue, startRange, endRange);           
        }
    }
}
