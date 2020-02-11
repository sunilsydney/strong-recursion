using System.Collections.Generic;
using StrongRecursion.Test.UserDefined;
using Xunit;

namespace StrongRecursion.Test
{
    public class StrongRecursionTest
    {
        /// <summary>
        /// Proof of concept of StrongRecursion
        /// </summary>
        [Fact]
        public void StrongRecursion_POC_Test()
        {
            // Arrange
            int n = 5000;
            int expected = (n * (n + 1)) / 2;

            // Usual recursion using program's call-stack
            int actual = Sum(n);
            Assert.Equal(expected, actual);

            // PoC of the strong recursion idea that does not use program's call stack for recursion
            int actual1 = CoreLogic_LiteMode(n);
            Assert.Equal(expected, actual1);

            // PoC of RecursionBuilder
            int actual2 = StrongRecursion(n);
            Assert.Equal(expected, actual2);
        }

        private int StrongRecursion(int input)
        {
            var builder = new RecursionBuilder<DemoParams, DemoResult>()
                .If((p) =>
                {
                    // Custom predicate for limiting condition by user
                    return (p.N == 1);
                })
                .Then((p, r) =>
                {
                    // Custom logic by user
                    return new DemoResult() { Res = 1 + r.Res };
                })
                .Else((p, r) =>
                {
                    // Custom logic by user
                    var inputParams = (DemoParams)p;
                    var prevResult = (DemoResult)r ?? new DemoResult();
                    return new StackFrame<DemoParams, DemoResult>()
                    {
                        Params = new DemoParams() { N = inputParams.N - 1 },
                        Result = new DemoResult() { Res = inputParams.N + prevResult.Res }
                    };
                });

            var recursion = builder.Build();
            var result = recursion.Run(new DemoParams() { N = input });

            return result.Res;
        }

        /// <summary>
        /// Demostrates the core logic of strong-recursion Lite mode.
        /// Lite mode uses less memory and does not support returning a value
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int CoreLogic_LiteMode(int input)
        {
            Stack<StackFrame<DemoParams, DemoResult>> stack = new Stack<StackFrame<DemoParams, DemoResult>>();
            stack.Push(new StackFrame<DemoParams, DemoResult>()
            {
                Params = new DemoParams() { N = input },
                Result = new DemoResult() { Res = 0 }
            });

            int finalResult = 0;

            while (stack.Count > 0)
            {
                var frame = stack.Pop();

                var frameParams = (DemoParams)frame.Params;
                var frameResult = (DemoResult)frame.Result;
                if (frameParams.N == 1)
                {
                    finalResult = frameResult.Res + frameParams.N;
                }
                else
                {
                    var newFrame = new StackFrame<DemoParams, DemoResult>() {
                        Params = new DemoParams() { N = frameParams.N - 1 },
                        Result = new DemoResult() { Res = frameResult.Res + frameParams.N }
                    };
                    stack.Push(newFrame);
                }
            }
            return finalResult;
        }

        /// <summary>
        /// Demonstrates strong recursion with return values, 
        /// solving factorial problem as an example
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(5)]
        public int CoreLogic_Factorial_WithReturnValue_Test(int input)
        {
            Stack<StackFrame<DemoParams, DemoResult>> stack = new Stack<StackFrame<DemoParams, DemoResult>>();
            stack.Push(new StackFrame<DemoParams, DemoResult>()
            {
                Params = new DemoParams() { N = input },
                Result = null
            }) ;

            StackFrame<DemoParams, DemoResult> poppedFrame = null;

            while (stack.Count > 0)
            {
                var frame = stack.Peek();

                if(frame.Result == null) // Winding Up
                {
                    if (frame.Params.N == 0 )
                    {                        
                        // Or pop and push a new frame for RecursionEngine implementation
                        var topFrame = stack.Peek();                     
                        topFrame.Result = new DemoResult() { Res = 1 };
                    }
                    else
                    {
                        var newFrame = new StackFrame<DemoParams, DemoResult>()
                        {
                            Params = new DemoParams() { N = frame.Params.N - 1 },
                            Result = null
                        };
                        stack.Push(newFrame);
                    }
                }
                else // Winding down
                {
                    poppedFrame = stack.Pop();
                    if (stack.Count > 0)
                    {
                        var topFrame = stack.Peek();

                        topFrame.Result = new DemoResult
                        {
                            Res = topFrame.Params.N * poppedFrame.Result.Res
                        };
                    }
                }
            }

            int expected = Factorial(input); // Using conventional recursion
            Assert.Equal(expected, poppedFrame.Result.Res);

            return poppedFrame.Result.Res;
        }

        /// <summary>
        /// Factorial using conventional call-stack based recursion
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private int Factorial(int n)
        {
            if (n == 0 || n == 1)
                return 1;
            else
                return n * Factorial(n - 1);
        }

        /// <summary>
        /// Returns the sum of all positive integers up to n including n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private int Sum(int n)
        {
            // Base case aka Limiting condition
            if( n == 1)
            {
                return 1;
            }
            return n + Sum(n - 1);
        }
    }
    
}
