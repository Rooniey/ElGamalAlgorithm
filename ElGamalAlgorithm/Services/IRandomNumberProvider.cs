namespace ElGamal.Services
{
    public interface IRandomNumberProvider
    {
        BigInteger GeneratePositiveNumberLessThan(BigInteger limit);
    }
}
