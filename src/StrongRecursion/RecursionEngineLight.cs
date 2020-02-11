using System;
using System.Collections.Generic;

namespace StrongRecursion
{
    /// <summary>
    /// Internal class, not visible to SDK users.
    /// LightMode is in Beta stage.
    /// </summary>
    internal class RecursionEngineLight<TParams, TResult> : IRecursionEngine<TParams, TResult>
        where TParams : Params
        where TResult : Result, new()

    {
        internal Func<TParams, bool> If = null;

        // Then0
        internal Func<TParams, TResult, TResult> Then = null;

        internal List<Func<TParams, bool>> ElseIfList = new List<Func<TParams, bool>>();

        // List of List of Then1 
        internal List<ThenList<TParams, TResult>> ThenListList
            = new List<ThenList<TParams, TResult>>();

        // List of Then2
        internal ThenList<TParams, TResult> ElseList
            = new ThenList<TParams, TResult>();
       
        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="prms"></param>
        /// <returns></returns>
        public TResult Run(TParams prms)
        {
            Stack<StackFrame<TParams, TResult>> stack = new Stack<StackFrame<TParams, TResult>>();
            // Initial frame
            stack.Push(new StackFrame<TParams, TResult>()
            {
                Params = prms,
                Result = new TResult()
            });

            TResult finalResult = null;

            while (stack.Count > 0)
            {
                var frame = stack.Pop();
                bool elseIfTriggered = false;

                // Base case aka Limiting condition
                if (If(frame.Params))
                {
                    if (Then == null)
                    {
                        finalResult = new TResult(); // User did not specify, returning default
                    }
                    else
                    {
                        finalResult = Then(frame.Params, frame.Result);
                    }
                }
                else
                {
                    for (int p = 0; p < ElseIfList.Count; p++)
                    {
                        var predicate = ElseIfList[p];
                        if (predicate(frame.Params))
                        {
                            elseIfTriggered = true;
                            var actions = ThenListList[p];

                            // Grab in reverse order
                            for (int i = actions.Count - 1; i >= 0; i--)
                            {
                                // TODO : Might need in-memory links for chaining results in right sequence
                                var action = actions[i];
                                var newFrame = action(frame.Params, frame.Result);
                                stack.Push(newFrame);
                            }
                            break; // Important
                        }
                    }

                    if (!elseIfTriggered && ElseList?.Count> 0)
                    {
                        // Grab in reverse order
                        for (int i = ElseList.Count - 1; i >= 0; i--)
                        {
                            var action = ElseList[i];
                            var newFrame = action(frame.Params, frame.Result);
                            stack.Push(newFrame);
                        }
                    }
                }
            }
            return finalResult;
        }

    }
}
