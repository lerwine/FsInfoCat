using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Collections
{
    /// <summary>
    /// Extension methods used with collection classes.
    /// </summary>
    public static class CollectionExtensions
    {
        // TODO: Document CollectionExtensions class members
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static string ToCsString(this MultiStringValue source, bool keepLineBreaks = false) => (source is null) ? "" :
            $"\"{ExtensionMethods.EscapeCsString(source, keepLineBreaks)}\"";

        private static IEnumerable<T> PrivateThrowWhenCancellationRequested<T>([DisallowNull] IEnumerable<T> source, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            if (source is not null)
                foreach (T value in source)
                {
                    token.ThrowIfCancellationRequested();
                    yield return value;
                }
        }

        private static IEnumerable<T> PrivateTakeUntilCancellationRequested<T>([DisallowNull] IEnumerable<T> source, CancellationToken token)
        {
            if (!token.IsCancellationRequested)
                foreach (T value in source)
                {
                    if (token.IsCancellationRequested)
                        break;
                    yield return value;
                }
        }

        public static Task RaiseProgressChangedAsync<T>(this IEnumerable<IProgress<T>> eventListeners, T value, CancellationToken cancellationToken) => Task.Run(() =>
            Parallel.ForEach(eventListeners, new ParallelOptions() { CancellationToken = cancellationToken, MaxDegreeOfParallelism = Environment.ProcessorCount }, p =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                p.Report(value);
            })
        );

        public static Task RaiseProgressChangedAsync<T>(this IEnumerable<IProgress<T>> eventListeners, T value) => RaiseProgressChangedAsync(eventListeners, value, CancellationToken.None);

        /// <summary>
        /// Returns an <c><see cref="IEnumerable{T}"/>&lt;<typeparamref name="T"/>&gt;</c> that throws <see cref="OperationCanceledException"/> upon element enumeration if specified <see cref="CancellationToken"/> has had cancellation requested.
        /// </summary>
        /// <typeparam name="T">The type of object to enumerate.</typeparam>
        /// <param name="source">The source <c><see cref="IEnumerable{T}"/>&lt;<typeparamref name="T"/>&gt;</c>.</param>
        /// <param name="token">The cancellation token that is used by to receive notice of cancellation.</param>
        /// <returns>An <c><see cref="IEnumerable{T}"/>&lt;<typeparamref name="T"/>&gt;</c> that throws <see cref="OperationCanceledException"/> upon element enumeration if specified <see cref="CancellationToken"/> has had cancellation requested.</returns>
        /// <exception cref="OperationCanceledException">The <paramref name="token"/> has had cancellation requested.</exception>
        /// <exception cref="ObjectDisposedException">The associated <see cref="CancellationTokenSource"/> has been disposed.</exception>
        /// <remarks>If <paramref name="source"/> is <see langword="null"/> or <c><paramref name="token"/>.<see cref="CancellationToken.CanBeCanceled">CanBeCanceled</see></c> is <see langword="false"/>, then the
        /// value of teh <paramref name="source"/> parameter is returned as-is.</remarks>
        public static IEnumerable<T> ThrowWhenCancellationRequested<T>([AllowNull] this IEnumerable<T> source, CancellationToken token)
        {
            if (source is not null && token.CanBeCanceled)
                return PrivateThrowWhenCancellationRequested(source, token);
            return source;
        }

        /// <summary>
        /// Returns an <c><see cref="IEnumerable{T}"/>&lt;<typeparamref name="T"/>&gt;</c> that enumerates through the source items until the specified <see cref="CancellationToken"/> has had cancellation requested..
        /// </summary>
        /// <typeparam name="T">The type of object to enumerate.</typeparam>
        /// <param name="source">The source <c><see cref="IEnumerable{T}"/>&lt;<typeparamref name="T"/>&gt;</c>.</param>
        /// <param name="token">The cancellation token that is used by to receive notice of cancellation.</param>
        /// <returns>An <c><see cref="IEnumerable{T}"/>&lt;<typeparamref name="T"/>&gt;</c> that enumerates through the <paramref name="source"/> items until the <paramref name="token"/> has had cancellation requested or the
        /// end of the <paramref name="source"/> enumeration has been reached.</returns>
        /// <remarks>If <paramref name="source"/> is <see langword="null"/> or <c><paramref name="token"/>.<see cref="CancellationToken.CanBeCanceled">CanBeCanceled</see></c> is <see langword="false"/>, then the
        /// value of teh <paramref name="source"/> parameter is returned as-is.</remarks>
        public static IEnumerable<T> TakeUntilCancellationRequested<T>([AllowNull] this IEnumerable<T> source, CancellationToken token)
        {
            if (source is not null && token.CanBeCanceled)
                return PrivateTakeUntilCancellationRequested(source, token);
            return source;
        }

        public static IEnumerable<T> NullIfNotAny<T>(this IEnumerable<T> source) => (source is null || !source.Any()) ? null : source;

        public static IEnumerable<T> NonNullElements<T>(this IEnumerable<T> source) where T : class => source?.Where(s => s is not null);

        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> source) => (source is null) ? Array.Empty<T>() : source;

        public static IEnumerable<IndexedValue<T>> ToIndexValuePairs<T>(this IEnumerable<T> source) => source?.Select((e, i) => new IndexedValue<T>(i, e));

        public static IEnumerable<IndexedValue<TResult>> ToIndexValuePairs<TElement, TResult>(this IEnumerable<TElement> source, Func<TElement, TResult> transform)
        {
            if (transform is null)
                throw new ArgumentNullException(nameof(transform));
            return source?.Select((e, i) => new IndexedValue<TResult>(i, transform(e)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, int, TKey> getKey, Func<TSource, int, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return source?.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e, i), getValue(e, i)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, int, TKey> getKey, Func<TSource, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return source?.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e, i), getValue(e)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> getKey, Func<TSource, int, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return source?.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e), getValue(e, i)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TSource, TKey, TValue>(this IEnumerable<TSource> source, Func<TSource, TKey> getKey, Func<TSource, TValue> getValue)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            if (getValue is null)
                throw new ArgumentNullException(nameof(getValue));
            return source?.Select(e => new KeyValuePair<TKey, TValue>(getKey(e), getValue(e)));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, int, TKey> getKey)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            return source?.Select((e, i) => new KeyValuePair<TKey, TValue>(getKey(e, i), e));
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> ToKeyValuePairs<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> getKey)
        {
            if (getKey is null)
                throw new ArgumentNullException(nameof(getKey));
            return source?.Select(e => new KeyValuePair<TKey, TValue>(getKey(e), e));
        }

        //public static IEqualityComparer<T> ToEqualityComparer<T>(this IComparer<T> comparer)
        //{
        //    if (comparer is null)
        //        return EqualityComparer<T>.Default;
        //    if (comparer is IEqualityComparer<T> equalityComparer)
        //        return equalityComparer;
        //    return new Internal.ComparerToEqualityComparer<T>(comparer);
        //}

        //public static IGeneralizableOrderAndEqualityComparer<T> ToOrderAndEqualityComparer<T>(this IEqualityComparer<T> comparer)
        //{
        //    if (comparer is null)
        //        return Defaults<T>.OrderAndEqualityComparer;
        //    if (comparer is IGeneralizableOrderAndEqualityComparer<T> g)
        //        return g;
        //    if (comparer is IComparer<T> c)
        //        return new Internal.GeneralizableOrderAndEqualityComparer<T>(comparer, c);
        //    return new Internal.GeneralizableOrderAndEqualityComparer<T>(comparer, null);
        //}

        //public static IGeneralizableOrderAndEqualityComparer<T> ToOrderAndEqualityComparer<T>(this IComparer<T> comparer)
        //{
        //    if (comparer is null)
        //        return Defaults<T>.OrderAndEqualityComparer;
        //    if (comparer is IGeneralizableOrderAndEqualityComparer<T> g)
        //        return g;
        //    if (comparer is IEqualityComparer<T> c)
        //        return new Internal.GeneralizableOrderAndEqualityComparer<T>(c, comparer);
        //    return new Internal.GeneralizableOrderAndEqualityComparer<T>(null, comparer);
        //}

        //public static IGeneralizableEqualityComparer<T> ToGeneralizable<T>(this IEqualityComparer<T> comparer)
        //{
        //    if (comparer is null)
        //        return Defaults<T>.EqualityComparer;
        //    if (comparer is IGeneralizableEqualityComparer<T> g)
        //        return g;
        //    return new Internal.GeneralizableEqualityComparer<T>(comparer);
        //}

        //public static IGeneralizableEqualityComparer<T> ToGeneralizableEqualityComparer<T>(this IComparer<T> comparer)
        //{
        //    if (comparer is null)
        //        return Defaults<T>.EqualityComparer;
        //    if (comparer is IGeneralizableEqualityComparer<T> g)
        //        return g;
        //    return new Internal.GeneralizableEqualityComparer<T>(comparer);
        //}

        //public static IGeneralizableComparer<T> ToGeneralizable<T>(this IComparer<T> comparer)
        //{
        //    if (comparer is null)
        //        return Defaults<T>.Comparer;
        //    if (comparer is IGeneralizableComparer<T> g)
        //        return g;
        //    return new Internal.GeneralizableComparer<T>(comparer);
        //}

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

        ///// <summary>
        ///// Coerces an <see cref="IEnumerable{T}"/> object as a generic <see cref="IList"/>
        ///// </summary>
        ///// <typeparam name="T">The source element type</typeparam>
        ///// <param name="source">The source <see cref="IEnumerable{T}"/> items.</param>
        ///// <param name="allowNull">If <see langword="true"/> and <paramref name="source"/> is <see langword="null"/> value will be returned;
        ///// otherwise an empty array wll be returned if <paramref name="source"/> is <see langword="null"/>.</param>
        ///// <returns>The <paramref name="source"/> items if it was already a generic <see cref="IList"/>; otherwise, the elements converted
        ///// to a generic <seealso cref="IList"/>.</returns>
        //public static IList CoerceGenericList<T>(this IEnumerable<T> source, bool allowNull = false) => (source is null) ? (allowNull ? null : Array.Empty<T>()) :
        //    (source is IList a) ? a : source.ToArray();

        ///// <summary>
        ///// Coerces an <see cref="IEnumerable{T}"/> object as a strongly typed <seealso cref="IList"/> that can be cat as a generic <see cref="IList"/> as well.
        ///// </summary>
        ///// <typeparam name="T">The source element type</typeparam>
        ///// <param name="source">The source <see cref="IEnumerable{T}"/> items.</param>
        ///// <param name="allowNull">If <see langword="true"/> and <paramref name="source"/> is <see langword="null"/> value will be returned;
        ///// otherwise an empty array wll be returned if <paramref name="source"/> is <see langword="null"/>.</param>
        ///// <returns>The <paramref name="source"/> items if it was already a generic <see cref="IList"/>; otherwise, the elements converted
        ///// to a generic <seealso cref="IList"/>.</returns>
        //public static IReadOnlyList<T> CoerceGeneralizableReadOnlyList<T>(this IEnumerable<T> source, bool allowNull = false) => (source is null) ? (allowNull ? null : Array.Empty<T>()) :
        //    (source is IReadOnlyList<T> r) ? ((r is IList g) ? r : new ReadOnlyCollection<T>((r is IList<T> i) ? i : r.ToArray())) :
        //    new ReadOnlyCollection<T>(source.ToArray());

        /// <summary>
        /// Combines the specified hash codes as a single hash code.
        /// </summary>
        /// <param name="hashCodes">The hash code values to combine.</param>
        /// <returns>The combined hash code value.</returns>
        public static int ToAggregateHashCode(this IEnumerable<int> hashCodes)
        {
            int[] arr = hashCodes.CoerceAsArray();
            int seed = arr.Length;
            if (seed == 0)
                return 0;
            if (arr.Length == 1)
                return arr[0];
            int prime = FindPrimeNumber(seed & 0xffff);
            seed = FindPrimeNumber(prime + 1);
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
            return source.Skip(index).Take(count).Select((e, i) => (E: e, I: i)).Where(a => comparer.Compare(a.E, item) == 0).Select(a => a.I + index)
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
            return source is List<T> list ? list.FindIndex(startIndex, count, match) :
                source.Skip(startIndex).Take(count).Select((e, i) => (E: e, I: i)).Where(a => match(a.E)).Select(a => a.I + startIndex)
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
            return source.Skip(startIndex).Select((e, i) => (E: e, I: i)).Where(a => match(a.E)).Select(a => a.I + startIndex)
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
            return source.Select((e, i) => (E: e, I: i)).Where(a => match(a.E)).Select(a => a.I).DefaultIfEmpty(-1).First();
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
            return source.Skip(startIndex).Take(count).Select((e, i) => (E: e, I: i)).Where(a => match(a.E)).Select(a => a.I + startIndex)
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
            return source.Skip(startIndex).Select((e, i) => (E: e, I: i)).Where(a => match(a.E)).Select(a => a.I + startIndex)
                .DefaultIfEmpty(-1).Last();
        }

        public static int FindLastIndex<T>(this IList<T> source, Predicate<T> match) => source is List<T> list
                ? list.FindLastIndex(match)
                : source.Select((e, i) => (E: e, I: i)).Where(a => match(a.E)).Select(a => a.I).DefaultIfEmpty(-1).Last();

        public static int IndexOf<T>(this IList<T> source, T item, int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (source is null || count < 1)
                return -1;
            if (source is List<T> list)
                return list.IndexOf(item, index, count);
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return source.Skip(index).Take(count).Select((e, i) => (E: e, I: i)).Where(a => comparer.Equals(a.E, item)).Select(a => a.I + index)
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
            return source.Skip(index).Select((e, i) => (E: e, I: i)).Where(a => comparer.Equals(a.E, item)).Select(a => a.I + index)
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
            return source.Skip(index).Take(count).Select((e, i) => (E: e, I: i)).Where(a => comparer.Equals(a.E, item)).Select(a => a.I + index)
                .DefaultIfEmpty(-1).Last();
        }

        //public static class Defaults<T>
        //{
        //    public static readonly IGeneralizableOrderAndEqualityComparer<T> OrderAndEqualityComparer;
        //    public static readonly IGeneralizableEqualityComparer<T> EqualityComparer;
        //    public static readonly IGeneralizableComparer<T> Comparer;

        //    static Defaults()
        //    {
        //        Type type = typeof(T);
        //        if (type.Equals(typeof(string)))
        //        {
        //            OrderAndEqualityComparer = (IGeneralizableOrderAndEqualityComparer<T>)new Internal.GeneralizableOrderAndEqualityComparer<string>(StringComparer.InvariantCulture, StringComparer.InvariantCulture);
        //            EqualityComparer = (IGeneralizableEqualityComparer<T>)new Internal.GeneralizableEqualityComparer<string>((IEqualityComparer<string>)StringComparer.InvariantCulture);
        //            Comparer = (IGeneralizableComparer<T>)new Internal.GeneralizableComparer<string>((IComparer<string>)StringComparer.InvariantCulture);
        //        }
        //        else
        //        {
        //            OrderAndEqualityComparer = new Internal.GeneralizableOrderAndEqualityComparer<T>(EqualityComparer<T>.Default, Comparer<T>.Default);
        //            EqualityComparer = new Internal.GeneralizableEqualityComparer<T>((IEqualityComparer<T>)EqualityComparer<T>.Default);
        //            Comparer = new Internal.GeneralizableComparer<T>((IComparer<T>)Comparer<T>.Default);
        //        }
        //    }

        //}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
