namespace ElGamal.Services.Interfaces
{
    public interface IRandomNumberProvider
    {
        BigInteger GeneratePositiveNumberLessThan(BigInteger limit);
    }
}
