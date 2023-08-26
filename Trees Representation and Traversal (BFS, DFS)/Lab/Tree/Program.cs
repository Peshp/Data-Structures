namespace Tree
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Program
    {
        public static void Main(string[] args)
        {
            var tree = new Tree<int>(34);
            tree.AddChild(34, new Tree<int>(20));
            tree.AddChild(34, new Tree<int>(25));
            tree.AddChild(34, new Tree<int>(88));
            tree.AddChild(88, new Tree<int>(5));

            Console.WriteLine(string.Join(" ", tree.OrderBfs()));

            tree.Swap(88, 5);

            Console.WriteLine(string.Join(" ", tree.OrderBfs()));
        }
    }
}
