using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat;

/// <summary>
/// Converts objects to a value type.
/// </summary>
/// <typeparam name="T">The coersion value type.</typeparam>
public class ValueCoersion<T> : Coersion<T>
    where T : struct
{
    /// <summary>
    /// Gets the default <see cref="ValueCoersion{T}"/> instance.
    /// </summary>
    public static readonly new ValueCoersion<T> Default = new();
    
    private static readonly EqualityComparer<T> _comparer = EqualityComparer<T>.Default;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public override bool Equals(T x, T y) => _comparer.Equals(x, y);

    public override int GetHashCode(T obj) => _comparer.GetHashCode(obj);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Attempts to cast an object to type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">The object to cast.</param>
    /// <param name="result">The object cast as type <typeparamref name="T"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> was type <typeparamref name="T"/>; otherwise, <see langword="false"/>.</returns>
    public override bool TryCast(object obj, out T result)
    {
        if (obj is T t)
        {
            result = t;
            return true;
        }
        result = default;
        return false;
    }
}
