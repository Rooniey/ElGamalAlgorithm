//using System;
//using System.Collections.Generic;
//using ElGamal;
//using ElGamal.Model;
//using ElGamal.Services;
//using ElGamal.Services.Data;
//using ElGamal.Services.Data.Padding;
//using FluentAssertions;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//
//namespace ElGamalTests
//{
//    [TestClass]
//    public class ElGamalAlgorithmTests
//    {
//        #region FIELDS        
//        private ElGamalAlgorithm _algorithm;
//        private readonly BigInteger[] _random = new[]{ new BigInteger(36) };
//        private readonly PublicKey _publicKey = new PublicKey()
//        {
//            P = new BigInteger(283),
//            G = new BigInteger(60),
//            Y = new BigInteger(216)
//        };
//        private readonly PrivateKey _privateKey = new PrivateKey()
//        {
//            P = new BigInteger(283),
//            X = new BigInteger(7)
//            
//        };
//        private readonly BigInteger _message = new BigInteger(101);
//        private readonly ElGamalCiphertext _encryptedMessage = new ElGamalCiphertext()
//            {C1 = new BigInteger(78), C2 = new BigInteger(218)};
//        #endregion
//
//        [TestInitialize]
//        public void SetUp()
//        {
//            _algorithm = new ElGamalAlgorithm(new RandomNumberProviderMock(_random));
//        }
//
//        [TestMethod]
//        public void When_Encrypt_Should_ReturnCorrectCiphertext()
//        {
//            ElGamalCiphertext ciphertext = _algorithm.Encrypt(_message, _publicKey);
//            ciphertext.Should().BeEquivalentTo(_encryptedMessage);
//        }
//
//        [TestMethod]
//        public void When_Decrypt_Should_ReturnCorrectMessage()
//        {
//            BigInteger message = _algorithm.Decrypt(_encryptedMessage, _privateKey);
//            message.Should().BeEquivalentTo(_message);
//        }
//
//        [TestMethod]
//        public void When_EncryptCalledWithPaddedValues_Should_ReturnPaddedData()
//        {
//            DataChunker _chunker = new DataChunker(new SimplePaddingStrategy());
//            byte[] inputData = new byte[] {0xff, 0xac, 0xff, 0xac};
//            CryptoKeyGenerator cryptoKey = new CryptoKeyGenerator(new RandomNumberProvider());
//            PrivateKey privateKey = cryptoKey.GeneratePrivateKey(64);
//            PublicKey publicKey = cryptoKey.GeneratePublicKey(privateKey);
//
//            var chunkedMessage = _chunker.ChunkData(inputData, 3);
//            List<ElGamalCiphertext> encryptedMessage = new List<ElGamalCiphertext>();
//
//            foreach (var bigInteger in chunkedMessage)
//            {
//                var encrypted = _algorithm.Encrypt(bigInteger, publicKey);
//                encryptedMessage.Add(encrypted);
//                
//            }
//
//            List<BigInteger> decryptedMessage = new List<BigInteger>();
//            foreach (var bigInteger2 in encryptedMessage)
//            {
//                var decrypted = _algorithm.Decrypt(bigInteger2, privateKey);
//                decryptedMessage.Add(decrypted);
//            }
//
//            var decryptedBytes = _chunker.MergeData(decryptedMessage.ToArray(), 3);
//            Console.WriteLine();
//            decryptedBytes.Should().BeEquivalentTo(new byte[] {0xff, 0xac, 0xff, 0xac});
//        }
//    }
//}
