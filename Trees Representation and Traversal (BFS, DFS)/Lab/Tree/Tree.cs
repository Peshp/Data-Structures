namespace Tree
{
    using System;
    using System.Collections.Generic;

    public class Tree<T> : IAbstractTree<T>
    {
        private T value;
        private Tree<T> parent;
        private List<Tree<T>> children;

        public Tree(T value)
        {
            this.value = value;
            children = new List<Tree<T>>();
        }

        public Tree(T value, params Tree<T>[] children)
            : this(value)
        {
            this.value = value;
            foreach (var child in children) 
            { 
                child.parent = this;
                this.children.Add(child);
            }
        }

        public void AddChild(T parentKey, Tree<T> child)
        {
            var tree = FindNode(parentKey);
            if(tree != null)
            {
                tree.children.Add(child);
                child.parent = tree;
            }
            else 
                throw new ArgumentNullException();
        }

        private Tree<T> FindNode(T parentKey)
        {
            var queue = new Queue<Tree<T>>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var subtree = queue.Dequeue();
                if (subtree.value.Equals(parentKey))
                {
                    return subtree;
                }

                foreach (var child in subtree.children)
                {
                    queue.Enqueue(child);
                }
            }

            return null;
        }

        public IEnumerable<T> OrderBfs()
        {
            var queue = new Queue<Tree<T>>();
            var result = new List<T>();

            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var subtree = queue.Dequeue();
                result.Add(subtree.value);

                foreach (var child in subtree.children)
                {
                    queue.Enqueue(child);
                }
            }

            return result;
        }

        public IEnumerable<T> OrderDfs()
        {
            var stack = new Stack<Tree<T>>();
            var result = new Stack<T>();

            stack.Push(this);
            while (stack.Count > 0)
            {
                var subtree = stack.Pop();              
                foreach(var child in subtree.children)
                {
                    stack.Push(child);
                }

                result.Push(subtree.value);
            }

            return result;
        }

        public IEnumerable<T> OrderRecursiveDfs()
        {
            var result = new List<T>();
            Dfs(this, result);
            return result;
        }

        private void Dfs(Tree<T> tree, List<T> result)
        {
            foreach (var child in tree.children)
            {
                this.Dfs(child, result);
            }

            result.Add(tree.value);
        }

        public void RemoveNode(T nodeKey)
        {
            var treeToRemove = FindNode(nodeKey);

            if (treeToRemove.Equals(this))
                throw new ArgumentException(nameof(treeToRemove));
            if (treeToRemove is null)
                throw new ArgumentNullException(nameof(treeToRemove));

            treeToRemove.parent.children.Remove(treeToRemove);
            treeToRemove.parent = null;
        }

        public void Swap(T firstKey, T secondKey)
        {
            var firstTree = FindNode(firstKey);
            var secondTree = FindNode(secondKey);

            if (firstTree.Equals(this) || secondTree.Equals(this))
                throw new ArgumentException();

            var firsttreeParent = firstTree.parent;
            var secondtreeParent = secondTree.parent;

            var firstChildIndex = firsttreeParent.children.IndexOf(firstTree);
            var secondChildIndex = secondtreeParent.children.IndexOf(secondTree);

            if (firsttreeParent is null || secondtreeParent is null)
                throw new ArgumentNullException();

            if (firsttreeParent.children[firstChildIndex] == secondTree)
            {
                RemoveNode(firstKey);
                AddChild(firstKey, new Tree<T>(secondKey)); 
            }

            firsttreeParent.children[firstChildIndex] = secondTree;
            secondtreeParent.children[secondChildIndex] = firstTree;

            secondTree.parent = firsttreeParent;
            firstTree.parent = secondtreeParent;
        }
    }
}
