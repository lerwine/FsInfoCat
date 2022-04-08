using System;
using System.Threading;

namespace DevUtil
{
    public static partial class PrimeNumberGenerator
    {
        private static readonly object _syncRoot = new();
        private static uint[] _cache = new uint[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
        private static readonly int _minArrayLength;

        public static int CacheLength { get; private set; }

        static PrimeNumberGenerator() => CacheLength = _minArrayLength = _cache.Length;

        public static readonly CacheAccessor Cache = new();

        public static uint Get(int index)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (index < _cache.Length)
                {
                    if (index >= CacheLength)
                        CacheLength = index + 1;
                    return _cache[index];
                }
                uint[] values = new uint[index];
                Array.Copy(_cache, values, values.Length);
                uint i = _cache[CacheLength - 1] + 2;
                while (!TestPrimeNumber(i)) i += 2;
                int position = CacheLength;
                _cache[position] = i;
                while (++position < index)
                {
                    i += 2;
                    while (!TestPrimeNumber(i)) i += 2;
                    _cache[position] = i;
                }
                CacheLength = index + 1;
                return i;
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public static uint Get(int index, int relativeIndex, out uint relativeValue)
        {
            if (index < 0 || index == int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (relativeIndex == 0 || (relativeIndex > 0) ? (int.MaxValue - index - relativeIndex < 1) : int.MinValue + index + relativeIndex > -1)
                throw new ArgumentOutOfRangeException(nameof(relativeIndex));
            Monitor.Enter(_syncRoot);
            try
            {
                relativeValue = Get(index + relativeIndex);
                return _cache[index];
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public static bool IsPrimeNumber(uint value)
        {
            if (value < 2 || (value & 1) == 0)
                return false;
            Monitor.Enter(_syncRoot);
            try
            {
                int e = ((CacheLength < _minArrayLength) ? _minArrayLength : CacheLength) - 1;
                uint v = _cache[e];
                if (value == v)
                    return true;
                if (value < v)
                {
                    for (int i = 1; i < e; i++)
                    {
                        if (value == _cache[i])
                            return true;
                    }
                    return false;
                }
            }
            finally { Monitor.Exit(_syncRoot); }
            return TestPrimeNumber(value);
        }

        private static bool TestPrimeNumber(uint value)
        {
            uint end = value >> 1;
            for (uint d = 2; d <= end; d++)
            {
                if (value % d == 0)
                    return false;
            }
            return true;
        }
    }
}
