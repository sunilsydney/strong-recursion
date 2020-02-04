using System;
using System.Collections.Generic;

namespace StrongRecursion.Generic
{
    /// <summary>
    /// Generic Recursion Builder
    /// </summary>
    /// <typeparam name="P"></typeparam>
    /// <typeparam name="R"></typeparam>
    public class RecursionBuilder<P, R> 
        where P: Params
        where R: Result, new()
        //where T: new() --> The type argument must have a public parameterless constructor. 
        //When used together with other constraints, the new() constraint must be specified last.
        // TODO consider placing the constraint on an interface rather than an abstract class?
    {
        public RecursionBuilder()
        { }

        Func<P, bool> _limitingCondition;
        Func<P, R, R> _limitingLogic;
        Func<P, R, StackFrame<P, R>> _logic;
        //R _initialResult = null;


        public RecursionBuilder<P, R> WithLimitingCondition(Func<P, bool> func)
        {
            _limitingCondition = func;
            return this;
        }

        public RecursionBuilder<P, R> WithLimitingLogic(Func<P, R, R> func)
        {
            _limitingLogic = func;
            return this;
        }

        public RecursionBuilder<P, R> WithLogic(Func<P, R, StackFrame<P, R>> func)
        {
            _logic = func;
            return this;
        }

        //public RecursionBuilder<P, R> WithInitialResult(R result)
        //{
        //    _initialResult = result;
        //    return this;
        //}

        public Result Run(P prms)
        {
            // TODO validate all private members

            Stack<StackFrame<P, R>> stack = new Stack<StackFrame<P, R>>();
            R finalResult = null;

            // Initial frame
            stack.Push(new StackFrame<P, R> ()
            {
                Params = prms,
                Result = new R()
            });            

            // Recursion using a stack on heap memory, without costing call-stack
            while (stack.Count > 0)
            {
                var frame = stack.Pop();

                // Limiting condition
                if (_limitingCondition(frame.Params))
                {                    
                    finalResult = _limitingLogic(frame.Params, frame.Result);
                }
                else
                {
                    var newFrame = _logic(frame.Params, frame.Result);
                    stack.Push(newFrame);
                }
            }

            return finalResult;
        }
    }
}

