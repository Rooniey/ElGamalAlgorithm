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
            BigInteger p = BigInteger.GeneratePseudoPrime(keyBitCount, 10, RandomNumberProvider.Random);
            BigInteger g = _randomNumberProvider.GeneratePositiveNumberLessThan(p);
            BigInteger x = _randomNumberProvider.GeneratePositiveNumberLessThan(p);

            return new PrivateKey() {P = p, G = g, X = x};
        }

        public PublicKey GeneratePublicKey(PrivateKey privateKey)
        {
            BigInteger y = privateKey.G.ModPow(privateKey.X, privateKey.P);
            return new PublicKey() { P = privateKey.P, G = privateKey.G, Y = y};

        }
    }
}