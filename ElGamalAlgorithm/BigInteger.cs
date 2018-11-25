using System;

namespace ElGamal
{
    public class BigInteger
    {
        public const int MaxLength = 131;

        public static readonly int[] PrimesBelow1000 =
        {
            2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97,
            101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199,
            211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293,
            307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397,
            401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499,
            503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599,
            601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691,
            701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797,
            809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887,
            907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997
        };

        private uint[] _data; 

        public int DataLength { get; set; }

        #region CONSTRUCTORS
        public BigInteger()
        {
            _data = new uint[MaxLength];
            DataLength = 1;
        }

        public BigInteger(long value)
        {
            _data = new uint[MaxLength];
            long tempVal = value;

            DataLength = 0;
            while (value != 0 && DataLength < MaxLength)
            {
                _data[DataLength] = (uint) (value & 0xFFFFFFFF);
                value >>= 32;
                DataLength++;
            }

            if (tempVal > 0) 
            {
                if (value != 0 || (_data[MaxLength - 1] & 0x80000000) != 0)
                    throw (new ArithmeticException("Positive overflow in constructor."));
            }
            else if (tempVal < 0) 
            {
                if (value != -1 || (_data[DataLength - 1] & 0x80000000) == 0)
                    throw (new ArithmeticException("Negative underflow in constructor."));
            }

            if (DataLength == 0)
                DataLength = 1;
        }

        public BigInteger(ulong value)
        {
            _data = new uint[MaxLength];

            DataLength = 0;
            while (value != 0 && DataLength < MaxLength)
            {
                _data[DataLength] = (uint) (value & 0xFFFFFFFF);
                value >>= 32;
                DataLength++;
            }

            if (value != 0 || (_data[MaxLength - 1] & 0x80000000) != 0)
                throw (new ArithmeticException("Positive overflow in constructor."));

            if (DataLength == 0)
                DataLength = 1;
        }

        public BigInteger(BigInteger bi)
        {
            _data = new uint[MaxLength];

            DataLength = bi.DataLength;

            bi._data.CopyTo(_data, 0);
        }
        
        public BigInteger(byte[] inData)
        {
            DataLength = inData.Length >> 2;

            int leftOver = inData.Length & 0x3;
            if (leftOver != 0) 
                DataLength++;

            if (DataLength > MaxLength)
                throw (new ArithmeticException("Byte overflow in constructor."));

            _data = new uint[MaxLength];

            for (int i = inData.Length - 1, j = 0; i >= 3; i -= 4, j++)
            {
                _data[j] = (uint) ((inData[i - 3] << 24) + (inData[i - 2] << 16) +
                                  (inData[i - 1] << 8) + inData[i]);
            }

            if (leftOver == 1)
                _data[DataLength - 1] = (uint) inData[0];
            else if (leftOver == 2)
                _data[DataLength - 1] = (uint) ((inData[0] << 8) + inData[1]);
            else if (leftOver == 3)
                _data[DataLength - 1] = (uint) ((inData[0] << 16) + (inData[1] << 8) + inData[2]);


            while (DataLength > 1 && _data[DataLength - 1] == 0)
                DataLength--;
        }

        public BigInteger(uint[] inData)
        {
            DataLength = inData.Length;

            if (DataLength > MaxLength)
                throw (new ArithmeticException("Byte overflow in constructor."));

            _data = new uint[MaxLength];

            for (int i = DataLength - 1, j = 0; i >= 0; i--, j++)
                _data[j] = inData[i];

            while (DataLength > 1 && _data[DataLength - 1] == 0)
                DataLength--;
        }
        #endregion

        #region CASTING OPERATORS 
        public static implicit operator BigInteger(long value)
        {
            return (new BigInteger(value));
        }

        public static implicit operator BigInteger(ulong value)
        {
            return (new BigInteger(value));
        }

        public static implicit operator BigInteger(int value)
        {
            return (new BigInteger((long) value));
        }

        public static implicit operator BigInteger(uint value)
        {
            return (new BigInteger((ulong) value));
        }
        #endregion

        #region COMPARISON FUNCTIONS AND OPERATORS
        public static bool operator ==(BigInteger bi1, BigInteger bi2)
        {
            return bi1.Equals(bi2);
        }


        public static bool operator !=(BigInteger bi1, BigInteger bi2)
        {
            return !(bi1.Equals(bi2));
        }

