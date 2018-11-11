namespace ElGamal.Services.Data
{
    public interface IDataChunker
    {
        BigInteger[] ChunkData(byte[] inputData, int blockSize);
        BigInteger[] BytesToBigIntegers(byte[] encryptedValues, int blockSize);
        byte[] MergeData(BigInteger[] encryptedValues, int blockSize);
    }
}