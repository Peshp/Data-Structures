namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Tree<T> : IAbstractTree<T>
    {
        private T key;
        private Tree<T> parent;
        private List<Tree<T>> children;

        public Tree(T key, params Tree<T>[] children)
        {
            this.Key = key;
            this.key = key;
            this.children = new List<Tree<T>>();

            foreach (var child in children)
            {
                AddChild(child);
                child.parent = this;
            }
        }

        public T Key { get; private set; }

        public Tree<T> Parent { get; private set; }

        public IReadOnlyCollection<Tree<T>> Children => this.children.AsReadOnly();

        public void AddChild(Tree<T> child)
        {
            this.children.Add(child);
        }

        public void AddParent(Tree<T> parent)
        {
            foreach (var child in parent.children)
            {
                child.Parent = parent;
            }
        }

        public string AsString()
        {
            var result = new StringBuilder();

            DfsAsString(result, this, 0);

            return result.ToString().Trim();
        }

        public IEnumerable<T> GetInternalKeys()
        {
            var result = new List<T>();
            var queue = new Queue<Tree<T>>();

            queue.Enqueue(this);
            while(queue.Count > 0)
            {
                var subtree = queue.Dequeue();
                
                foreach (var child in subtree.children)
                {
                    queue.Enqueue(child);
                    if (child.Children.Count > 0)
                    {
                        result.Add(child.Key);
                    }
                }
            }

            result.Remove(this.Key);
            return result;
        }  

        public IEnumerable<T> GetLeafKeys()
        {
            var list = new List<T>();
            var queue = new Queue<Tree<T>>();

            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                var subtree = queue.Dequeue();

                foreach (var child in subtree.children)
                {
                    queue.Enqueue(child);
                    if (child.children.Count == 0)
                    {
                        list.Add(child.Key);
                    }
                }
            }

            return list;
        }

        public T GetDeepestKey()
        {
            var list = new List<T>();
            var result = DfsDeepSeatch(this, list);
            return result.First();
        }        

        public IEnumerable<T> GetLongestPath()
        {
            throw new NotImplementedException();
        }

        private List<T> DfsDeepSeatch(Tree<T> tree, List<T> list)
        {           
            foreach (var child in tree.Children)
            {
                DfsDeepSeatch(child, list);
            }

            list.Add(tree.Key);
            return list;
        }

        private void DfsAsString(StringBuilder result, Tree<T> tree, int a)
        {
            result.Append(' ', a).AppendLine(tree.Key.ToString());

            foreach (var child in tree.Children)
            {
                DfsAsString(result, child, a + 2);
            }
        }
    }
}
