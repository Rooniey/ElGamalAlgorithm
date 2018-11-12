using ElGamal.Model;
using ElGamal.Services.Interfaces;

namespace ElGamal.Services
{
    public class CryptoKeyGenerator : ICryptoKeyGenerator
    {
        private IRandomNumberProvider _randomNumberProvider;

        public CryptoKeyGenerator(IRandomNumberProvider randomNumberProvider)
        {
            _randomNumberProvider = randomNumberProvider;
        }

        public PrivateKey GeneratePrivateKey(int keyBitCount)
        {
            BigInteger p = BigInteger.GeneratePseudoPrime(keyBitCount, 20, RandomNumberProvider.Random);
            BigInteger g = FindPrimitiveRoot(p);
            BigInteger x = _randomNumberProvider.GeneratePositiveNumberLessThan(p);

            return new PrivateKey() {P = p, G = g, X = x};
        }

        public PublicKey GeneratePublicKey(PrivateKey privateKey)
        {
            BigInteger y = privateKey.G.ModPow(privateKey.X, privateKey.P);
            return new PublicKey() { P = privateKey.P, G = privateKey.G, Y = y};

        }

        private BigInteger FindPrimitiveRoot(BigInteger p)
        {
            if (p == 2) return new BigInteger(1);


            // the prime divisors of p-1 are 2 and (p-1)/2 because
            // p = 2x + 1 where x is a prime
            BigInteger p1 = new BigInteger(2);

            BigInteger p2 = (p - 1); // p1

            // test random g's until one is found that is a primitive root mod p
            while (true)
            {
                BigInteger g = _randomNumberProvider.GeneratePositiveNumberLessThan(p - 1);
                // g is a primitive root if for all prime factors of p-1, p[i]
                // g^((p-1)/p[i]) (mod p) is not congruent to 1
                if (!(g.ModPow((p - 1)/p1, p) == 1))
                {
                    if (!(g.ModPow((p - 1) / p2, p) == 1))
                    {
                        return g;
                    }
                }

            }
        }
    }
}