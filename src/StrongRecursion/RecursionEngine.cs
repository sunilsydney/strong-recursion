using System;
using System.Collections.Generic;

namespace StrongRecursion
{
    /// <summary>
    /// Internal class, not visible to SDK users
    /// </summary>
    internal class RecursionEngine<TParams, TResult> : IRecursionEngine<TParams, TResult>
        where TParams : Params
        where TResult : Result, new()

    {
        internal Func<TParams, bool> If = null;
        // Then0
        internal Func<TParams, TResult, TResult> Then = null;

        internal List<Func<TParams, bool>> ElseIfList = new List<Func<TParams, bool>>();

        // List of Then1 
        internal List<Func<TParams, TResult, StackFrame<TParams, TResult>>> ThenList 
            = new List<Func<TParams, TResult, StackFrame<TParams, TResult>>>();

        // List of Then2
        internal List<Func<TParams, TResult, StackFrame<TParams, TResult>>> ElseList
            = new List<Func<TParams, TResult, StackFrame<TParams, TResult>>>();
       

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
                Params = prms
            });

            TResult finalResult = null;
            bool elseIfTriggered = false;

            while (stack.Count > 0)
            {
                var frame = stack.Pop();
                elseIfTriggered = false;

                // Limiting condition
                if (If(frame.Params))
                {
                    if (Then == null)
                    {
                        finalResult = frame.Result; //TODO User did not specify limiting logic
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
                            var action = ThenList[p];
                            var newFrame = action(frame.Params, frame.Result);
                            stack.Push(newFrame);
                            elseIfTriggered = true;

                            break; // Important
                        }
                    }

                    //if (!elseIfTriggered && _else != null)
                    //{
                    //    var newFrame = _else(frame.Params, frame.Result);
                    //    stack.Push(newFrame);
                    //}
                }
            }
            return finalResult;
        }

    }
}
