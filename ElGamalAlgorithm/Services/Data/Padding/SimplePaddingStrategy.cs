using System;
using System.Linq;

namespace ElGamal.Services.Data.Padding
{
    public class SimplePaddingStrategy : IPaddingStrategy
    {
        public byte[] ApplyPadding(byte[] data, int bytesInBlock)
        {
            if (bytesInBlock > 255)
            {
                throw new ArgumentException($"Block length has to be shorter than 255, but was {bytesInBlock}");
            }

            int bytesToPad;
            if (data.Length % bytesInBlock == 0)
                bytesToPad = bytesInBlock;
            else
                bytesToPad = bytesInBlock - (data.Length % bytesInBlock);

            byte[] paddedData = new byte[data.Length + bytesToPad];
            data.CopyTo(paddedData, 0);
            for (int i = data.Length; i < data.Length + bytesToPad; i++)
            {
                paddedData[i] = (byte)bytesToPad;
            }

            return paddedData;
        }

        public byte[] RemovePadding(byte[] message)
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
