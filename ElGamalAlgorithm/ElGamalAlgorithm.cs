using ElGamal.Model;
using ElGamal.Services;

namespace ElGamal
{
    public class ElGamalAlgorithm
    {
        private readonly IRandomNumberProvider _randomNumberProvider;

        public ElGamalAlgorithm(IRandomNumberProvider randomNumberProvider)
        {
            _randomNumberProvider = randomNumberProvider;
        }

        // Input assumptions
        // 0 < m < P - 1
        public ElGamalCiphertext Encrypt(BigInteger m, PublicKey key)
        {
            BigInteger k = _randomNumberProvider.GeneratePositiveNumberLessThan(key.P - 1);
            BigInteger c1 = key.G.modPow(k, key.P);
            BigInteger c2 = (key.Y.modPow(k, key.P) * m) % key.P;
            return new ElGamalCiphertext() {C1 = c1, C2 = c2};
        }

        public BigInteger Decrypt(ElGamalCiphertext ciphertext, PrivateKey key)
        {
            return (ciphertext.C1.modPow(key.P - key.X - 1, key.P) * ciphertext.C2) % key.P;
        }
    }

}
