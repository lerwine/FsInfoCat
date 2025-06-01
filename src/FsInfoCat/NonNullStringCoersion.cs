using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat;

/// <summary>
/// Coerces to a non-null string value.
/// </summary>
/// <param name="comparer">The string equality comparer to use.</param>
public class NonNullStringCoersion(IEqualityComparer<string> comparer) : ICoersion<string>
{
    /// <summary>
    /// Gets the default <see cref="NonNullStringCoersion"/> instance.
    /// </summary>
    public static readonly NonNullStringCoersion Default = new();

    readonly IEqualityComparer<string> _backingComparer = comparer ?? StringComparer.InvariantCulture;

    Type ICoersion.ValueType => typeof(string);

    private NonNullStringCoersion() : this(null) { }

    /// <summary>
    /// Casts the specified object as a <see cref="string"/>.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><paramref name="obj"/> cast as a <see cref="string"/>.</returns>
    /// <exception cref="InvalidCastException"><paramref name="obj"/> could not be cast as a <see cref="string"/>.</exception>
    public virtual string Cast(object obj) => (string)obj ?? "";

    /// <summary>
    /// Coerces the specified object to a <see cref="string"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <returns><paramref name="obj"/> coerced as a <see cref="string"/>.</returns>
    /// <exception cref="NotSupportedException"><paramref name="obj"/> could not be converted to a <see cref="string"/>.</exception>
    public virtual string Coerce(object obj) => (obj is null) ? "" : ((obj is string text) ? text : obj.ToString() ?? "");

    /// <summary>
    /// Normalizes the specified value.
    /// </summary>
    /// <param name="obj">The value to normalize.</param>
    /// <returns>The normalized value.</returns>
    public virtual string Normalize(string obj) => obj ?? "";

    object ICoersion.Normalize(object obj) => Normalize((string)obj);

    bool IEqualityComparer.Equals(object x, object y) => TryCast(x, out string a) && TryCast(y, out string b) ? Equals(a, b) :
        Equals(x, y);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public bool Equals(string x, string y) => string.IsNullOrEmpty(x) ? string.IsNullOrEmpty(y) : (!(y is null) && _backingComparer.Equals(x, y));

    public int GetHashCode(string obj) => _backingComparer.GetHashCode(obj ?? "");
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    int IEqualityComparer.GetHashCode(object obj) => TryCast(obj, out string text) ? GetHashCode(text) : ((obj is null) ? 0 : obj.GetHashCode());

    /// <summary>
    /// Attempts to cast an object as a <see cref="string"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The cast value, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast as a <see cref="string"/>; otherwise, <see langword="false"/>.</returns>
    public virtual bool TryCast(object obj, out string result)
    {
        if (obj is null)
            result = "";
        else if (obj is string text)
            result = text;
        else
        {
            result = null;
            return false;
        }
        return true;
    }

    bool ICoersion.TryCast(object obj, out object result)
    {
        bool r = TryCast(obj, out string text);
        result = text;
        return r;
    }

    /// <summary>
    /// Attempts to coerce an object to a <see cref="string"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The value cast or converted to a <see cref="string"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast, converted, or parsed to a <see cref="string"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public virtual bool TryCoerce(object obj, out string result)
    {
        if (obj is null)
            result = "";
        else if (obj is string text)
            result = text;
        else
        {
            try { result = obj.ToString() ?? ""; }
            catch
            {
                result = null;
                return false;
            }
        }
        return true;
    }

    bool ICoersion.TryCoerce(object obj, out object result)
    {
        bool r = TryCoerce(obj, out string text);
        result = text;
        return r;
    }

    object ICoersion.Cast(object obj) => Cast(obj);

    object ICoersion.Coerce(object obj) => Coerce(obj);
}
