using System;
using System.Collections.Generic;
using System.Text;

namespace StrongRecursion
{

    public class ThenList<TParams, TResult> 
            : List<Func<TParams, TResult, StackFrame<TParams, TResult>>>
            where TParams : Params
            where TResult : Result, new()
    {
    }
}
