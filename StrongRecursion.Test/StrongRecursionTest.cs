using System;
using System.Collections.Generic;
using StrongRecursion;
using Xunit;

namespace StrongRecursion.Test
{    
    public class StrongRecursionTest
    {
        [Fact]
        public void PoC()
        {
            // Arrange
            int n = 5000;
            int expected = (n * (n + 1)) / 2;

            // Usual recursion using program's call-stack
            int actual = Sum(n);
            Assert.Equal(expected, actual);

            // PoC of the strong recursion idea that does not use program's call stack for recursion
            int actual1 = CoreLogic(n);
            Assert.Equal(expected, actual1);

            // PoC of RecursionBuilder
            int actual2 = StrongRecursion(n);
            Assert.Equal(expected, actual2);            
        }

        private int StrongRecursion(int input)
        {
            var result = new RecursionBuilder()
                .WithLimitingCondition((p) => 
                {
                    // Custom predicate for limiting condition by user
                    return (p.n == 1); 
                })
                .WithLimitingLogic((p, r) => 
                {
                    // Custom logic by user
                    return new Result() { res = 1 + r.res }; 
                })
                .WithLogic((p, r) =>
                {
                    // Custom logic by user
                    return new StackFrame()
                    {
                        Params = new Params() { n = p.n - 1 },
                        Result = new Result() { res = p.n + r.res }
                    };                    
                })
                .Run(new Params() { n = input });

            return result.res;
        }

        /// <summary>
        /// TODO reference one frame from another to sort of chain the calculation?
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int CoreLogic(int input)
        {
            Stack<StackFrame> stack = new Stack<StackFrame>();
            stack.Push(new StackFrame() 
            { 
                Params = new Params() { n = input },
                Result = new Result() { res = 0 }
            });

            int finalResult = 0;

            while(stack.Count > 0)
            {
                var frame = stack.Pop();

                if(frame.Params.n == 1)
                {
                    finalResult = frame.Result.res + frame.Params.n;
                }
                else
                {
                    var newFrame = new StackFrame() {
                        Params = new Params() { n = frame.Params.n-1 },
                        Result = new Result() { res = frame.Result.res + frame.Params.n }
                    };
                    stack.Push(newFrame);
                }
            }
            return finalResult;
        }

        private int Sum(int n)
        {
            // sum = 1 + 2 + 3 + ..... + n
            // Limiting condition
            if( n == 1)
            {
                return 1;
            }
            return n + Sum(n - 1);
        }
    }

    
}
