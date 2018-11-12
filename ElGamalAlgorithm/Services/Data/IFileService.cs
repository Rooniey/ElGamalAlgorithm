namespace ElGamal.Services.Data
{
    public interface IFileService
    {
        void SaveToFile(byte[] bytesToSave, string filePath);
    }
}