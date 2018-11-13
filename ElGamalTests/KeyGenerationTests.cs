using System;
using ElGamal.Model;
using ElGamal.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElGamalTests
{
    [TestClass]
    public class KeyGenerationTests
    {
        private CryptoKeyGenerator _keyGenerator;

        [TestInitialize]
        public void SetUp()
        {
            _keyGenerator = new CryptoKeyGenerator(new RandomNumberProvider());
        }

        [TestMethod]
        public void When_GeneratePrivateKey_Should_ReturnCorrectKey()
        {

            PrivateKey key = _keyGenerator.GeneratePrivateKey(1024);
            Console.WriteLine("asdasdas");
        }
    }
}
