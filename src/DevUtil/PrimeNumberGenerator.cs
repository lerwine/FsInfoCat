using System;
using System.Threading;

namespace DevUtil
{
    /// <summary>
    /// Utility class for generating and caching prime numbers.
    /// </summary>
    public static partial class PrimeNumberGenerator
    {
        private static readonly object _syncRoot = new();
        private static uint[] _cache = new uint[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
        private static readonly int _minArrayLength;

        /// <summary>
        /// Gets the number of values in the the cache.
        /// </summary>
        /// <value>The number of prime number values in the the cache.</value>
        public static int CacheLength { get; private set; }

        static PrimeNumberGenerator() => CacheLength = _minArrayLength = _cache.Length;

        /// <summary>
        /// Gets an object that can be used to access cached prime number values.
        /// </summary>
        public static readonly CacheAccessor Cache = new();

        /// <summary>
        /// Gets a prime number value at the specified sequential index, calculating additional prime numbers if the value has not been cached.
        /// </summary>
        /// <param name="index">The sequential index of the prime number to retrieve.</param>
        /// <returns>An unsigned 32-bit integer value representing the sequentially-indexed prime number value.</returns>
        public static uint Get(int index)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
            Monitor.Enter(_syncRoot);
            try
            {
                int position = _cache.Length;
                if (index < position)
                {
                    if (index >= CacheLength)
                        CacheLength = index + 1;
                    return _cache[index];
                }
                uint value = _cache[position];
                Array.Resize(ref _cache, index + 1);
                try
                {
                    static bool testIsPrime(uint u)
                    {
                        uint m = (uint)Math.Sqrt(u);
                        foreach (uint d in _cache)
                        {
                            if (d > m) break;
                            if (u % d == 0) return false;
                        }
                        return true;
                    }
                    do
                    {
                        value += 2;
                        while (!testIsPrime(value))
                            value += 2;
                        _cache[position++] = value;
                    } while (position <= index);
                }
                finally
                {
                    if (position < _cache.Length)
                        Array.Resize(ref _cache, position);
                    CacheLength = position;
                }
                return value;
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        /// <summary>
        /// Gets 2 prime number values.
        /// </summary>
        /// <param name="index2">The index.</param>
        /// <param name="index1">Index of the relative.</param>
        /// <param name="primeNumber2">The relative value.</param>
        /// <returns>System.UInt32.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">relativeIndex</exception>
        public static uint Get2(int index1, int index2, out uint primeNumber2)
        {
            if (index1 < 0) throw new ArgumentOutOfRangeException(nameof(index1));
            if (index2 < 0) throw new ArgumentOutOfRangeException(nameof(index2));
            Monitor.Enter(_syncRoot);
            try
            {
                if (index1 > index2)
                {
                    uint result = Get(index1);
                    primeNumber2 = _cache[index2];
                    return result;
                }
                primeNumber2 = Get(index2);
                return (index1 < index2) ? _cache[index1] : primeNumber2;
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
