using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Collections
{
    /// <summary>
    /// Extension methods used with collection classes.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Finds the next prime number that is greater than or equal to the specified value.
        /// </summary>
        /// <param name="startValue">The starting value.</param>
        /// <returns>The next prime number that is greater than or equal to the specified <paramref name="startValue"/>.</returns>
        /// <remarks>This can be used to create hash codes.</remarks>
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

        /// <summary>
        /// Determines whether a value is aprime number.
        /// </summary>
        /// <param name="value">The value to test.</param>
        /// <returns>
        /// <see langword="true"/> if the specified <paramref name="value"/> is a prime number; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool IsPrimeNumber(int value)
        {
            if (((value = Math.Abs(value)) & 1) == 0)
                return false;
            for (int i = value >> 1; i > 1; i--)
            {
                if (value % i == 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Coerces an <see cref="IEnumerable{T}"/> object as an array.
        /// </summary>
        /// <typeparam name="T">The element type.</typeparam>
        /// <param name="source">The source <see cref="IEnumerable{T}"/>.</param>
        /// <param name="allowNull">If <see langword="true"/> and <paramref name="source"/> is <see langword="null"/> value will be returned;
        /// otherwise an empty array wll be returned if <paramref name="source"/> is <see langword="null"/>.</param>
        /// <returns>The <paramref name="source"/> value if it was already an array, or the elements of the <paramref name="source"/> converted to an array.</returns>
        public static T[] CoerceAsArray<T>(this IEnumerable<T> source, bool allowNull = false) => (source is null) ? (allowNull ? null : Array.Empty<T>()) :
            (source is T[] a) ? a : source.ToArray();

        /// <summary>
        /// Coerces an <see cref="IEnumerable{T}"/> object as a generic <see cref="IList"/>
        /// </summary>
        /// <typeparam name="T">The source element type</typeparam>
        /// <param name="source">The source <see cref="IEnumerable{T}"/> items.</param>
        /// <param name="allowNull">If <see langword="true"/> and <paramref name="source"/> is <see langword="null"/> value will be returned;
        /// otherwise an empty array wll be returned if <paramref name="source"/> is <see langword="null"/>.</param>
        /// <returns>The <paramref name="source"/> items if it was already a generic <see cref="IList"/>; otherwise, the elements converted
        /// to a generic <seealso cref="IList"/>.</returns>
        public static IList CoerceGenericList<T>(this IEnumerable<T> source, bool allowNull = false) => (source is null) ? (allowNull ? null : Array.Empty<T>()) :
            (source is IList a) ? a : source.ToArray();

        /// <summary>
        /// Coerces an <see cref="IEnumerable{T}"/> object as a strongly typed <seealso cref="IList"/> that can be cat as a generic <see cref="IList"/> as well.
        /// </summary>
        /// <typeparam name="T">The source element type</typeparam>
        /// <param name="source">The source <see cref="IEnumerable{T}"/> items.</param>
        /// <param name="allowNull">If <see langword="true"/> and <paramref name="source"/> is <see langword="null"/> value will be returned;
        /// otherwise an empty array wll be returned if <paramref name="source"/> is <see langword="null"/>.</param>
        /// <returns>The <paramref name="source"/> items if it was already a generic <see cref="IList"/>; otherwise, the elements converted
        /// to a generic <seealso cref="IList"/>.</returns>
        public static IReadOnlyList<T> CoerceGeneralizableReadOnlyList<T>(this IEnumerable<T> source, bool allowNull = false) => (source is null) ? (allowNull ? null : Array.Empty<T>()) :
            (source is IReadOnlyList<T> r) ? ((r is IList g) ? r : new ReadOnlyCollection<T>((r is IList<T> i) ? i : r.ToArray())) :
            new ReadOnlyCollection<T>(source.ToArray());

        /// <summary>
        /// Combines the specified hash codes as a single hash code.
        /// </summary>
        /// <param name="hashCodes">The hash code values to combine.</param>
        /// <returns>The combined hash code value.</returns>
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

        /// <summary>
        /// Gets the aggregate hash code.
        /// </summary>
        /// <typeparam name="T">The type of object from which the hash code will be obtained.</typeparam>
        /// <param name="source">The source <see cref="IEnumerable{T}"/> values from which to create an aggregated hash code.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> that will be used to generate a hash code for each element.
        /// If not specified, the <see cref="EqualityComparer{T}.Default"/> will be used.</param>
        /// <returns>The aggregated hash code that is derived from the hash code of each <paramref name="source"/> element.</returns>
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
