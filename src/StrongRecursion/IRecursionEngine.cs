namespace StrongRecursion
{
    /// <summary>
    /// Abstracts the ability to perform a recursive logic and return a result
    /// </summary>
    /// <typeparam name="TParams"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IRecursionEngine<TParams, TResult>
        where TParams : Params
        where TResult : Result, new()
    {
        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="prms"></param>
        /// <returns></returns>
        TResult Run(TParams prms);
    }
}
