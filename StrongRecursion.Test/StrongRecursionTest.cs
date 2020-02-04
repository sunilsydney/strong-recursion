using System.Collections.Generic;
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
            var result = new RecursionBuilder()
                .WithLimitingCondition((p) =>
                {
                    // Custom predicate for limiting condition by user
                    var inputParams = (DemoParams)p;
                    return (inputParams.N == 1);
                })
                .WithLimitingLogic((p, r) =>
                {
                    // Custom logic by user
                    var prevResult = (DemoResult)r;
                    return new DemoResult() { Res = 1 + prevResult.Res };
                })
                .WithLogic((p, r) =>
                {
                    // Custom logic by user
                    var inputParams = (DemoParams)p;
                    var prevResult = (DemoResult)r;
                    return new StackFrame()
                    {
                        Params = new DemoParams() { N = inputParams.N - 1 },
                        Result = new DemoResult() { Res = inputParams.N + prevResult.Res }
                    };
                })
                .WithInitialResult(new DemoResult() { Res = 0 })
                .Run(new DemoParams() { N = input });

            return ((DemoResult)result).Res;
        }

        /// <summary>
        /// Consider referencing one frame from another to sort of chain the calculation?
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int CoreLogic(int input)
        {
            Stack<StackFrame> stack = new Stack<StackFrame>();
            stack.Push(new StackFrame() 
            { 
                Params = new DemoParams() { N = input },
                Result = new DemoResult() { Res = 0 }
            });

            int finalResult = 0;

            while(stack.Count > 0)
            {
                var frame = stack.Pop();

                var frameParams = (DemoParams)frame.Params;
                var frameResult = (DemoResult)frame.Result;
                if(frameParams.N == 1)
                {
                    finalResult = frameResult.Res + frameParams.N;
                }
                else
                {
                    var newFrame = new StackFrame() {
                        Params = new DemoParams() { N = frameParams.N -1 },
                        Result = new DemoResult() { Res = frameResult.Res + frameParams.N }
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
