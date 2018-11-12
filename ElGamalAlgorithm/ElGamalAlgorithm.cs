using ElGamal.Model;
using ElGamal.Services.Data;
using ElGamal.Services.Data.Padding;
using ElGamal.Services.Interfaces;

namespace ElGamal
{
    public class ElGamalAlgorithm
    {
        private readonly IRandomNumberProvider _randomNumberProvider;
        private readonly IDataChunker _dataChunker = new DataChunker(new SimplePaddingStrategy());

        public ElGamalAlgorithm(IRandomNumberProvider randomNumberProvider)
        {
            _randomNumberProvider = randomNumberProvider;
        }

        public byte[] Encrypt(byte[] dataToEncrypt, PublicKey publicKey)
        {
            int keySizeInBytes = publicKey.P.BitCount() / 8;
            int bytesInBlock = keySizeInBytes - 1;

            BigInteger[] chunkedData = _dataChunker.ChunkData(dataToEncrypt, bytesInBlock);

            ElGamalCiphertext[] ciphertexts = new ElGamalCiphertext[chunkedData.Length];

            for (int i = 0; i < chunkedData.Length; i++)
            {
                ciphertexts[i] = Encrypt(chunkedData[i], publicKey);
            }

            return _dataChunker.CiphertextsToBytes(ciphertexts, keySizeInBytes);
        }

        public byte[] Decrypt(byte[] dataToDecrypt, PrivateKey privateKey)
        {
            int keySizeInBytes = privateKey.P.BitCount() / 8;
            int bytesInBlock = keySizeInBytes - 1;

            ElGamalCiphertext[] ciphertext = _dataChunker.BytesToCipherText(dataToDecrypt, keySizeInBytes);
            BigInteger[] messages = new BigInteger[ciphertext.Length];

            for (int i = 0; i < ciphertext.Length; i++)
            {
                messages[i] = Decrypt(ciphertext[i], privateKey);
            }

            return _dataChunker.MergeData(messages, bytesInBlock);
        }


        private ElGamalCiphertext Encrypt(BigInteger m, PublicKey key)
        {
            BigInteger k = _randomNumberProvider.GeneratePositiveNumberLessThan(key.P - 1);
            BigInteger c1 = key.G.ModPow(k, key.P);
            BigInteger c2 = (key.Y.ModPow(k, key.P) * m) % key.P;
            return new ElGamalCiphertext() {C1 = c1, C2 = c2};
        }

        private BigInteger Decrypt(ElGamalCiphertext ciphertext, PrivateKey key)
        {
            return (ciphertext.C1.ModPow(key.P - key.X - 1, key.P) * ciphertext.C2) % key.P;
        }
    }

}