        public override bool Equals(object o)
        {
            BigInteger bi = (BigInteger)o;

            if (this.DataLength != bi.DataLength)
                return false;

            for (int i = 0; i < this.DataLength; i++)
            {
                if (this._data[i] != bi._data[i])
                    return false;
            }

            return true;
        }
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
        public static bool operator >(BigInteger bi1, BigInteger bi2)
        {
            int pos = MaxLength - 1;


            if ((bi1._data[pos] & 0x80000000) != 0 && (bi2._data[pos] & 0x80000000) == 0)
                return false;


            else if ((bi1._data[pos] & 0x80000000) == 0 && (bi2._data[pos] & 0x80000000) != 0)
                return true;


            int len = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength;
            for (pos = len - 1; pos >= 0 && bi1._data[pos] == bi2._data[pos]; pos--) ;

            if (pos >= 0)
            {
                if (bi1._data[pos] > bi2._data[pos])
                    return true;
                return false;
            }

            return false;
        }

        public static bool operator <(BigInteger bi1, BigInteger bi2)
        {
            int pos = MaxLength - 1;


            if ((bi1._data[pos] & 0x80000000) != 0 && (bi2._data[pos] & 0x80000000) == 0)
                return true;


            else if ((bi1._data[pos] & 0x80000000) == 0 && (bi2._data[pos] & 0x80000000) != 0)
                return false;


            int len = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength;
            for (pos = len - 1; pos >= 0 && bi1._data[pos] == bi2._data[pos]; pos--) ;

            if (pos >= 0)
            {
                if (bi1._data[pos] < bi2._data[pos])
                    return true;
                return false;
            }

            return false;
        }


        public static bool operator >=(BigInteger bi1, BigInteger bi2)
        {
            return (bi1 == bi2 || bi1 > bi2);
        }

        public static bool operator <=(BigInteger bi1, BigInteger bi2)
        {
            return (bi1 == bi2 || bi1 < bi2);
        }
        #endregion

        #region ARITHMETICAL FUNCTIONS AND OPERATORS
        public static BigInteger operator +(BigInteger bi1, BigInteger bi2)
        {
            BigInteger result = new BigInteger();

            result.DataLength = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength;

            long carry = 0;
            for (int i = 0; i < result.DataLength; i++)
            {
                long sum = (long) bi1._data[i] + (long) bi2._data[i] + carry;
                carry = sum >> 32;
                result._data[i] = (uint) (sum & 0xFFFFFFFF);
            }

            if (carry != 0 && result.DataLength < MaxLength)
            {
                result._data[result.DataLength] = (uint) (carry);
                result.DataLength++;
            }

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;


            
            int lastPos = MaxLength - 1;
            if ((bi1._data[lastPos] & 0x80000000) == (bi2._data[lastPos] & 0x80000000) &&
                (result._data[lastPos] & 0x80000000) != (bi1._data[lastPos] & 0x80000000))
            {
                throw (new ArithmeticException());
            }

            return result;
        }

        public static BigInteger operator -(BigInteger bi1, BigInteger bi2)
        {
            BigInteger result = new BigInteger
            {
                DataLength = (bi1.DataLength > bi2.DataLength) ? bi1.DataLength : bi2.DataLength
            };

            long carryIn = 0;
            for (int i = 0; i < result.DataLength; i++)
            {
                long diff;

                diff = (long) bi1._data[i] - (long) bi2._data[i] - carryIn;
                result._data[i] = (uint) (diff & 0xFFFFFFFF);

                if (diff < 0)
                    carryIn = 1;
                else
                    carryIn = 0;
            }

            
            if (carryIn != 0)
            {
                for (int i = result.DataLength; i < MaxLength; i++)
                    result._data[i] = 0xFFFFFFFF;
                result.DataLength = MaxLength;
            }
            
            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;

            int lastPos = MaxLength - 1;
            if ((bi1._data[lastPos] & 0x80000000) != (bi2._data[lastPos] & 0x80000000) &&
                (result._data[lastPos] & 0x80000000) != (bi1._data[lastPos] & 0x80000000))
            {
                throw (new ArithmeticException());
            }

            return result;
        }

