using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElGamal.Helpers
{
    public static class PaddingExtensionMethod
    {
        public static byte[] AddPadding(this byte[] unpaddedBytes, int keyLength)
        {
            var blockLength = keyLength - 1;


            return unpaddedBytes;
        }
    }
}
