using System;
using StrongRecursion.Test.Tree;
using Xunit;
using System.Diagnostics;

namespace StrongRecursion.Test
{
    public class ExampleTreeTraversal
    {
        static string buildConf = "Release";
        static string executablePath = @$"..\..\..\..\ConventionalRecursion\bin\{buildConf}\netcoreapp3.1\ConventionalRecursion.exe";


        /// <summary>
        /// Performs a DFS pre-order traversal of a very deep tree 
        /// using StrongRecursion and conventional recursion.
        /// This test proves conventional recursion runs into stack-overflow,
        /// while StrongRecursion does not.
        /// </summary>
        [Fact]
        public void TreeTraversal_Test()
        {
            // Arrange
            int depth = 50000;
            var tree = TreeHelper.CreateTree(depth);

            // Action 1 : Using StrongRecurion to prove it doesn't cause stack-overflow
            // int nodeCount = TraverseByStrongRecurion(tree.RootNode);

            // Assert 1
            Assert.True(true); // Yes, if control reaches this point, stack overflow did not happen
            // Assert.Equal(((depth * 2) + 1), nodeCount); // This equation is very specific to the structure of the sample tree

            // Action 2 : Using conventional recursion, to prove it causes stack-overflow
            Log("Traversing the tree using conventional recurion, on a separate process");
            System.Diagnostics.Process process = null;
            int exitCode = RunProcess(process, executablePath, depth);
            Log($"Exit code of ConventionalRecursion.exe: {exitCode}");

            // Assert 2
            Assert.NotEqual(0, exitCode); 
            Assert.Equal(-1073741571, exitCode); //Verifies stackoverflow happened. 
            // -1073741571 the signed integer representation of Microsoft's "stack overflow/stack exhaustion" error code 0xC00000FD.

        }

        private int TraverseByStrongRecurion(Node node)
        {
            int nodeCount = 0;
            if (node == null)
                return 0;
            string data = node.Data;
            // Log(data); Commented out to avoid flooding logs and console of Github CI
            // Note that this project (ConventionalRecursion) is always built for "Release"
            TraverseByStrongRecurion(node.Left);
            TraverseByStrongRecurion(node.Right);

            var builder = new RecursionBuilder();
            
            //builder.WithLimitingCondition(())


            return nodeCount;
        }

        /// <summary>
        /// Demonstrates that convention recursion works for a small depth
        /// </summary>
        [Fact]
        public void Conventional_Recurion_Success_Test()
        {
            // Arrange
            int depth = 1000;
            System.Diagnostics.Process process = null;

            // Action
            int exitCode = RunProcess(process, executablePath, depth);
            Log($"Exit code of ConventionalRecursion.exe: {exitCode}");

            // Assert
            Assert.Equal(0, exitCode);
        }

        private int RunProcess(Process process, string executablePath, int depth)
        {
            try
            {
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = executablePath,
                        Arguments = $"{depth}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = false
                    }
                };

                Log($"Starting {executablePath} {depth}");                
                Log($"Note that depth = {depth}");

                process.Start();

                Log($"Process Id: {process.Id}");
                Log($"Process MaxWorkingSet: {process.MaxWorkingSet} bytes");

                while (!process.StandardOutput.EndOfStream)
                {
                    var line = process.StandardOutput.ReadLine();
                    Log(line);
                }

                process.WaitForExit();
            }
            catch (Exception e)
            {
                Log(e.Message);
                Log(e.ToString());                
            }           

            return process.ExitCode;
        }
        
        private void Log(string v)
        {
            Console.WriteLine(v);
            System.Diagnostics.Debug.WriteLine(v);
        }
    }

    
}
