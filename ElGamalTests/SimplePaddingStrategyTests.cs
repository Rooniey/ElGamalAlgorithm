using System;
using ElGamal.Services.Data;
using ElGamal.Services.Data.Padding;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElGamalTests
{
    [TestClass]
    public class SimplePaddingStrategyTests
    {
        private SimplePaddingStrategy _paddingStrategy;

        [TestInitialize]
        public void SetUp()
        {
            _paddingStrategy = new SimplePaddingStrategy();
        }

        [TestMethod]
        public void When_ApplyPaddingCalled_WithBytesLengthNotMultipleOfBlock_Should_ReturnCorrectResult()
        {
            byte[] simpleInput = new byte[] {0xFF, 0xFF, 0xFF, 0xFF};
            int bytesInBlock = 3;

            byte[] result = _paddingStrategy.ApplyPadding(simpleInput, bytesInBlock);

            result.Should().BeEquivalentTo(new byte[] {0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x02});
        }

        [TestMethod]
        public void When_ApplyPaddingCalled_WithBytesLengthMultipleOfBlock_Should_ReturnCorrectResult()
        {
            byte[] simpleInput = new byte[] { 0xFF, 0xFF, 0xFF };
            int bytesInBlock = 3;

            byte[] result = _paddingStrategy.ApplyPadding(simpleInput, bytesInBlock);

            result.Should().BeEquivalentTo(new byte[] { 0xFF, 0xFF, 0xFF, 0x03, 0x03, 0x03 });
        }

        [TestMethod]
        public void When_RemovePaddingCalled_WithBytesLengthNotMultipleOfBlock_Should_ReturnCorrectResult()
        {
            byte[] simpleInput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0x02, 0x02 };

            byte[] result = _paddingStrategy.RemovePadding(simpleInput);

            result.Should().BeEquivalentTo(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF });
        }

        [TestMethod]
        public void When_RemovePaddingCalled_WithBytesLengthMultipleOfBlock_Should_ReturnCorrectResult()
        {
            byte[] simpleInput = new byte[] { 0xFF, 0xFF, 0xFF, 0x03, 0x03, 0x03 };

            byte[] result = _paddingStrategy.RemovePadding(simpleInput);

            result.Should().BeEquivalentTo(new byte[] { 0xFF, 0xFF, 0xFF });
        }

    }
}
