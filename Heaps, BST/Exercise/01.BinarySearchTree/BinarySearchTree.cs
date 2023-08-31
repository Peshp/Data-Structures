namespace _02.BinarySearchTree
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
            node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);

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
            if (root == null)
                throw new InvalidOperationException();

            root = this.Delete(element, root);    
        }
        private Node Delete(T element, Node node)
        {
            if (node == null)
                return null;

            if(element.CompareTo(node.Value) < 0)
                node.Left = this.Delete(element, node.Left);
            if (element.CompareTo(node.Value) > 0)
                node.Right = this.Delete(element, node.Right);
            else
            {                
                if(node.Right == null)
                    return node.Left;
                if (node.Left == null)
                    return node.Right;

                var current = node;
                node = this.FindMin(current.Right);
                node.Right = this.DeleteMin(current.Right);
                node.Left = current.Left;
            }
            node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);

            return node;
        }
        private Node FindMin(Node node)
        {
            if (node == null)
                return node;

            return this.FindMin(node.Left);
        }

        public void DeleteMax()
        {
            if (root == null)
                throw new InvalidOperationException();

            root = this.DeleteMax(root);
        }
        private Node DeleteMax(Node node)
        {
            if (node.Right == null)
                return node.Left;

            node.Right = this.DeleteMax(node.Right);
            node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);

            return node;
        }

        public void DeleteMin()
        {
            if(root == null)
                throw new InvalidOperationException();

            root = this.DeleteMin(root);
        }
        private Node DeleteMin(Node node)
        {
            if (node.Left == null)
                return node.Right;

            node.Left = this.DeleteMin(node.Left);
            node.Count = 1 + this.Count(node.Left) + this.Count(node.Right);
            return node;
        }

        public int Count()
        {
            return this.Count(root);
        }
        private int Count(Node node)
        {
            if(node == null)
                return 0;

            return node.Count;
        }

        public int Rank(T element)
        {
            return this.Rank(element, root);
        }
        private int Rank(T element, Node node)
        {
            if(node == null)
                return 0;

            if(element.CompareTo(node.Value) < 0)
                return this.Rank(element, node.Left);
            if (element.CompareTo(node.Value) > 0)
                return 1 + this.Count(node.Left) + this.Rank(element, node.Right);

            return this.Count(node.Left);
        }

        public T Select(int rank)
        {
            Node node = this.Select(rank, root);

            if(node == null)
                throw new InvalidOperationException();

            return node.Value;
        }
        private Node Select(int element, Node node)
        {
            if(node == null)
                return null;

            int leftCount = this.Count(node.Left);
            if (leftCount == element)
                return node;
            if (leftCount > element)
                return this.Select(element, node.Left);

            return this.Select(element - (leftCount + 1), node.Right);
        }

        public T Ceiling(T element)
        {
            return this.Select(this.Rank(element) + 1);
        }

        public T Floor(T element)
        {
            return this.Select(this.Rank(element) - 1);
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
