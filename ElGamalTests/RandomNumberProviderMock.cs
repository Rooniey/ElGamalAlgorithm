using ElGamal;
using ElGamal.Services;
using ElGamal.Services.Interfaces;

namespace ElGamalTests
{
    internal class RandomNumberProviderMock : IRandomNumberProvider
    {
        private int _index = 0;
        private BigInteger[] _values;

        public RandomNumberProviderMock(BigInteger[] values)
        {
            _values = values;
        }

        public BigInteger GeneratePositiveNumberLessThan(BigInteger limit)
        {
            if (_index >= _values.Length) _index = 0;
            return _values[_index++];
        }
    }
}
