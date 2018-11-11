using System.ComponentModel;

namespace ElGamal.Helpers
{
    public static class TextUtility
    {
        public enum Encoding
        {
            [Description("ASCII")]
            Ascii,

            [Description("UTF-8")]
            Utf8,

            [Description("Unicode")]
            Unicode
        }

        public static byte[] ToByteArray(this string text, Encoding encoding)
        {
            switch (encoding)
            {
                case Encoding.Ascii:
                    return System.Text.Encoding.ASCII.GetBytes(text);

                case Encoding.Unicode:
                    return System.Text.Encoding.Unicode.GetBytes(text);

                case Encoding.Utf8:
                    return System.Text.Encoding.UTF8.GetBytes(text);

                default:
                    return null;
            }
        }

        public static string ToText(this byte[] data, Encoding encoding)
        {
            switch (encoding)
            {
                case Encoding.Ascii:
                    return System.Text.Encoding.ASCII.GetString(data);

                case Encoding.Unicode:
                    return System.Text.Encoding.Unicode.GetString(data);

                case Encoding.Utf8:
                    return System.Text.Encoding.UTF8.GetString(data);

                default:
                    return null;
            }
        }
    }
}
