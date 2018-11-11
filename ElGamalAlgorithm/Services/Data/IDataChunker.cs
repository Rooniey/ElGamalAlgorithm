namespace ElGamal.Services.Data
{
    public interface IDataChunker
    {
        BigInteger[] ChunkData(byte[] inputData, int blockSize);
        byte[] MergeData(BigInteger[] inputData);
    }
}