        public static BigInteger operator *(BigInteger bi1, BigInteger bi2)
        {
            int lastPos = MaxLength - 1;
            bool bi1Neg = false, bi2Neg = false;

            try
            {
                if ((bi1._data[lastPos] & 0x80000000) != 0) 
                {
                    bi1Neg = true;
                    bi1 = -bi1;
                }

                if ((bi2._data[lastPos] & 0x80000000) != 0) 
                {
                    bi2Neg = true;
                    bi2 = -bi2;
                }
            }
            catch (Exception)
            {
                // ignored
            }

            BigInteger result = new BigInteger();

            try
            {
                for (int i = 0; i < bi1.DataLength; i++)
                {
                    if (bi1._data[i] == 0) continue;

                    ulong mcarry = 0;
                    for (int j = 0, k = i; j < bi2.DataLength; j++, k++)
                    {
                        
                        ulong val = ((ulong) bi1._data[i] * (ulong) bi2._data[j]) +
                                    (ulong) result._data[k] + mcarry;

                        result._data[k] = (uint) (val & 0xFFFFFFFF);
                        mcarry = (val >> 32);
                    }

                    if (mcarry != 0)
                        result._data[i + bi2.DataLength] = (uint) mcarry;
                }
            }
            catch (Exception)
            {
                throw (new ArithmeticException("Multiplication overflow."));
            }

            result.DataLength = bi1.DataLength + bi2.DataLength;
            if (result.DataLength > MaxLength)
                result.DataLength = MaxLength;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;
            
            if ((result._data[lastPos] & 0x80000000) != 0)
            {
                if (bi1Neg != bi2Neg && result._data[lastPos] == 0x80000000) 
                {

                    if (result.DataLength == 1)
                        return result;
                    else
                    {
                        bool isMaxNeg = true;
                        for (int i = 0; i < result.DataLength - 1 && isMaxNeg; i++)
                        {
                            if (result._data[i] != 0)
                                isMaxNeg = false;
                        }

                        if (isMaxNeg)
                            return result;
                    }
                }

                throw (new ArithmeticException("Multiplication overflow."));
            }
            
            if (bi1Neg != bi2Neg)
                return -result;

            return result;
        }

        public static BigInteger operator <<(BigInteger bi1, int shiftVal)
        {
            BigInteger result = new BigInteger(bi1);
            result.DataLength = ShiftLeft(result._data, shiftVal);

            return result;
        }

        private static int ShiftLeft(uint[] buffer, int shiftVal)
        {
            int shiftAmount = 32;
            int bufLen = buffer.Length;

            while (bufLen > 1 && buffer[bufLen - 1] == 0)
                bufLen--;

            for (int count = shiftVal; count > 0;)
            {
                if (count < shiftAmount)
                    shiftAmount = count;

                

                ulong carry = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    ulong val = ((ulong) buffer[i]) << shiftAmount;
                    val |= carry;

                    buffer[i] = (uint) (val & 0xFFFFFFFF);
                    carry = val >> 32;
                }

                if (carry != 0)
                {
                    if (bufLen + 1 <= buffer.Length)
                    {
                        buffer[bufLen] = (uint) carry;
                        bufLen++;
                    }
                }

                count -= shiftAmount;
            }

            return bufLen;
        }

        public static BigInteger operator >>(BigInteger bi1, int shiftVal)
        {
            BigInteger result = new BigInteger(bi1);
            result.DataLength = ShiftRight(result._data, shiftVal);


            if ((bi1._data[MaxLength - 1] & 0x80000000) != 0) 
            {
                for (int i = MaxLength - 1; i >= result.DataLength; i--)
                    result._data[i] = 0xFFFFFFFF;

                uint mask = 0x80000000;
                for (int i = 0; i < 32; i++)
                {
                    if ((result._data[result.DataLength - 1] & mask) != 0)
                        break;

                    result._data[result.DataLength - 1] |= mask;
                    mask >>= 1;
                }

                result.DataLength = MaxLength;
            }

            return result;
        }

        private static int ShiftRight(uint[] buffer, int shiftVal)
        {
            int shiftAmount = 32;
            int invShift = 0;
            int bufLen = buffer.Length;

            while (bufLen > 1 && buffer[bufLen - 1] == 0)
                bufLen--;

            

            for (int count = shiftVal; count > 0;)
            {
                if (count < shiftAmount)
                {
                    shiftAmount = count;
                    invShift = 32 - shiftAmount;
                }

                

                ulong carry = 0;
                for (int i = bufLen - 1; i >= 0; i--)
                {
                    ulong val = ((ulong) buffer[i]) >> shiftAmount;
                    val |= carry;

                    carry = ((ulong) buffer[i]) << invShift;
                    buffer[i] = (uint) (val);
                }

                count -= shiftAmount;
            }

            while (bufLen > 1 && buffer[bufLen - 1] == 0)
                bufLen--;

            return bufLen;
        }

