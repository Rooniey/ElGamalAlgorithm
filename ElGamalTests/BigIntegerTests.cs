﻿using System;
using ElGamal;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ElGamalTests
{
    [TestClass]
    public class BigIntegerTests
    {
        [TestMethod]
        public void GetBytesTest()
        {
            var firstBytes = new byte[] {0xFF, 0x00, 0xFF, 0x00};

            var bigint = new BigInteger(firstBytes);
            var secondBytes = bigint.GetAllBytes();
            var secondInt = new BigInteger(secondBytes);
            firstBytes.Should().BeEquivalentTo(secondBytes);
            bigint.Should().BeEquivalentTo(secondInt);
        }
    }
}
