using System;
using System.Collections.Generic;
using System.Text;

namespace StrongRecursion
{
    public class StackFrame
    {
        public int n = 0;
        public int result = 0;

        public Params Params => null;
        public Result Result => null;
        
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
