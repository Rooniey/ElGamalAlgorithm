using ElGamal.Model;

namespace ElGamal.Services.Data
{
    public interface IDataChunker
    {
        BigInteger[] ChunkData(byte[] data, int bytesInBlock);
        byte[] MergeData(BigInteger[] encryptedValues, int blockSize);
        byte[] CiphertextsToBytes(ElGamalCiphertext[] encryptedValues, int blockSize);
        ElGamalCiphertext[] BytesToCipherText(byte[] data, int bytesInBlock);
    }
}