        public static BigInteger operator -(BigInteger bi1)
        {
            if (bi1.DataLength == 1 && bi1._data[0] == 0)
                return (new BigInteger());

            BigInteger result = new BigInteger(bi1);

            for (int i = 0; i < MaxLength; i++)
                result._data[i] = (uint) (~(bi1._data[i]));
            
            long val, carry = 1;
            int index = 0;

            while (carry != 0 && index < MaxLength)
            {
                val = (long) (result._data[index]);
                val++;

                result._data[index] = (uint) (val & 0xFFFFFFFF);
                carry = val >> 32;

                index++;
            }

            if ((bi1._data[MaxLength - 1] & 0x80000000) == (result._data[MaxLength - 1] & 0x80000000))
                throw (new ArithmeticException("Overflow in negation.\n"));

            result.DataLength = MaxLength;

            while (result.DataLength > 1 && result._data[result.DataLength - 1] == 0)
                result.DataLength--;
            return result;
        }

        private static void MultiByteDivide(BigInteger bi1, BigInteger bi2,
            BigInteger outQuotient, BigInteger outRemainder)
        {
            uint[] result = new uint[MaxLength];

            int remainderLen = bi1.DataLength + 1;
            uint[] remainder = new uint[remainderLen];

            uint mask = 0x80000000;
            uint val = bi2._data[bi2.DataLength - 1];
            int shift = 0, resultPos = 0;

            while (mask != 0 && (val & mask) == 0)
            {
                shift++;
                mask >>= 1;
            }

            for (int i = 0; i < bi1.DataLength; i++)
                remainder[i] = bi1._data[i];
            ShiftLeft(remainder, shift);
            bi2 = bi2 << shift;

            int j = remainderLen - bi2.DataLength;
            int pos = remainderLen - 1;

            ulong firstDivisorByte = bi2._data[bi2.DataLength - 1];
            ulong secondDivisorByte = bi2._data[bi2.DataLength - 2];

            int divisorLen = bi2.DataLength + 1;
            uint[] dividendPart = new uint[divisorLen];

            while (j > 0)
            {
                ulong dividend = ((ulong) remainder[pos] << 32) + (ulong) remainder[pos - 1];

                ulong q_hat = dividend / firstDivisorByte;
                ulong r_hat = dividend % firstDivisorByte;

                bool done = false;
                while (!done)
                {
                    done = true;

                    if (q_hat == 0x100000000 ||
                        (q_hat * secondDivisorByte) > ((r_hat << 32) + remainder[pos - 2]))
                    {
                        q_hat--;
                        r_hat += firstDivisorByte;

                        if (r_hat < 0x100000000)
                            done = false;
                    }
                }

                for (int h = 0; h < divisorLen; h++)
                    dividendPart[h] = remainder[pos - h];

                BigInteger kk = new BigInteger(dividendPart);
                BigInteger ss = bi2 * (long) q_hat;

                
                while (ss > kk)
                {
                    q_hat--;
                    ss -= bi2;
                    
                }

                BigInteger yy = kk - ss;


                for (int h = 0; h < divisorLen; h++)
                    remainder[pos - h] = yy._data[bi2.DataLength - h];


                result[resultPos++] = (uint) q_hat;

                pos--;
                j--;
            }

            outQuotient.DataLength = resultPos;
            int y = 0;
            for (int x = outQuotient.DataLength - 1; x >= 0; x--, y++)
                outQuotient._data[y] = result[x];
            for (; y < MaxLength; y++)
                outQuotient._data[y] = 0;

            while (outQuotient.DataLength > 1 && outQuotient._data[outQuotient.DataLength - 1] == 0)
                outQuotient.DataLength--;

            if (outQuotient.DataLength == 0)
                outQuotient.DataLength = 1;

            outRemainder.DataLength = ShiftRight(remainder, shift);

            for (y = 0; y < outRemainder.DataLength; y++)
                outRemainder._data[y] = remainder[y];
            for (; y < MaxLength; y++)
                outRemainder._data[y] = 0;
        }

