using System;
using System.Collections.Generic;
using StrongRecursion;
using StrongRecursion.Test.Tree;
using System.Linq;
using System.Threading;
using Xunit;

namespace StrongRecursion.Test
{    
    public class ExampleTreeTraversal
    {
        /// <summary>
        /// Performs a DFS pre-order traversal of a 10000-depth tree 
        /// using StrongRecursion and conventional recursion.
        /// This test proves conventional recursion runs into stack-overflow,
        /// while StrongRecursion does not.
        /// </summary>
        [Fact]
        public void TreeTraversalTest()
        {
            // Arrange
            var tree = TreeHelper.CreateTree(20000);

            // Action 1 : Using StrongRecurion to prove it doesn't cause stack-overflow

            // Action 2 : Using conventional recursion, to prove it causes stack-overflow
            Log("Traversing the tree using conventional recurion");
            Log("Doing this on a new thread so that main thread is able to report test status");

            try
            {
                Thread t1 = new Thread(DoWork);
                t1.Start(tree.RootNode);
            } 
            catch(Exception ex)
            {
                Log(ex.ToString());
            }

            Thread.Sleep(TimeSpan.FromSeconds(30));

            Assert.Equal("RootNode", tree.RootNode.Data); //Verifies stackoverflow happened. Otherwise, DoWork() would have modified this value
            /*
            In VisualStudio Output window (Tests), observe
            
            Executing test method: StrongRecursion.Test.ExampleTreeTraversal.TreeTraversalTest
            [1/02/2020 4:35:44.216 PM] ---------- Run started ----------
            The active test run was aborted. Reason: Test host process crashed : 
            Process is terminated due to StackOverflowException.
            [1/02/2020 4:36:01.343 PM] ========== Run aborted: 0 tests run (0:00:17.1237768) ==========
            */
        }

        public void DoWork(Object obj)
        {
            Node root = (Node)obj;
            try
            {
                TraverseByConventionalRecurion(root);
            }
            catch (Exception ex) // Chances of catching StackOverflow Exception are bleak
            {

                
            }
            
            root.Data = "Completed";
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
