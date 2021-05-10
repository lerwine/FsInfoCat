using System;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Collections
{
    public static class CollectionExtensions
    {
        public static int FindPrimeNumber(int startValue)
        {
            try
            {
                if ((Math.Abs(startValue) & 1) == 0)
                    startValue++;
                while (!IsPrimeNumber(startValue))
                    startValue += 2;
            }
            catch (OverflowException) { return 1; }
            return startValue;
        }

        public static bool IsPrimeNumber(int n)
        {
            if (((n = Math.Abs(n)) & 1) == 0)
                return false;
            for (int i = n >> 1; i > 1; i--)
            {
                if (n % i == 0)
                    return false;
            }
            return true;
        }

        public static T[] CoerceAsArray<T>(this IEnumerable<T> source) => (source is null) ? Array.Empty<T>() : (source is T[] a) ? a : source.ToArray();

        public static int ToAggregateHashCode(this IEnumerable<int> hashCodes)
        {
            int[] arr = hashCodes.CoerceAsArray();
            int prime = arr.Length;
            if (prime == 0)
                return 0;
            if (arr.Length == 1)
                return arr[0];
            int seed = FindPrimeNumber(prime);
            for (int n = 1; n < prime; n++)
                seed = FindPrimeNumber(seed + 1);
            prime = FindPrimeNumber(seed + 1);
            return arr.Aggregate(seed, (a, i) =>
            {
                unchecked { return (a * prime) ^ i; }
            });
        }

        public static int GetAggregateHashCode<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null)
        {
            if (source is null || !source.Any())
                return 0;
            if (comparer is null)
            {
                if (typeof(T).Equals(typeof(object)) || typeof(T).Equals(typeof(ValueType)) || typeof(T).Equals(typeof(void)))
                    return source.Cast<object>().Select(obj => (obj is null) ? 0 : obj.GetHashCode()).ToAggregateHashCode();
                comparer = EqualityComparer<T>.Default;
            }
            return source.Select(obj => comparer.GetHashCode(obj)).ToAggregateHashCode();
        }

        public static int BinarySearch<T>(this IList<T> source, int index, int count, T item, IComparer<T> comparer)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (source is null || count < 1)
                return -1;
            if (comparer is null)
                comparer = Comparer<T>.Default;
            if (source is List<T> list)
                return list.BinarySearch(index, count, item, comparer);
            return source.Skip(index).Take(count).Select((e, i) => new { E = e, I = i }).Where(a => comparer.Compare(a.E, item) == 0).Select(a => a.I + index)
                .DefaultIfEmpty(-1).First();
        }

        public static void CopyTo<T>(this IList<T> source, int index, T[] array, int arrayIndex, int count)
        {
            if (source is null)
                return;
            if (source is List<T> list)
                list.CopyTo(index, array, arrayIndex, count);
            else
                source.Skip(index).Take(count).ToList().CopyTo(array, arrayIndex);
        }

        public static int FindIndex<T>(this IList<T> source, int startIndex, int count, Predicate<T> match)
        {
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (source is null || count < 1)
                return -1;
            if (source is List<T> list)
                list.FindIndex(startIndex, count, match);
            return source.Skip(startIndex).Take(count).Select((e, i) => new { E = e, I = i }).Where(a => match(a.E)).Select(a => a.I + startIndex)
                .DefaultIfEmpty(-1).First();
        }

        public static int FindIndex<T>(this IList<T> source, int startIndex, Predicate<T> match)
        {
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (source is null)
                return -1;
            if (source is List<T> list)
                return list.FindIndex(startIndex, match);
            return source.Skip(startIndex).Select((e, i) => new { E = e, I = i }).Where(a => match(a.E)).Select(a => a.I + startIndex)
                .DefaultIfEmpty(-1).First();
        }

        public static int FindIndex<T>(this IList<T> source, Predicate<T> match)
        {
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (source is null)
                return -1;
            if (source is List<T> list)
                return list.FindIndex(match);
            return source.Select((e, i) => new { E = e, I = i }).Where(a => match(a.E)).Select(a => a.I).DefaultIfEmpty(-1).First();
        }

        public static int FindLastIndex<T>(this IList<T> source, int startIndex, int count, Predicate<T> match)
        {
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (source is null || count < 1)
                return -1;
            if (source is List<T> list)
                return list.FindLastIndex(match);
            return source.Skip(startIndex).Take(count).Select((e, i) => new { E = e, I = i }).Where(a => match(a.E)).Select(a => a.I + startIndex)
                .DefaultIfEmpty(-1).Last();
        }

        public static int FindLastIndex<T>(this IList<T> source, int startIndex, Predicate<T> match)
        {
            if (match is null)
                throw new ArgumentNullException(nameof(match));
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (source is null)
                return -1;
            if (source is List<T> list)
                return list.FindLastIndex(startIndex, match);
            return source.Skip(startIndex).Select((e, i) => new { E = e, I = i }).Where(a => match(a.E)).Select(a => a.I + startIndex)
                .DefaultIfEmpty(-1).Last();
        }

        public static int FindLastIndex<T>(this IList<T> source, Predicate<T> match)
        {
            if (source is List<T> list)
                return list.FindLastIndex(match);
            return source.Select((e, i) => new { E = e, I = i }).Where(a => match(a.E)).Select(a => a.I).DefaultIfEmpty(-1).Last();
        }

        public static int IndexOf<T>(this IList<T> source, T item, int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (source is null || count < 1)
                return -1;
            if (source is List<T> list)
                return list.IndexOf(item, index, count);
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return source.Skip(index).Take(count).Select((e, i) => new { E = e, I = i }).Where(a => comparer.Equals(a.E, item)).Select(a => a.I + index)
                .DefaultIfEmpty(-1).First();
        }

        public static int IndexOf<T>(this IList<T> source, T item, int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (source is null)
                return -1;
            if (source is List<T> list)
                return list.IndexOf(item, index);
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return source.Skip(index).Select((e, i) => new { E = e, I = i }).Where(a => comparer.Equals(a.E, item)).Select(a => a.I + index)
                .DefaultIfEmpty(-1).First();
        }

        public static int LastIndexOf<T>(this IList<T> source, T item, int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (source is null || count < 1)
                return -1;
            if (source is List<T> list)
                return list.IndexOf(item, index, count);
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return source.Skip(index).Take(count).Select((e, i) => new { E = e, I = i }).Where(a => comparer.Equals(a.E, item)).Select(a => a.I + index)
                .DefaultIfEmpty(-1).Last();
        }
    }
}
