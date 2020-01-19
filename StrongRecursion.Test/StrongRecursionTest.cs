using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StrongRecursion;

namespace StrongRecursion.Test
{
    [TestClass]
    public class StrongRecursionTest
    {
        [TestMethod]
        public void PoC()
        {
            // Arrange
            int n = 5000;
            int expected = (n * (n + 1)) / 2;

            int actual1 = StrongRecursion(n);
            Assert.AreEqual(expected, actual1);

            int actual2 = SafeRecursion(n);
            Assert.AreEqual(expected, actual2);

            int actual = Sum(n);
            Assert.AreEqual(expected, actual);
        }

        private int SafeRecursion(int input)
        {
            var builder = new RecursionBuilder()
                .WithLimitingCondition((p) => { return true; })
                .WithLimitingLogic((p, r) => { return new Result() { res = 1 }; })
                .WithLogic((p, r) =>
                {
                    return new Result() { res = 2 };
                })
                .Run(new Params() { n = 5000 });

        }

        /// <summary>
        /// TODO reference one frame from another to sort of chain the calculation?
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int StrongRecursion(int input)
        {
            Stack<StackFrame> stack = new Stack<StackFrame>();
            stack.Push(new StackFrame() { n = input, result = 0});

            int finalResult = 0;

            while(stack.Count > 0)
            {
                var frame = stack.Pop();

                if(frame.n == 1)
                {
                    finalResult = frame.result + frame.n;
                }
                else
                {
                    var newFrame = new StackFrame() {
                        n = frame.n - 1,
                        result = frame.result + frame.n
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
