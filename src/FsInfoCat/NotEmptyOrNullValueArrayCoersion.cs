namespace FsInfoCat;

/// <summary>
/// Coerces to a non-empty array or null.
/// </summary>
/// <typeparam name="T">The coerced array element type.</typeparam>
/// <param name="coersion">The object for coercing individual elements.</param>
public class NotEmptyOrNullValueArrayCoersion<T>(ICoersion<T> coersion) : ArrayCoersion<T>(coersion)
{
    /// <summary>
    /// Intializes a new <see cref="NotEmptyOrNullValueArrayCoersion{T}"/> with default element coersion.
    /// </summary>
    public NotEmptyOrNullValueArrayCoersion() : this(null) { }

    /// <summary>
    /// Returns a null value if <see cref="System.Array"/> is empty.
    /// </summary>
    /// <param name="array"></param>
    /// <returns><paramref name="array"/> if <paramref name="array"/> is not empty; otherwise, returns <see langword="null"/>.</returns>
    public static T[] NullIfEmpty(T[] array) => (array is null || array.Length == 0) ? null : array;

    /// <summary>
    /// Casts the specified object as an array of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><paramref name="obj"/> cast as an array of type <typeparamref name="T"/>.</returns>
    /// <exception cref="System.InvalidCastException"><paramref name="obj"/> could not be cast as an array of type <typeparamref name="T"/>.</exception>
    public override T[] Cast(object obj) => NullIfEmpty(base.Cast(obj));

    /// <summary>
    /// Coerces the specified object to an array of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <returns><paramref name="obj"/> coerced as an array of type <typeparamref name="T"/>.</returns>
    /// <exception cref="System.NotSupportedException"><paramref name="obj"/> could not be converted to an array of type <typeparamref name="T"/>.</exception>
    public override T[] Coerce(object obj) => NullIfEmpty(base.Coerce(obj));

    /// <summary>
    /// Normalizes the specified value.
    /// </summary>
    /// <param name="obj">The value to normalize.</param>
    /// <returns>The normalized value.</returns>
    public override T[] Normalize(T[] obj) => (obj is null || obj.Length == 0) ? null : obj;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public override bool Equals(T[] x, T[] y) => base.Equals(NullIfEmpty(x), NullIfEmpty(y));

    public override int GetHashCode(T[] obj) => base.GetHashCode(NullIfEmpty(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Attempts to cast an object as an array of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The cast value, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast as an array of type <typeparamref name="T"/>; otherwise, <see langword="false"/>.</returns>
    public override bool TryCast(object obj, out T[] result)
    {
        bool r = base.TryCast(obj, out result);
        if (!(result is null || result.Length > 0))
            result = null;
        return r;
    }

    /// <summary>
    /// Attempts to coerce an object to an array of type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The value cast or converted to an array of type <typeparamref name="T"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast, converted, or parsed to an array of type <typeparamref name="T"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public override bool TryCoerce(object obj, out T[] result)
    {
        bool r = base.TryCoerce(obj, out result);
        if (!(result is null || result.Length > 0))
            result = null;
        return r;
    }
}
