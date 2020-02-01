using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongRecursion;
using StrongRecursion.Test.Tree;
using System.Linq;

namespace StrongRecursion.Test
{
    [TestClass]
    public class ExampleTreeTraversal
    {
        /// <summary>
        /// Performs a DFS pre-order traversal of a 10000-depth tree 
        /// using StrongRecursion and conventional recursion.
        /// This test proves conventional recursion runs into stack-overflow,
        /// while StrongRecursion does not.
        /// </summary>
        [TestMethod]
        public void TreeTraversalTest()
        {
            // Arrange
            var tree = TreeHelper.CreateTree(20000);

            // Action 1 : Using StrongRecurion to prove it doesn't cause stack-overflow

            // Action 2 : Using conventional recursion, to prove it causes stack-overflow
            Log("Traversing the tree using conventional recurion");
            TraverseByConventionalRecurion(tree.RootNode);

            /*
            In VisualStudio Output window (Tests), observe
            
            Executing test method: StrongRecursion.Test.ExampleTreeTraversal.TreeTraversalTest
            [1/02/2020 4:35:44.216 PM] ---------- Run started ----------
            The active test run was aborted. Reason: Test host process crashed : 
            Process is terminated due to StackOverflowException.
            [1/02/2020 4:36:01.343 PM] ========== Run aborted: 0 tests run (0:00:17.1237768) ==========
            */
        }

        private void TraverseByConventionalRecurion(Node node)
        {            
            if (node == null)
                return;
            string data = node.Data;
            Log(data);
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
