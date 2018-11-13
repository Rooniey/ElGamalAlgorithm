using System;
using ElGamal.Model;
using ElGamal.Services.Data.Padding;

namespace ElGamal.Services.Data
{
    public class DataChunker : IDataChunker
    {
        private readonly IPaddingStrategy _simplePaddingStrategy;

        public DataChunker(IPaddingStrategy simplePaddingStrategy)
        {
            _simplePaddingStrategy = simplePaddingStrategy;
        }

        public BigInteger[] ChunkData(byte[] data, int bytesInBlock)
        {
            data = _simplePaddingStrategy.ApplyPadding(data, bytesInBlock);
            int numberOfBlocks = data.Length / bytesInBlock;
            BigInteger[] blocks = new BigInteger[numberOfBlocks];

            for (int i = 0; i < numberOfBlocks; i++)
            {
                byte[] block = new byte[bytesInBlock];
                Array.Copy(data, i * bytesInBlock, block, 0, bytesInBlock);
                blocks[i] = new BigInteger(block);
            }

            return blocks;
        }

        public byte[] CiphertextsToBytes(ElGamalCiphertext[] encryptedValues, int bytesInBlock)
        {
            byte[] encryptedByteArray = new byte[(2 * encryptedValues.Length) * bytesInBlock];

            for (int i = 0; i < encryptedValues.Length; i++)
            {
                byte[] paddedC1 = AddPadding(encryptedValues[i].C1.GetAllBytes(), bytesInBlock);
                byte[] paddedC2 = AddPadding(encryptedValues[i].C2.GetAllBytes(), bytesInBlock);
                paddedC1.CopyTo(encryptedByteArray, (2 * i) * bytesInBlock);
                paddedC2.CopyTo(encryptedByteArray, (2 * i + 1) * bytesInBlock);
            }

            return encryptedByteArray;
        }

        public ElGamalCiphertext[] BytesToCipherText(byte[] data, int bytesInBlock)
        {
            ElGamalCiphertext[] encryptedByteArray = new ElGamalCiphertext[(data.Length / bytesInBlock) / 2];

            for (int i = 0; i < data.Length; i += 2 * bytesInBlock)
            {
                byte[] tmp1 = new byte[bytesInBlock];
                Array.Copy(data, i, tmp1, 0, bytesInBlock);
                BigInteger paddedNumberC1 = new BigInteger(tmp1);

                byte[] tmp2 = new byte[bytesInBlock];
                Array.Copy(data, i + bytesInBlock, tmp2, 0, bytesInBlock);
                BigInteger paddedNumberC2 = new BigInteger(tmp2);

                encryptedByteArray[i / (2 * bytesInBlock)] = new ElGamalCiphertext() { C1 = paddedNumberC1, C2 = paddedNumberC2 };
            }

            return encryptedByteArray;
        }

        public byte[] MergeData(BigInteger[] messages, int bytesInBlock)
        {
            byte[] result = new byte[messages.Length * bytesInBlock];

            for (int i = 0; i < messages.Length; i++)
            {
                byte[] padded =
                    AddPadding(messages[i].GetAllBytes(), bytesInBlock);
                padded.CopyTo(result, i * bytesInBlock);
            }

            result = _simplePaddingStrategy.RemovePadding(result);
            return result;
        }

        private byte[] AddPadding(byte[] data, int bytesInBlock)
        {
            int diff = data.Length % bytesInBlock;
            int bytesToPadd = diff == 0 ? 0 : bytesInBlock - diff;
            byte[] paddedData = new byte[data.Length + bytesToPadd];
            data.CopyTo(paddedData, bytesToPadd);
            return paddedData;
        }
    }
}
