using System;

namespace ElGamal.Services.Data.Padding
{
    public class PaddingWithZeroesStrategy : IPaddingStrategy
    {
        public byte[] ApplyPadding(byte[] data, int bytesInBlock)
        {
            int diff = data.Length % bytesInBlock;
            int bytesToPadd = diff == 0 ? 0 : bytesInBlock - diff;
            byte[] paddedData = new byte[data.Length + bytesToPadd];
            data.CopyTo(paddedData, bytesToPadd);
            return paddedData;
        }

        public byte[] RemovePadding(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
