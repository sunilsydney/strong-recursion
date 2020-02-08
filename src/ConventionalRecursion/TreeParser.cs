using System;
using Common.Tree;

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
            {
                return;
            }
            else
            {
                string data = node.Data;
                Log(data);
                // Note that console is redirected to ExampleTreeTraversal.TreeTraversal_Test
                // Note that this project (ConventionalRecursion) is always built for "Release"

                TraverseByConventionalRecurion(node.Left);
                TraverseByConventionalRecurion(node.Right);
            }
        }

        private void Log(string v)
        {
            Console.WriteLine(v);
            // System.Diagnostics.Debug.WriteLine(v);
        }
    }
}
