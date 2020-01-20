using System;
using System.Collections.Generic;

namespace StrongRecursion
{
    public class RecursionBuilder
    {
        public RecursionBuilder()
        { }

        Func<Params, bool> _limitingCondition;
        Func<Params, Result, Result> _limitingLogic;
        Func<Params, Result, StackFrame> _logic;

        public RecursionBuilder WithLimitingCondition(Func<Params, bool> func)
        {
            _limitingCondition = func;
            return this;
        }

        public RecursionBuilder WithLimitingLogic(Func<Params, Result, Result> func)
        {
            _limitingLogic = func;
            return this;
        }

        public RecursionBuilder WithLogic(Func<Params, Result, StackFrame> func)
        {
            _logic = func;
            return this;
        }

        public Result Run(Params prms)
        {
            Stack<StackFrame> stack = new Stack<StackFrame>();
            // Initial frame
            stack.Push(new StackFrame()
            {
                Params = prms,
                Result = new Result() { res = 0 }
            });

            Result finalResult = null;

            while (stack.Count > 0)
            {
                var frame = stack.Pop();

                // Limiting condition
                if(_limitingCondition(frame.Params))
                {
                    //int sum = frame.Result.res + frame.Params.n;
                    //finalResult = new Result() { res = sum };
                    finalResult = _limitingLogic(frame.Params, frame.Result);
                }
                else
                {
                    //var newFrame = new StackFrame()
                    //{
                    //    Params = new Params() { n = frame.Params.n - 1 },
                    //    Result = new Result() { res = frame.Result.res + frame.Params.n }
                    //};

                    var newFrame = _logic(frame.Params, frame.Result);
                    stack.Push(newFrame);
                }
            }
            return finalResult;
        }
    }
}