        private static void SingleByteDivide(BigInteger bi1, BigInteger bi2,
            BigInteger outQuotient, BigInteger outRemainder)
        {
            uint[] result = new uint[MaxLength];
            int resultPos = 0;

            
            for (int i = 0; i < MaxLength; i++)
                outRemainder._data[i] = bi1._data[i];
            outRemainder.DataLength = bi1.DataLength;

            while (outRemainder.DataLength > 1 && outRemainder._data[outRemainder.DataLength - 1] == 0)
                outRemainder.DataLength--;

            ulong divisor = (ulong) bi2._data[0];
            int pos = outRemainder.DataLength - 1;
            ulong dividend = (ulong) outRemainder._data[pos];

            if (dividend >= divisor)
            {
                ulong quotient = dividend / divisor;
                result[resultPos++] = (uint) quotient;

                outRemainder._data[pos] = (uint) (dividend % divisor);
            }

            pos--;

            while (pos >= 0)
            {

                dividend = ((ulong) outRemainder._data[pos + 1] << 32) + (ulong) outRemainder._data[pos];
                ulong quotient = dividend / divisor;
                result[resultPos++] = (uint) quotient;

                outRemainder._data[pos + 1] = 0;
                outRemainder._data[pos--] = (uint) (dividend % divisor);
            }

            outQuotient.DataLength = resultPos;
            int j = 0;
            for (int i = outQuotient.DataLength - 1; i >= 0; i--, j++)
                outQuotient._data[j] = result[i];
            for (; j < MaxLength; j++)
                outQuotient._data[j] = 0;

            while (outQuotient.DataLength > 1 && outQuotient._data[outQuotient.DataLength - 1] == 0)
                outQuotient.DataLength--;

            if (outQuotient.DataLength == 0)
                outQuotient.DataLength = 1;

            while (outRemainder.DataLength > 1 && outRemainder._data[outRemainder.DataLength - 1] == 0)
                outRemainder.DataLength--;
        }

        public static BigInteger operator /(BigInteger bi1, BigInteger bi2)
        {
            BigInteger quotient = new BigInteger();
            BigInteger remainder = new BigInteger();

            int lastPos = MaxLength - 1;
            bool divisorNeg = false, dividendNeg = false;

            if ((bi1._data[lastPos] & 0x80000000) != 0) 
            {
                bi1 = -bi1;
                dividendNeg = true;
            }

            if ((bi2._data[lastPos] & 0x80000000) != 0) 
            {
                bi2 = -bi2;
                divisorNeg = true;
            }

            if (bi1 < bi2)
            {
                return quotient;
            }

            else
            {
                if (bi2.DataLength == 1)
                    SingleByteDivide(bi1, bi2, quotient, remainder);
                else
                    MultiByteDivide(bi1, bi2, quotient, remainder);

                if (dividendNeg != divisorNeg)
                    return -quotient;

                return quotient;
            }
        }

        public static BigInteger operator %(BigInteger bi1, BigInteger bi2)
        {
            BigInteger quotient = new BigInteger();
            BigInteger remainder = new BigInteger(bi1);

            int lastPos = MaxLength - 1;
            bool dividendNeg = false;

            if ((bi1._data[lastPos] & 0x80000000) != 0) 
            {
                bi1 = -bi1;
                dividendNeg = true;
            }

            if ((bi2._data[lastPos] & 0x80000000) != 0) 
                bi2 = -bi2;

            if (bi1 < bi2)
            {
                return remainder;
            }

            else
            {
                if (bi2.DataLength == 1)
                    SingleByteDivide(bi1, bi2, quotient, remainder);
                else
                    MultiByteDivide(bi1, bi2, quotient, remainder);

                if (dividendNeg)
                    return -remainder;

                return remainder;
            }
        }

