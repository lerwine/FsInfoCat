using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.UnitTests.Helpers;

public class EnumerableComparer<T>(IEqualityComparer<T> elementComparer = null) : IEqualityComparer<IEnumerable<T>>
{
    public static EnumerableComparer<T> Default { get; } = new EnumerableComparer<T>();

    private readonly IEqualityComparer<T> _elementComparer = elementComparer ?? EqualityComparer<T>.Default;

    public bool Equals(IEnumerable<T> x, IEnumerable<T> y)
    {
        if (x is null) return y is null;
        if (y is null) return false;
        if (ReferenceEquals(x, y)) return true;
        if (x is ICollection<T> m && y is ICollection<T> n)
        {
            if (m.Count != n.Count) return false;
            using IEnumerator<T> a = x.GetEnumerator();
            using IEnumerator<T> b = y.GetEnumerator();
            while (a.MoveNext() && b.MoveNext())
                if (!_elementComparer.Equals(a.Current, b.Current))
                    return false;
            return true;
        }
        using IEnumerator<T> c = x.GetEnumerator();
        using IEnumerator<T> d = y.GetEnumerator();
        while (c.MoveNext())
            if (!(d.MoveNext() && _elementComparer.Equals(c.Current, d.Current)))
                return false;
        return !d.MoveNext();
    }

    public int GetHashCode([DisallowNull] IEnumerable<T> obj)
    {
        ArgumentNullException.ThrowIfNull(obj);
        HashCode hashCode = new();
        foreach (var e in obj)
            hashCode.Add(e, _elementComparer);
        return hashCode.ToHashCode();
    }
}
