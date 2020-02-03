using System;
using StrongRecursion.Test.Tree;

namespace ConventionalRecursion
{
    public class TreeParser
    {
        /// <summary>
        /// Performs a pre-order depth-first-search (DFS)
        /// using conventional call-stack based recursion
        /// </summary>
        /// <param name="node"></param>
        public void TraverseByConventionalRecurion(Node node)
        {
            if (node == null)
                return;
            string data = node.Data;
            // Log(data); Commented out to avoid flooding logs and console of Github CI
            // Note that this project (ConventionalRecursion) is always built for "Release"
            TraverseByConventionalRecurion(node.Left);
            TraverseByConventionalRecurion(node.Right);
        }

        private void Log(string v)
        {
            Console.WriteLine(v);
            System.Diagnostics.Debug.WriteLine(v);
        }
    }
}
