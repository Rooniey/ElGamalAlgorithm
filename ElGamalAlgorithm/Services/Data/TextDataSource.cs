using System;
using ElGamal.Helpers;

namespace ElGamal.Services.Data
{
    public class TextDataSource : IDataSource
    {

        public string Text { get; }
        public TextUtility.Encoding TextEncoding { get; }

        public TextDataSource(string textSource, TextUtility.Encoding textEncoding)
        {
            if (string.IsNullOrEmpty(textSource))
            {
                throw new ArgumentException("TexDataSource constructor: given string is null or empty and can not be used as data source");
            }
            Text = textSource;
            TextEncoding = textEncoding;
        }

        public byte[] GetData()
        {
            return Text.ToByteArray(TextEncoding);
        }
    }
}
