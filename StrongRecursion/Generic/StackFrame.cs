namespace StrongRecursion.Generic
{
    public class StackFrame<P, R>
        where P : Params
        where R : Result, new()
    {
        public P Params { get; set; }
        public R Result { get; set; }

    }
}
