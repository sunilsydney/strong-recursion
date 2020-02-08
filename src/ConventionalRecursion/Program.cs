using System;
using Common.Tree;

namespace ConventionalRecursion
{
    /// <summary>
    /// A console application to demonstrate stackoverflow
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int depth = 1000;
            if(args.Length > 0)
            {
                if(int.TryParse(args[0], out int d))
                {
                    depth = d;
                }
            }

            var tree = TreeHelper.CreateTree(depth);

            var parser = new TreeParser();

            parser.TraverseByConventionalRecurion(tree.RootNode);
        }        
    }
}
