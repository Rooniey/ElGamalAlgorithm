using System.Collections.Generic;
using ElGamal.Model;

namespace ElGamal.Services.Data
{
    public interface IDataChunker
    {
        BigInteger[] ChunkData(byte[] inputData, int blockSize);
        BigInteger[] BytesToBigIntegers(byte[] encryptedValues, int blockSize);
        byte[] MergeData(BigInteger[] encryptedValues, int blockSize);

        byte[] CiphertextsToBytes(List<ElGamalCiphertext> encryptedValues, int blockSize);
    }
}