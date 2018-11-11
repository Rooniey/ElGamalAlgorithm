using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.Services.Data
{
    public class DataChunker : IDataChunker
    {
        public BigInteger[] ChunkData(byte[] inputData, int blockSize)
        {
            var paddedData = AddPadding(inputData, blockSize);
            BigInteger[] blocks = new BigInteger[paddedData.Length / blockSize];

            for (int i = 0; i < paddedData.Length / blockSize; i++)
            {
                byte[] block = new byte[blockSize];
                Array.Copy(paddedData, i * blockSize, block, 0, blockSize);
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

        private byte[] AddPadding(byte[] message, int blockSize)
        {
            if (blockSize > 255)
            {
                throw new ArgumentException($"Block length has to be shorter than 255, but was {blockSize}");
            }

            if (message.Length % blockSize == 0)
            {
                byte[] paddedMessage = new byte[message.Length + blockSize];
                message.CopyTo(paddedMessage, 0);
                for (int i = message.Length; i < message.Length + blockSize; i++)
                {
                    paddedMessage[i] = (byte)blockSize;
                }
                return paddedMessage;
            }
            else
            {
                int remainderBytes = blockSize - (message.Length % blockSize);
                byte[] paddedMessage = new byte[message.Length + remainderBytes];

                message.CopyTo(paddedMessage, 0); //copy message content

                for (int i = message.Length; i < message.Length + remainderBytes; i++)
                {
                    paddedMessage[i] = (byte)remainderBytes;
                }

                return paddedMessage;
            }
        }

        public byte[] MergeData(BigInteger[] decryptedValues, int blockSize)
        {
            List<byte> decryptedBytes = new List<byte>();

            foreach (BigInteger decryptedValue in decryptedValues)
            {
                var valueBytes = decryptedValue.getBytes();
                decryptedBytes.AddRange(valueBytes);
            }

            return RemovePadding(decryptedBytes.ToArray(), blockSize);
        }

        private byte[] RemovePadding(byte[] message, int blockSize)
        {
            //find last byte
            byte lastByte = message.Last();

            if (lastByte > message.Length)
            {
                throw new ArgumentException("Message is shorter than padding length taken from last byte.");
            }

            //remove correct number of bytes (just dont copy them)
            byte[] messageWithoutPadding = new byte[message.Length - lastByte];

            Array.Copy(message, 0, messageWithoutPadding, 0, messageWithoutPadding.Length);

            return messageWithoutPadding;
        }
    }
}
