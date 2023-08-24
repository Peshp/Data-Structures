namespace _02.BinarySearchTree
{
    using System;

    public class BinarySearchTree<T> : IBinarySearchTree<T>
        where T : IComparable<T>
    {
        private class Node
        {
            public Node(T element)
            {
                Element = element;
            }

            public T Element { get; set; }

            public Node Left { get; set; }

            public Node Right { get; set; }
        }

        private Node root;

        public BinarySearchTree()
        {           
        }

        private BinarySearchTree(Node node)
        {
            this.PreOrderCopy(node);
        }

        private void PreOrderCopy(Node node)
        {
            if (node == null)
            {
                return;
            }

            this.Insert(node.Element);
            this.PreOrderCopy(node.Left);
            this.PreOrderCopy(node.Right);
        }

        public bool Contains(T element)
            => this.FindNode(element) != null;

        private Node FindNode(T element)
        {
            var node = this.root;
            while (node != null)
            {
                if (element.CompareTo(node.Element) < 0)
                    node = node.Left;
                else if (element.CompareTo(node.Element) > 0)
                    node = node.Right;
                else
                    break;
            }

            return node;
        }

        public void EachInOrder(Action<T> action)
            => this.EachInOrder(action, root);

        private void EachInOrder(Action<T> action, Node root)
        {
            if (root == null)
                return;

            this.EachInOrder(action, root.Left);
            action.Invoke(root.Element);
            this.EachInOrder(action, root.Right);
        }

        public void Insert(T element)
            => root = this.Insert(element, root);

        private Node Insert(T element, Node node)
        {
            if (node == null)
                node = new Node(element);
            else if (element.CompareTo(node.Element) < 0)
                node.Left = this.Insert(element, node.Left);
            else if (element.CompareTo(node.Element) > 0)
                node.Right = this.Insert(element, node.Right);

            return node;
        }

        public IBinarySearchTree<T> Search(T element)
        {
            var node = this.FindNode(element);

            if (node == null)
                return null;
            
            return new BinarySearchTree<T>(node);
        }
    }
}
