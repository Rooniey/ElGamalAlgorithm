using System;
using ElGamal.Services.Interfaces;

namespace ElGamal.Services
{
    public class RandomNumberProvider : IRandomNumberProvider
    {
        public static readonly Random Random = new Random();

        public BigInteger GeneratePositiveNumberLessThan(BigInteger limit)
        {
            BigInteger randomNumber = new BigInteger();
            randomNumber.GenerateRandomBitsFromZero(limit.BitCount(), Random);
            return (randomNumber % (limit - 1)) + 1;
        }
    }
}
