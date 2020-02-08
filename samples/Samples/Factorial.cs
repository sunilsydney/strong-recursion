using StrongRecursion;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Samples
{
    public class Factorial
    {
        [Fact]
        public void Factorial_Sample_Test()
        {
            // Arrange
            int number = 12;
            int expected = Facto(number);

            // Action
            int actual = FactorialByStrongRecursion(number);

            // Assert
            Assert.Equal(expected, actual);
        }


        private int Facto(int n)
        {
            if(n == 0)
            {
                return 1;
            }
            else
            {
                return n * Facto(n - 1);
            }
        }

        /// <summary>
        /// TODO make this closer to original code
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int FactorialByStrongRecursion(int number)
        {
            var answer = new RecursionBuilder<FParams, FResult>()
                .If((p) =>
                {
                    return (p.n == 0);
                })
                .Then((p, r) =>
                {
                    return new FResult() { result = 1 * r.result };
                })
                .Else((p, r) =>
                {                   
                    return new StackFrame<FParams, FResult>()
                    {
                        Params = new FParams { n = p.n - 1 },
                        Result = new FResult { result = p.n * r.result }
                    };
                })
                .Build()
                .Run(new FParams { n = number });

            return answer.result;
        }

    }

    public class FParams : Params
    {
        public int n;
    }
    public class FResult : Result
    {
        public int result;
        public FResult() { result = 1; } // Default value specific to problem domain
    }
}
