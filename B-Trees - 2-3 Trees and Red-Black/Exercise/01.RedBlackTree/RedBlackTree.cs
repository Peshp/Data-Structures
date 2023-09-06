namespace _01.RedBlackTree
{
    using System;

    public class RedBlackTree<T> where T : IComparable
    {
        public class Node
        {
            public Node(T value)
            {
                this.Value = value;
            }

            public T Value { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }

            public bool Color { get; set; }
        }

        public Node root;


        private RedBlackTree(Node node)
        {
            this.PreInOrderCopy(node);
        }

        private void PreInOrderCopy(Node node)
        {
            if (node == null)
                return;

            this.Insert(node.Value);
            this.PreInOrderCopy(node.Left);
            this.PreInOrderCopy(node.Right);
        }

        public RedBlackTree()
        {

        }

        public void EachInOrder(Action<T> action)
        {
            this.EachInOrder(root, action);
        }

        private void EachInOrder(Node node, Action<T> action)
        {
            if (node == null)
                return;

            this.EachInOrder(node.Left, action);
            action(node.Value);
            this.EachInOrder(node.Right, action);
        }

        public RedBlackTree<T> Search(T element)
        {
            Node current = this.FindNode(element);

            return new RedBlackTree<T>(current);
        }

        private Node FindNode(T element)
        {
            var current = root;
            while(current != null)
            {
                if (element.CompareTo(current.Value) < 0)
                    current = current.Left;
                else if (element.CompareTo(current.Value) > 0)
                    current = current.Right;
                else
                    break;
            }

            return current;
        }

        public void Insert(T element)
        {
            root = this.Insert(root, element);
        }

        private Node Insert(Node node, T element)
        {
            if(node == null)
            {
                node = new Node(element);
            }
            if(element.CompareTo(node.Left) < 0)
            {
                node.Left = this.Insert(node.Left, element);
            }
            else
            {
                node.Right = this.Insert(node.Right, element);
            }

            if (IsRed(node.Right))
            {
                node = RotateLeft(node);
            }
            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {
                node = RotateRight(node);
            }
            if(IsRed(node.Left) && IsRed(node.Right))
            {
                ColorFlip(node);
            }

            return node;
        }

        public void Delete(T key)
        {
            throw new NotImplementedException();
        }

        private Node MoveRedRight(Node node)
        {
            ColorFlip(node);
            if (IsRed(node.Left.Left))
            {
                node.Left = RotateRight(node.Left);
                node = RotateLeft(node);
                ColorFlip(node);
            }

            return FixUp(node);
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
                return null;

            if(IsRed(node.Left) && IsRed(node.Left.Left))
                node = MoveRedLeft(node.Left);

            node.Left = this.DeleteMin(node.Left);
            return FixUp(node);
        }

        private Node MoveRedLeft(Node node)
        {
            ColorFlip(node);
            if (IsRed(node.Right.Left))
            {
                node.Right = RotateRight(node.Right);
                node = RotateLeft(node);
                ColorFlip(node);
            }

            return FixUp(node);
        }

        private Node FixUp(Node node)
        {
            if (IsRed(node.Right))
                node = RotateLeft(node.Left);
            if(IsRed(node.Left) && IsRed(node.Left.Left))
                node = RotateRight(node);
            if(IsRed(node.Left) && IsRed(node.Right))
                ColorFlip(node);

            return node;
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
                return null;

            if (IsRed(node.Right) && IsRed(node.Right.Right))
                node = MoveRedLeft(node.Right);

            node.Right = this.DeleteMin(node.Right);
            return FixUp(node);
        }

        public int Count()
        {
            return this.Count(root);
        }

        private int Count(Node root)
        {
            if (root == null)
                return 0;

            return 1 + this.Count(root.Left) + this.Count(root.Right);
        }

        private Node RotateLeft(Node node)
        {
            var temp = node.Right;
            node.Right = temp.Left;
            node.Left = node;
            temp.Color = temp.Left.Color;
            temp.Left.Color = true;

            return temp;
        }

        private Node RotateRight(Node node)
        {
            var temp = node.Left;
            node.Left = temp.Right;
            temp.Right = node;
            temp.Color = temp.Right.Color;
            temp.Right.Color = true;

            return temp;
        }

        private bool IsRed(Node node)
        {
            if(node == null)
                return false;

            return node.Color == true;
        }

        private void ColorFlip(Node node)
        {
            node.Color = !node.Color;
            node.Left.Color = !node.Left.Color;
            node.Right.Color = !node.Right.Color;
        }
    }
}