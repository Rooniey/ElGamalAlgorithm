using ElGamal;
using ElGamal.Services.Data;
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
            _dataChunker = new DataChunker();
        }

        [TestMethod]
        public void When_ChunkDataCalledWith_ByteLengthNotMultipleOfBlockSize_Should_ReturnCorrectData()
        {

            byte[] sampleInput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
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

            byte[] sampleInput = new byte[] { 0xFF, 0xFF, 0xFF };
            BigInteger[] result = _dataChunker.ChunkData(sampleInput, _blockSize);
            result.Should().BeEquivalentTo(new BigInteger[]
            {
                new BigInteger(new byte[] {0xFF, 0xFF, 0xFF}),
                new BigInteger(new byte[] {0x03, 0x03, 0x03})
            });
        }

    }
}