using System;
using ElGamal.Services.Interfaces;

namespace ElGamal.Services
{
    public class RandomNumberProvider : IRandomNumberProvider
    {
        public static readonly Random Random = new Random();

        public BigInteger GeneratePositiveNumberLessThan(BigInteger limit)
        {
            //TODO generates at least one bit less number  
            BigInteger randomNumber = new BigInteger();
            randomNumber.GenerateRandomBits(limit.BitCount() - 1, Random);
            return randomNumber;
        }
    }
}
