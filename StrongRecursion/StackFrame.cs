namespace StrongRecursion
{
    public class StackFrame
    {
        public Params Params { get; set; }
        public Result Result { get; set; }

    }

    /// <summary>
    /// User to extend this class to add more params
    /// </summary>
    public class Params
    {
        public int n = 0;
    }

    /// <summary>
    /// User to extend this class to add more params
    /// </summary>
    public class Result
    {
        public int res = 0;
    }
}
