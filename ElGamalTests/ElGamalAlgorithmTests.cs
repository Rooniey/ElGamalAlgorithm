using ElGamal;
using ElGamal.Model;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElGamalTests
{
    [TestClass]
    public class ElGamalAlgorithmTests
    {
        #region FIELDS        
        private ElGamalAlgorithm _algorithm;
        private readonly BigInteger[] _random = new[]{ new BigInteger(36) };
        private readonly PublicKey _publicKey = new PublicKey()
        {
            P = new BigInteger(283),
            G = new BigInteger(60),
            Y = new BigInteger(216)
        };
        private readonly PrivateKey _privateKey = new PrivateKey()
        {
            P = new BigInteger(283),
            X = new BigInteger(7)
            
        };
        private readonly BigInteger _message = new BigInteger(101);
        private readonly ElGamalCiphertext _encryptedMessage = new ElGamalCiphertext()
            {C1 = new BigInteger(78), C2 = new BigInteger(218)};
        #endregion

        [TestInitialize]
        public void SetUp()
        {
            _algorithm = new ElGamalAlgorithm(new RandomNumberProviderMock(_random));
        }

        [TestMethod]
        public void When_Encrypt_Should_ReturnCorrectCiphertext()
        {
            ElGamalCiphertext ciphertext = _algorithm.Encrypt(_message, _publicKey);
            ciphertext.Should().BeEquivalentTo(_encryptedMessage);
        }

        [TestMethod]
        public void When_Decrypt_Should_ReturnCorrectMessage()
        {
            BigInteger message = _algorithm.Decrypt(_encryptedMessage, _privateKey);
            message.Should().BeEquivalentTo(_message);
        }
    }
}
