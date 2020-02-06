using System;
using System.Collections.Generic;

namespace StrongRecursion
{
    public class RecursionBuilder
    {
        public RecursionBuilder()
        { }

        private Func<Params, bool> _if = null;
        private Func<Params, Result, Result> _then = null;
        private List<Func<Params, bool>> _elseIfList = new List<Func<Params, bool>>();
        // TODO support returning of multiple StackFrames. 
        private List<Func<Params, Result, StackFrame>> _thenList = new List<Func<Params, Result, StackFrame>>();
        // TODO support returning of multiple StackFrames. 
        private Func<Params, Result, StackFrame> _else = null;
        // Result _initialResult = null;


        /// <summary>
        /// Limiting condition of the recursion
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder If(Func<Params, bool> func)
        {
            ValidateState(nameof(If));
            _if = func;
            return this;
        }

        /// <summary>
        /// Exit logic of the recursion
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder Then(Func<Params, Result, Result> func)
        {
            ValidateState(); // Special case
            _then = func;
            return this;
        }
        
        /// <summary>
        /// When evaluated to true, leads to a recursive action
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder ElseIf(Func<Params, bool> func)
        {
            _elseIfList.Add(func); // Will match with elemet of _thenList at same index
            return this;
        }

        /// <summary>
        /// A recursive action performed when the previous "ElseIf" evaluated to true
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder Then(Func<Params, Result, StackFrame> func)
        {
            ValidateState(nameof(Then));
            _thenList.Add(func); // Will match with elemet of _elseIfList at same index
            return this;
        }

        /// <summary>
        /// Optional action to perform if no condition evaluated to true
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public RecursionBuilder Else(Func<Params, Result, StackFrame> func)
        {
            ValidateState(nameof(Else));
            _else = func;
            return this;
        }


        //public RecursionBuilder WithInitialResult(Result result)
        //{
        //    _initialResult = result;
        //    return this;
        //}

        public Result Run(Params prms)
        {
            // TODO validate all private members

            Stack<StackFrame> stack = new Stack<StackFrame>();
            // Initial frame
            stack.Push(new StackFrame()
            {
                Params = prms
            });

            Result finalResult = null;
            bool elseIfTriggered = false;

            while (stack.Count > 0)
            {
                var frame = stack.Pop();
                elseIfTriggered = false;

                // Limiting condition
                if (_if(frame.Params))
                {
                    if(_then == null)
                    {
                        finalResult = frame.Result; //TODO User did not specify limiting logic
                    }
                    else
                    {
                        finalResult = _then(frame.Params, frame.Result);
                    }
                }
                else
                {                    
                    for (int p = 0; p < _elseIfList.Count; p++)
                    {
                        var predicate = _elseIfList[p];
                        if (predicate(frame.Params))
                        {
                            var action = _thenList[p];
                            var newFrame = action(frame.Params, frame.Result);
                            stack.Push(newFrame);
                            elseIfTriggered = true;

                            break; // Important
                        }
                    }

                    if (!elseIfTriggered && _else != null)
                    {
                        var newFrame = _else(frame.Params, frame.Result);
                        stack.Push(newFrame);
                    }                    
                }
            }
            return finalResult;
        }

        private void ValidateState(string methodName)
        {
            // TODO
            // Validate order, sequence and chaining, and Throw custome exception if there are issues
            /*
            if(a == 1)
            {
            }
            Then
            {
            }
            ElseIf(a == 2)
            {
            }
            Then
            {
            }
            ElseIf(a == 3)
            {
            }
            Then
            {
            }
            Else
            {
            }

            */
            // throw new NotImplementedException();
        }

        private void ValidateState()
        {
            // TODO 
        }
    }
}
