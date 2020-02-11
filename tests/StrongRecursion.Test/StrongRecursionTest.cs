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
            int actual1 = CoreLogic(n);
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
        /// Demostrates the core logic of strong-recursion
        /// Consider referencing one frame from another to sort of chain the calculation?
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int CoreLogic(int input)
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
        /// Demonstrates return values, solving factorial problem as an example
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
                    var inParams = (DemoParams)frame.Params;
                    //var inResult = (DemoResult)frame.Result; // Will be null
                    if (inParams.N == 0 )
                    {
                        var newFrame = new StackFrame<DemoParams, DemoResult>()
                        {
                            Params = null,
                            Result = new DemoResult() { Res = 1 }
                        };
                        stack.Pop(); // Important
                        stack.Push(newFrame);
                    }
                    else
                    {
                        var newFrame = new StackFrame<DemoParams, DemoResult>()
                        {
                            Params = new DemoParams() { N = inParams.N - 1 },
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

                        if(poppedFrame.Params == null) // poppedFrame came from Base case
                        {
                            topFrame.Result = poppedFrame.Result; // Supply the result backwards 
                        }
                        else
                        {
                            topFrame.Result = new DemoResult
                            {
                                Res = topFrame.Params.N * poppedFrame.Result.Res
                            };
                        }
                    }
                }
            }

            int expected = Factorial(input); // Using conventional recursion
            Assert.Equal(expected, poppedFrame.Result.Res);

            return poppedFrame.Result.Res;
        }

        private int Factorial(int n)
        {
            if (n == 0 || n == 1)
                return 1;
            else
                return n * Factorial(n - 1);
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
