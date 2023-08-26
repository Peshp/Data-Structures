namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class IntegerTreeFactory
    {
        private Dictionary<int, IntegerTree> nodesByKey;

        public IntegerTreeFactory()
        {
            this.nodesByKey = new Dictionary<int, IntegerTree>();
        }

        public IntegerTree CreateTreeFromStrings(string[] input)
        {
            foreach (var node in input)
            {
                var keys = node.ToString().Split().Select(int.Parse).ToArray();

                var parent = keys[0];
                var child = keys[1];

                AddEdge(parent, child);
            }

            return GetRoot();
        }

        public IntegerTree CreateNodeByKey(int key)
        {
            if(!this.nodesByKey.ContainsKey(key))
            {
                nodesByKey.Add(key, new IntegerTree(key));
            }

            return this.nodesByKey[key];
        }

        public void AddEdge(int parent, int child)
        {
            var parentNode = CreateNodeByKey(parent);
            var childNode = CreateNodeByKey(child);

            parentNode.AddChild(childNode);
            childNode.AddParent(parentNode);
        }

        public IntegerTree GetRoot()
        {
            foreach(var node in nodesByKey)
            {
                if (nodesByKey.ContainsKey(node.Key))
                {
                    return nodesByKey[node.Key];
                }
            }

            return null;
        }
    }
}
