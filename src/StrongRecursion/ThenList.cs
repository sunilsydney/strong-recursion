using System;
using System.Collections.Generic;

namespace StrongRecursion
{

    public class ThenList<TParams, TResult> 
            : List<Func<TParams, TParams>>
            where TParams : Params
            where TResult : Result, new()
    {
    }
}
