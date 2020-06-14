using System;
namespace StrongRecursion
{
    /// <summary>
    /// Abstract the ability to define a recursion using fluent syntax
    /// </summary>
    /// <typeparam name="TParams"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IRecursionBuilder<TParams, TResult>
        where TParams : Params
        where TResult : Result, new()
        //where T: new() --> The type argument must have a public parameterless constructor
    {
        /// <summary>
        /// The condition of base case aka limiting logic
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        RecursionBuilder<TParams, TResult> If(Func<TParams, bool> func);

        /// <summary>
        /// The logic of base case aka limiting logic
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        RecursionBuilder<TParams, TResult> Then(Func<TParams, TResult> func);

        /// <summary>
        /// When evaluated to true, leads to a recursive action
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        RecursionBuilder<TParams, TResult> ElseIf(Func<TParams, bool> func);

        /// <summary>
        /// The logic to perform when the preceding condition is true.
        /// When used immediately following "ElseIf" or "Else", this can be chained to support multiple actions or logical branching
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        RecursionBuilder<TParams, TResult> Then1(Func<TParams, TResult, TResult> func);

        /// <summary>
        /// Optional action to perform if no condition evaluated to true.
        /// Can be chained using "Then" to support multiple actions
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        RecursionBuilder<TParams, TResult> Else(Func<TParams, TResult, TResult> func);
        
        /// <summary>
        /// Builds a recursion engine from the current state of builder
        /// </summary>
        /// <returns></returns>
        IRecursionEngine<TParams, TResult> Build();
    }
}
