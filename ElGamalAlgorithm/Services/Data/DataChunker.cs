using System;
using System.Collections.Generic;
using ElGamal.Model;
using ElGamal.Services.Data.Padding;
using ElGamal.Services.Data.Sources;

namespace ElGamal.Services.Data
{
    public class DataChunker : IDataChunker
    {
        private readonly IPaddingStrategy _paddingStrategy;
        private readonly IPaddingStrategy _PaddingStrategyForFullfilment;

        public DataChunker(IPaddingStrategy paddingStrategy)
        {
            _paddingStrategy = paddingStrategy ?? throw new ArgumentNullException(nameof(paddingStrategy));
            _PaddingStrategyForFullfilment = new PaddingWithZeroesStrategy();
        }

        public BigInteger[] ChunkData(byte[] inputData, int bytesInBlock)
        {
            var paddedData = _paddingStrategy.ApplyPadding(inputData, bytesInBlock);
            int numberOfBlocks = paddedData.Length / bytesInBlock;
            BigInteger[] blocks = new BigInteger[numberOfBlocks];

            for (int i = 0; i < numberOfBlocks; i++)
            {
                byte[] block = new byte[bytesInBlock];
                Array.Copy(paddedData, i * bytesInBlock, block, 0, bytesInBlock);
                blocks[i] = new BigInteger(block);
            }

            return blocks;
        }

        public BigInteger[] BytesToBigIntegers(byte[] encryptedBytes, int blockSize)
        {
            BigInteger[] blocks = new BigInteger[encryptedBytes.Length/blockSize];

            for(int i = 0; i < encryptedBytes.Length / blockSize; i++)
            {
                byte[] block = new byte[blockSize];
                Array.Copy(encryptedBytes, i * blockSize, block, 0, blockSize);
                blocks[i] = new BigInteger(block);
            }

            return blocks;
        }

        public byte[] MergeData(BigInteger[] decryptedValues, int blockSize)
        {
            List<byte> decryptedBytes = new List<byte>();

            foreach (BigInteger decryptedValue in decryptedValues)
            {
                var valueBytes = decryptedValue.getBytes();
                decryptedBytes.AddRange(valueBytes);
            }

            return _paddingStrategy.RemovePadding(decryptedBytes.ToArray());
        }

        public byte[] CiphertextsToBytes(ElGamalCiphertext[] encryptedValues, int bytesInBlock)
        {
            byte[] encryptedByteArray = new byte[(2 * encryptedValues.Length) * bytesInBlock];

            for (int i = 0; i < encryptedValues.Length; i++)
            {
                byte[] paddedNumberC1 = _PaddingStrategyForFullfilment.ApplyPadding(encryptedValues[i].C1.getBytes(), bytesInBlock);
                byte[] paddedNumberC2 = _PaddingStrategyForFullfilment.ApplyPadding(encryptedValues[i].C2.getBytes(), bytesInBlock);
                paddedNumberC1.CopyTo(encryptedByteArray, (2*i) * bytesInBlock);
                paddedNumberC2.CopyTo(encryptedByteArray, (2*i+1) * bytesInBlock);
            }

            return encryptedByteArray;
        }

        public ElGamalCiphertext[] BytesToCipherText(byte[] data, int bytesInBlock)
        {
            ElGamalCiphertext[] encryptedByteArray = new ElGamalCiphertext[ (data.Length/bytesInBlock)/2 ];

            for (int i = 0; i < data.Length; i+=2*bytesInBlock)
            {
                byte[] tmp = new byte[bytesInBlock];
                Array.Copy(data, i, tmp, 0, bytesInBlock);
                BigInteger paddedNumberC1 = new BigInteger(tmp);
                Array.Copy(data, i + bytesInBlock, tmp, 0, bytesInBlock);
                BigInteger paddedNumberC2 = new BigInteger(tmp);
                encryptedByteArray[i/(2*bytesInBlock)] = new ElGamalCiphertext() {C1 = paddedNumberC1, C2 = paddedNumberC2};   
            }

            return encryptedByteArray;
        }
    }
}
