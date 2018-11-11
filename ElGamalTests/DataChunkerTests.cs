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

        [TestMethod]
        public void When_BytesToBigIntegersCalledWith_ByteLengthMultipleOfBlockSize_Should_ReturnCorrectData()
        {

            byte[] sampleInput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x02 };
            BigInteger[] result = _dataChunker.BytesToBigIntegers(sampleInput, _blockSize);
            result.Should().BeEquivalentTo(new BigInteger[]
            {
                new BigInteger(new byte[] {0xFF, 0xFF, 0xFF}),
                new BigInteger(new byte[] {0xFF, 0x02, 0x02})
            });
        }

        [TestMethod]
        public void When_MergeDataCalledWith_MultipleBigIntegers_Should_ReturnCorrectData()
        {

            BigInteger[] sampleInput = new BigInteger[]{ new BigInteger(new byte[]{ 0xff, 0xac, 0xff }), new BigInteger(new byte[] { 0xff, 0xac, 0x01 }) };
            byte[] result = _dataChunker.MergeData(sampleInput, _blockSize);
            result.Should().BeEquivalentTo(new byte[]
            {
                0xff, 0xac, 0xff, 0xff, 0xac
            });
        }
    }
}