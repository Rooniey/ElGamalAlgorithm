using ElGamal;
using ElGamal.Model;
using ElGamal.Services.Data;
using ElGamal.Services.Data.Padding;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElGamalTests
{
    [TestClass]
    public class DataChunkerTests
    {
        private DataChunker _dataChunker;
        private readonly int _blockSize = 3;

        [TestInitialize]
        public void SetUp()
        {
            _dataChunker = new DataChunker(
                new SimplePaddingStrategy()
            );
        }

        [TestMethod]
        public void When_ChunkDataCalledWith_ByteLengthNotMultipleOfBlockSize_Should_ReturnCorrectData()
        {
            byte[] sampleInput = new byte[] {0xFF, 0xFF, 0xFF, 0xFF};
            BigInteger[] result = _dataChunker.ChunkData(sampleInput, _blockSize);
            result.Should().BeEquivalentTo(new BigInteger[]
            {
                new BigInteger(new byte[] {0xFF, 0xFF, 0xFF}),
                new BigInteger(new byte[] {0xFF, 0x02, 0x02})
            });
        }

        [TestMethod]
        public void When_ChunkDataCalledWith_ByteLengthMultipleOfBlockSize_Should_ReturnCorrectData()
        {
            byte[] sampleInput = new byte[] {0xFF, 0xFF, 0xFF};
            BigInteger[] result = _dataChunker.ChunkData(sampleInput, _blockSize);
            result.Should().BeEquivalentTo(new BigInteger[]
            {
                new BigInteger(new byte[] {0xFF, 0xFF, 0xFF}),
                new BigInteger(new byte[] {0x03, 0x03, 0x03})
            });
        }

        [TestMethod]
        public void When_CiphertextToBytesCalled_Should_ReturnCorrectData()
        {
            ElGamalCiphertext[] ciphertexts = new[]
            {
                new ElGamalCiphertext()
                {
                    C1 = new BigInteger(new byte[] {0xFF, 0xFF}),
                    C2 = new BigInteger(new byte[] {0xFF, 0xFF, 0xFF})
                },
                new ElGamalCiphertext()
                {
                    C1 = new BigInteger(new byte[] {0xAC, 0xFF}),
                    C2 = new BigInteger(new byte[] {0xFF, 0xDF, 0xFF})
                }
            };
            int bytesInBlock = 3;

            byte[] result = _dataChunker.CiphertextsToBytes(ciphertexts, bytesInBlock);
            result.Should().BeEquivalentTo(new byte[] {0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0xAC, 0xFF, 0xFF, 0xDF, 0xFF});
        }

        [TestMethod]
        public void When_BytesToBigIntegersCalledWith_ByteLengthMultipleOfBlockSize_Should_ReturnCorrectData()
        {
            byte[] sampleInput = new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x02};
            BigInteger[] result = _dataChunker.ChunkData(sampleInput, _blockSize);
            result.Should().BeEquivalentTo(new BigInteger[]
            {
                new BigInteger(new byte[] {0xFF, 0xFF, 0xFF}),
                new BigInteger(new byte[] {0xFF, 0x02, 0x02}),
                new BigInteger(new byte[] {0x03, 0x03, 0x03}), 
            });
        }

        [TestMethod]
        public void When_MergeDataCalledWith_MultipleBigIntegers_Should_ReturnCorrectData()
        {
            BigInteger[] sampleInput = new BigInteger[]
                {new BigInteger(new byte[] {0xff, 0xac, 0xff}), new BigInteger(new byte[] {0xff, 0xac, 0x01})};
            byte[] result = _dataChunker.MergeData(sampleInput, _blockSize);
            result.Should().BeEquivalentTo(new byte[]
            {
                0xff, 0xac, 0xff, 0xff, 0xac
            });
        }
    }
}