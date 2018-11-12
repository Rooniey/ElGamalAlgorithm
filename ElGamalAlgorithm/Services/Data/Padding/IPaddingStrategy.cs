namespace ElGamal.Services.Data.Padding
{
    public interface IPaddingStrategy
    {
        byte[] ApplyPadding(byte[] data, int bytesInBlock);
        byte[] RemovePadding(byte[] data);
    }
}
