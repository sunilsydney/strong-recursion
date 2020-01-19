using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StrongRecursion.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            int n = 5000;
            int expected = (n * (n + 1)) / 2;

            int actual = Sum(n);
            Assert.AreEqual(expected, actual);
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