        public override string ToString()
        {
            int radix = 10;

            string charSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";

            BigInteger a = this;

            bool negative = false;
            if ((a._data[MaxLength - 1] & 0x80000000) != 0)
            {
                negative = true;
                try
                {
                    a = -a;
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            BigInteger quotient = new BigInteger();
            BigInteger remainder = new BigInteger();
            BigInteger biRadix = new BigInteger(radix);

            if (a.DataLength == 1 && a._data[0] == 0)
                result = "0";
            else
            {
                while (a.DataLength > 1 || (a.DataLength == 1 && a._data[0] != 0))
                {
                    SingleByteDivide(a, biRadix, quotient, remainder);

                    if (remainder._data[0] < 10)
                        result = remainder._data[0] + result;
                    else
                        result = charSet[(int) remainder._data[0] - 10] + result;

                    a = quotient;
                }

                if (negative)
                    result = "-" + result;
            }

            return result;
        }

        public BigInteger ModPow(BigInteger exp, BigInteger n)
        {
            if ((exp._data[MaxLength - 1] & 0x80000000) != 0)
                throw (new ArithmeticException("Positive exponents only."));

            BigInteger resultNum = 1;
            BigInteger tempNum;
            bool thisNegative = false;

            if ((this._data[MaxLength - 1] & 0x80000000) != 0) 
            {
                tempNum = -this % n;
                thisNegative = true;
            }
            else
                tempNum = this % n; 

            if ((n._data[MaxLength - 1] & 0x80000000) != 0) 
                n = -n;
            
            BigInteger constant = new BigInteger();

            int i = n.DataLength << 1;
            constant._data[i] = 0x00000001;
            constant.DataLength = i + 1;

            constant = constant / n;
            int totalBits = exp.BitCount();
            int count = 0;
            
            for (int pos = 0; pos < exp.DataLength; pos++)
            {
                uint mask = 0x01;
                

                for (int index = 0; index < 32; index++)
                {
                    if ((exp._data[pos] & mask) != 0)
                        resultNum = BarrettReduction(resultNum * tempNum, n, constant);

                    mask <<= 1;

                    tempNum = BarrettReduction(tempNum * tempNum, n, constant);


                    if (tempNum.DataLength == 1 && tempNum._data[0] == 1)
                    {
                        if (thisNegative && (exp._data[0] & 0x1) != 0) 
                            return -resultNum;
                        return resultNum;
                    }

                    count++;
                    if (count == totalBits)
                        break;
                }
            }

            if (thisNegative && (exp._data[0] & 0x1) != 0) 
                return -resultNum;

            return resultNum;
        }  

        private BigInteger BarrettReduction(BigInteger x, BigInteger n, BigInteger constant)
        {
            int k = n.DataLength,
                kPlusOne = k + 1,
                kMinusOne = k - 1;

            BigInteger q1 = new BigInteger();
            
            for (int i = kMinusOne, j = 0; i < x.DataLength; i++, j++)
                q1._data[j] = x._data[i];
            q1.DataLength = x.DataLength - kMinusOne;
            if (q1.DataLength <= 0)
                q1.DataLength = 1;

            BigInteger q2 = q1 * constant;
            BigInteger q3 = new BigInteger();
            
            for (int i = kPlusOne, j = 0; i < q2.DataLength; i++, j++)
                q3._data[j] = q2._data[i];
            q3.DataLength = q2.DataLength - kPlusOne;
            if (q3.DataLength <= 0)
                q3.DataLength = 1;
            
            BigInteger r1 = new BigInteger();
            int lengthToCopy = (x.DataLength > kPlusOne) ? kPlusOne : x.DataLength;
            for (int i = 0; i < lengthToCopy; i++)
                r1._data[i] = x._data[i];
            r1.DataLength = lengthToCopy;

            BigInteger r2 = new BigInteger();
            for (int i = 0; i < q3.DataLength; i++)
            {
                if (q3._data[i] == 0) continue;

                ulong mcarry = 0;
                int t = i;
                for (int j = 0; j < n.DataLength && t < kPlusOne; j++, t++)
                {
                    ulong val = ((ulong) q3._data[i] * (ulong) n._data[j]) +
                                (ulong) r2._data[t] + mcarry;

                    r2._data[t] = (uint) (val & 0xFFFFFFFF);
                    mcarry = (val >> 32);
                }

                if (t < kPlusOne)
                    r2._data[t] = (uint) mcarry;
            }

            r2.DataLength = kPlusOne;
            while (r2.DataLength > 1 && r2._data[r2.DataLength - 1] == 0)
                r2.DataLength--;

            r1 -= r2;
            if ((r1._data[MaxLength - 1] & 0x80000000) != 0) 
            {
                BigInteger val = new BigInteger();
                val._data[kPlusOne] = 0x00000001;
                val.DataLength = kPlusOne + 1;
                r1 += val;
            }

            while (r1 >= n)
                r1 -= n;

            return r1;
        }

        public BigInteger Gcd(BigInteger bi)
        {
            BigInteger x;
            BigInteger y;

            if ((_data[MaxLength - 1] & 0x80000000) != 0) 
                x = -this;
            else
                x = this;

            if ((bi._data[MaxLength - 1] & 0x80000000) != 0) 
                y = -bi;
            else
                y = bi;

            BigInteger g = y;

            while (x.DataLength > 1 || (x.DataLength == 1 && x._data[0] != 0))
            {
                g = x;
                x = y % x;
                y = g;
            }

            return g;
        }

        public void GenerateRandomBits(int bits, Random rand)
        {
            int dwords = bits >> 5;
            int remBits = bits & 0x1F;

            if (remBits != 0)
                dwords++;

            if (dwords > MaxLength)
                throw (new ArithmeticException("Number of required bits > MaxLength."));

            for (int i = 0; i < dwords; i++)
                _data[i] = (uint) (rand.NextDouble() * 0x100000000);

            for (int i = dwords; i < MaxLength; i++)
                _data[i] = 0;

            if (remBits != 0)
            {
                uint mask = (uint) (0x01 << (remBits - 1));
                _data[dwords - 1] |= mask;

                mask = (uint) (0xFFFFFFFF >> (32 - remBits));
                _data[dwords - 1] &= mask;
            }
            else
                _data[dwords - 1] |= 0x80000000;

            DataLength = dwords;

            if (DataLength == 0)
                DataLength = 1;
        }

        public void GenerateRandomBitsFromZero(int bits, Random rand)
        {
            int dwords = bits >> 5;
            int remBits = bits & 0x1F;

            if (remBits != 0)
                dwords++;

            if (dwords > MaxLength)
                throw (new ArithmeticException("Number of required bits > MaxLength."));

            for (int i = 0; i < dwords; i++)
                _data[i] = (uint)(rand.NextDouble() * 0x100000000);

            for (int i = dwords; i < MaxLength; i++)
                _data[i] = 0;

            if (remBits != 0)
            {
                uint mask = (uint)(0xFFFFFFFF >> (32 - remBits));
                _data[dwords - 1] &= mask;
            }

            DataLength = dwords;

            if (DataLength == 0)
                DataLength = 1;
        }

        public int BitCount()
        {
            while (DataLength > 1 && _data[DataLength - 1] == 0)
                DataLength--;

            uint value = _data[DataLength - 1];
            uint mask = 0x80000000;
            int bits = 32;

            while (bits > 0 && (value & mask) == 0)
            {
                bits--;
                mask >>= 1;
            }

            bits += ((DataLength - 1) << 5);

            return bits;
        }

        //***********************************************************************
        // Probabilistic prime test based on Rabin-Miller's
        //
        // for any p > 0 with p - 1 = 2^s * t
        //
        // p is probably prime (strong pseudoprime) if for any a < p,
        // 1) a^t mod p = 1 or
        // 2) a^((2^j)*t) mod p = p-1 for some 0 <= j <= s-1
        //
        // Otherwise, p is composite.
        public bool RabinMillerTest(int confidence)
        {
            BigInteger thisVal;
            if ((this._data[MaxLength - 1] & 0x80000000) != 0) 
                thisVal = -this;
            else
                thisVal = this;

            if (thisVal.DataLength == 1)
            {
                
                if (thisVal._data[0] == 0 || thisVal._data[0] == 1)
                    return false;
                else if (thisVal._data[0] == 2 || thisVal._data[0] == 3)
                    return true;
            }

            if ((thisVal._data[0] & 0x1) == 0) 
                return false;
            
            BigInteger p_sub1 = thisVal - (new BigInteger(1));
            int s = 0;

            for (int index = 0; index < p_sub1.DataLength; index++)
            {
                uint mask = 0x01;

                for (int i = 0; i < 32; i++)
                {
                    if ((p_sub1._data[index] & mask) != 0)
                    {
                        index = p_sub1.DataLength; 
                        break;
                    }
                    mask <<= 1;
                    s++;
                }
            }

            BigInteger t = p_sub1 >> s;

            int bits = thisVal.BitCount();
            BigInteger a = new BigInteger();
            Random rand = new Random();

            for (int round = 0; round < confidence; round++)
            {
                bool done = false;

                while (!done) 
                {
                    int testBits = 0;
                    
                    while (testBits < 2)
                        testBits = (int) (rand.NextDouble() * bits);

                    a.GenerateRandomBits(testBits, rand);

                    int byteLen = a.DataLength;

                    
                    if (byteLen > 1 || (byteLen == 1 && a._data[0] != 1))
                        done = true;
                }
                
                BigInteger gcdTest = a.Gcd(thisVal);
                if (gcdTest.DataLength == 1 && gcdTest._data[0] != 1)
                    return false;

                BigInteger b = a.ModPow(t, thisVal);

                bool result = b.DataLength == 1 && b._data[0] == 1;

                for (int j = 0; result == false && j < s; j++)
                {
                    if (b == p_sub1) 
                    {
                        result = true;
                        break;
                    }

                    b = (b * b) % thisVal;
                }

                if (result == false)
                    return false;
            }

            return true;
        }

        public bool IsProbablePrime(int confidence)
        {
            BigInteger thisVal;
            if ((this._data[MaxLength - 1] & 0x80000000) != 0) 
                thisVal = -this;
            else
                thisVal = this;
            
            for (int p = 0; p < PrimesBelow1000.Length; p++)
            {
                BigInteger divisor = PrimesBelow1000[p];

                if (divisor >= thisVal)
                    break;

                BigInteger resultNum = thisVal % divisor;
                if (resultNum.IntValue() == 0)
                {
                    return false;
                }
            }

            if (thisVal.RabinMillerTest(confidence))
                return true;
            else
            {
                
                return false;
            }
        }

        public int IntValue()
        {
            return (int) _data[0];
        }

        public static BigInteger GeneratePseudoPrime(int bits, int confidence, Random rand)
        {
            BigInteger result = new BigInteger();
            bool done = false;

            while (!done)
            {
                result.GenerateRandomBits(bits, rand);
                result._data[0] |= 0x01; 

                
                done = result.IsProbablePrime(confidence);
            }

            return result;
        }

        public BigInteger genCoPrime(Random rand)
        {
            var bits = this.BitCount();
            bool done = false;
            BigInteger result = new BigInteger();

            while (!done)
            {
                result.GenerateRandomBits(bits, rand);
                
                BigInteger g = result.gcd(this);
                if (g.DataLength == 1 && g._data[0] == 1)
                    done = true;
            }

            return result;
        }

        public BigInteger gcd(BigInteger bi)
        {
            BigInteger x;
            BigInteger y;

            if ((_data[MaxLength - 1] & 0x80000000) != 0)     
                x = -this;
            else
                x = this;

            if ((bi._data[MaxLength - 1] & 0x80000000) != 0)     
                y = -bi;
            else
                y = bi;

            BigInteger g = y;

            while (x.DataLength > 1 || (x.DataLength == 1 && x._data[0] != 0))
            {
                g = x;
                x = y % x;
                y = g;
            }

            return g;
        }

        public byte[] GetAllBytes()
        {
            int numBits = BitCount();

            int numBytes = numBits >> 3;
            if ((numBits & 0x7) != 0)
                numBytes++;

            byte[] result = new byte[numBytes];

            int pos = 0;
            uint tempVal, val = _data[DataLength - 1];

            bool isHighestByteFound = false;

            var shiftedVal = val >> 24;
            var andVal = shiftedVal & 0xFF;
            tempVal = andVal;

            if (tempVal != 0)
            {

                result[pos++] = (byte)tempVal;
                isHighestByteFound = true;
            }

            shiftedVal = val >> 16;
            andVal = shiftedVal & 0xFF;
            tempVal = andVal;

            if ( isHighestByteFound || tempVal != 0)
            {
                result[pos++] = (byte)tempVal;
                isHighestByteFound = true;
            }

            shiftedVal = val >> 8;
            andVal = shiftedVal & 0xFF;
            tempVal = andVal;

            if ( isHighestByteFound ||  tempVal != 0)
            {
                result[pos++] = (byte)tempVal;
                isHighestByteFound = true;
            }

            andVal = val & 0xFF;
            tempVal = andVal;

            if (isHighestByteFound || tempVal != 0 )
            {
                result[pos++] = (byte)tempVal;
            }

            for (int i = DataLength - 2; i >= 0; i--, pos += 4)
            {
                val = _data[i];
                result[pos + 3] = (byte)(val & 0xFF);
                val >>= 8;
                result[pos + 2] = (byte)(val & 0xFF);
                val >>= 8;
                result[pos + 1] = (byte)(val & 0xFF);
                val >>= 8;
                result[pos] = (byte)(val & 0xFF);
            }

            return result;
        }

        #endregion
    }
}