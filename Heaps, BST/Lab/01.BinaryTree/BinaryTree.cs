namespace _01.BinaryTree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class BinaryTree<T> : IAbstractBinaryTree<T>
    {
        public BinaryTree(T element, IAbstractBinaryTree<T> left, IAbstractBinaryTree<T> right)
        {
            Value = element;
            LeftChild = left;
            RightChild = right;
        }

        public T Value { get; private set; }

        public IAbstractBinaryTree<T> LeftChild { get; private set; }

        public IAbstractBinaryTree<T> RightChild { get; private set; }

        public string AsIndentedPreOrder(int indent)
        {
            var result = new StringBuilder();

            PreOrderAsString(result, this, indent);

            return result.ToString().Trim();
        }

        private void PreOrderAsString(StringBuilder result, BinaryTree<T> binaryTree, int indent)
        {
            result.Append(' ', indent)
                .AppendLine(binaryTree.Value.ToString());

            if (LeftChild != null)
                result.Append(' ', indent + 2)
                    .Append(AsIndentedPreOrder(indent));

            if (RightChild != null)
                result.Append(' ', indent + 2)
                    .Append(AsIndentedPreOrder(indent));
        }

        public void ForEachInOrder(Action<T> action)
        {
            if(LeftChild != null)
                LeftChild.ForEachInOrder(action);
            action.Invoke(this.Value);
            if(RightChild != null) 
                RightChild.ForEachInOrder(action);
        }

        public IEnumerable<IAbstractBinaryTree<T>> InOrder()
        {
            var result = new List<IAbstractBinaryTree<T>>();

            if (LeftChild != null)
                result.AddRange(LeftChild.InOrder());
            result.Add(this);
            if (RightChild != null)
                result.AddRange(RightChild.InOrder());
            
            return result;
        }

        public IEnumerable<IAbstractBinaryTree<T>> PostOrder()
        {
            var result = new List<IAbstractBinaryTree<T>>();
           
            if (LeftChild != null)
                result.AddRange(LeftChild.PostOrder());
            if (RightChild != null)
                result.AddRange(RightChild.PostOrder());
            result.Add(this);

            return result;
        }

        public IEnumerable<IAbstractBinaryTree<T>> PreOrder()
        {
            var result = new List<IAbstractBinaryTree<T>>();

            result.Add(this);
            if(LeftChild != null) 
                result.AddRange(LeftChild.PreOrder());
            if(RightChild != null)
                result.AddRange(RightChild.PreOrder());

            return result;
        }
    }
}
