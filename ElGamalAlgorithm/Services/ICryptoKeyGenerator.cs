using ElGamal.Model;

namespace ElGamal.Services
{
    public interface ICryptoKeyGenerator
    {
        PrivateKey GeneratePrivateKey(int keyBitCount);
        PublicKey GeneratePublicKey(PrivateKey privateKey);
    }
}
