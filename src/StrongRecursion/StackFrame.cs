namespace StrongRecursion
{
    public class StackFrame<TParams, TResult>
        where TParams : Params
        where TResult : Result, new()
    {
        public TParams Params { get; set; }
        public TResult Result { get; set; }

    }
}
