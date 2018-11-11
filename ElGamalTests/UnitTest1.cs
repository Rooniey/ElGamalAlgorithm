using System;
using ElGamal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElGamalTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            BigInteger integer = new BigInteger(new byte[] { 0xff, 0x00 ,0xa0, 0x00 });
            var bytes = integer.getBytes();
            Console.WriteLine(".");
        }
    }
}
