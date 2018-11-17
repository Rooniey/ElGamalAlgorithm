using System;
using System.IO;
using ElGamal;
using ElGamal.Model;
using ElGamal.Services;
using ElGamal.Services.Data;
using ElGamal.Services.Data.Padding;
using ElGamal.Services.Interfaces;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElGamalTests
{
    [TestClass]
    public class ElGamalAlgorithmTests
    {
        private CryptoKeyGenerator _keyGenerator;
        private PrivateKey _privateKey;
        private PublicKey _publicKey;
        private DataChunker _dataChunker;
        private IRandomNumberProvider _randomProvider;
        private string _originalFilePath = @"C:\Users\marek\Desktop\pobrane.png";
        private string _encryptedFilePath = @"C:\Users\marek\Desktop\encrypted";
        private string _decryptedFilePath = @"C:\Users\marek\Desktop\decrypted.png";


        [TestInitialize]
        public void SetUp()
        {
            _randomProvider = new RandomNumberProvider();
            _dataChunker = new DataChunker(new SimplePaddingStrategy());
            _keyGenerator = new CryptoKeyGenerator(_randomProvider);
            _privateKey = _keyGenerator.GeneratePrivateKey(1024);
            _publicKey = _keyGenerator.GeneratePublicKey(_privateKey);
        }

        [TestMethod]
        public void AllTests()
        {
            for (int j = 0; j < 1; j++)
            {
                Console.WriteLine($"{j} time start");

                byte[] rawData = File.ReadAllBytes(_originalFilePath);

                int keySizeInBytes = _publicKey.P.BitCount() / 8;
                int bytesInBlock = keySizeInBytes - 1;

                BigInteger[] chunkedData = _dataChunker.ChunkData(rawData, bytesInBlock);

                ElGamalCiphertext[] ciphertexts = new ElGamalCiphertext[chunkedData.Length];

                for (int i = 0; i < chunkedData.Length; i++)
                {
                    ciphertexts[i] = Encrypt(chunkedData[i], _publicKey);
                }


                byte[] originalEncrypted = _dataChunker.CiphertextsToBytes(ciphertexts, keySizeInBytes);

                File.WriteAllBytes(_encryptedFilePath, originalEncrypted);



                byte[] usedEncryptedData = File.ReadAllBytes(_encryptedFilePath);


                usedEncryptedData.Should().BeEquivalentTo(originalEncrypted);


                ElGamalCiphertext[] ciphertext = _dataChunker.BytesToCipherText(usedEncryptedData, keySizeInBytes);

                ciphertexts.Should().BeEquivalentTo(ciphertext);

                BigInteger[] messages = new BigInteger[ciphertext.Length];

                for (int i = 0; i < ciphertext.Length; i++)
                {
                    messages[i] = Decrypt(ciphertext[i], _privateKey);
                }

                chunkedData.Should().BeEquivalentTo(messages);

                byte[] decryptedData = _dataChunker.MergeData(messages, bytesInBlock);


                decryptedData.Should().BeEquivalentTo(rawData);

                File.WriteAllBytes(_decryptedFilePath, decryptedData);

                Console.WriteLine($"{j} time passed");

            }



        }

        private ElGamalCiphertext Encrypt(BigInteger m, PublicKey key)
        {
            BigInteger k = (key.P - 1).genCoPrime(RandomNumberProvider.Random);
            BigInteger c1 = key.G.ModPow(k, key.P);
            BigInteger c2 = (key.Y.ModPow(k, key.P) * m) % key.P;
            return new ElGamalCiphertext() { C1 = c1, C2 = c2 };
        }

        private BigInteger Decrypt(ElGamalCiphertext ciphertext, PrivateKey key)
        {
            return (ciphertext.C1.ModPow(key.P - key.X - 1, key.P) * ciphertext.C2) % key.P;
        }
    }
}
