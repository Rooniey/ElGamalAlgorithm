using ElGamal.Model;

namespace ElGamal.Services.Interfaces
{
    public interface ICryptoKeyGenerator
    {
        PrivateKey GeneratePrivateKey(int keyBitCount);
        PublicKey GeneratePublicKey(PrivateKey privateKey);
    }
}
