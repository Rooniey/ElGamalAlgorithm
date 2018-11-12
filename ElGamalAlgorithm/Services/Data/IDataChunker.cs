using System.Collections.Generic;
using ElGamal.Model;

namespace ElGamal.Services.Data
{
    public interface IDataChunker
    {
        BigInteger[] ChunkData(byte[] inputData, int bytesInBlock);
        byte[] MergeData(BigInteger[] encryptedValues, int blockSize);
        BigInteger[] BytesToBigIntegers(byte[] encryptedValues, int blockSize);
        byte[] CiphertextsToBytes(ElGamalCiphertext[] encryptedValues, int blockSize);
        ElGamalCiphertext[] BytesToCipherText(byte[] data, int bytesInBlock);
    }
}