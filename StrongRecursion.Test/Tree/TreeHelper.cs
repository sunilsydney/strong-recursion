namespace StrongRecursion.Test.Tree
{
    public class TreeHelper
    {
        public static readonly int MAX_DEPTH = 100000;
        /// <summary>
        /// Creates a sample tree in memory with a given depth
        /// Depth 0 is root node alone
        /// Depth 1 is root node with two children, and so on.
        /// Note that only left nodes will have children for simplicity.
        /// </summary>
        /// <param name="depth"></param>
        public static Tree CreateTree(int depth)
        {
            if(depth < 0 || depth > MAX_DEPTH)
            {
                return null;
            }
            var tree = new Tree() 
            { 
                Name = "SampleTree", 
                Description = "To demonstrate StrongRecursion",
                RootNode = new Node { Data = "RootNode"}                
            };

            Node currentNode = tree.RootNode;

            for (int i = 1; i <= depth; i++)
            {
                currentNode.Left = new Node() { Data = $"Left node at depth {i}" };
                currentNode.Right = new Node() { Data = $"Right node at depth {i}" };

                // Tree grows only on the left side for this example
                currentNode = currentNode.Left;
            }

            return tree;
        }
    }
}
