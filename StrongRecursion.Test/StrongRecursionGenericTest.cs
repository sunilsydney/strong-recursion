using System.Collections.Generic;
using Xunit;
using StrongRecursion.Generic;

namespace StrongRecursion.Test
{
    public class StrongRecursionGenericTest
    {
        /// <summary>
        /// Proof of concept of StrongRecursion, using Generic constract
        /// </summary>
        [Fact]
        public void StrongRecursionGeneric_POC_Test()
        {
            // Arrange
            int n = 5000;
            int expected = (n * (n + 1)) / 2;

            // Usual recursion using program's call-stack
            int actual = Sum(n);
            Assert.Equal(expected, actual);
                        
            // PoC of RecursionBuilderGeneric
            int actual2 = StrongRecursion(n);
            Assert.Equal(expected, actual2);            
        }

        private int StrongRecursion(int input)
        {
            var result = new RecursionBuilder<DemoParams, DemoResult>()
                .WithLimitingCondition((p) =>
                {
                    // Custom predicate for limiting condition by user
                    return (p.N == 1);
                })
                .WithLimitingLogic((p, r) =>
                {
                    // Custom logic by user
                    return new DemoResult() { Res = 1 + r.Res };
                })
                .WithLogic((p, r) =>
                {
                    // Custom logic by user
                    return new StackFrame<DemoParams, DemoResult>()
                    {
                        Params = new DemoParams() { N = p.N - 1 },
                        Result = new DemoResult() { Res = p.N + r.Res }
                    };
                })
                .Run(new DemoParams() { N = input });

            return ((DemoResult)result).Res;
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
