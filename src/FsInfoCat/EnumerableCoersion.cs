using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat;

/// <summary>
/// Class for coersing objects as <see cref="IEnumerable{T}"/>.
/// </summary>
/// <typeparam name="T">The coerced element type.</typeparam>
/// <param name="elementCoersion">Object for coersing individual element values.</param>
/// <returns>An object for coersing objects as <see cref="IEnumerable{T}"/>.</returns>
public class EnumerableCoersion<T>(ICoersion<T> elementCoersion) : ICoersion<IEnumerable<T>>
{
    /// <summary>
    /// Gets the default <see cref="EnumerableCoersion{T}"/> object.
    /// </summary>
    public static readonly EnumerableCoersion<T> Default = new();

    private readonly ICoersion<T> _backingCoersion = elementCoersion ?? Coersion<T>.Default;

    Type ICoersion.ValueType => typeof(IEnumerable<T>);

    /// <summary>
    /// Creates an object for coersing values to <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.
    /// </summary>
    public EnumerableCoersion() : this(null) { }

    /// <summary>
    /// Casts the specified object as <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><paramref name="obj"/> cast as type <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.</returns>
    /// <exception cref="InvalidCastException"><paramref name="obj"/> could not be cast as <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.</exception>
    /// <exception cref="FormatException"><paramref name="obj"/> was a character sequence that could not be parsed as <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.</exception>
    public virtual IEnumerable<T> Cast(object obj) => (IEnumerable<T>)obj;

    /// <summary>
    /// Coerces the specified object to <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <returns><paramref name="obj"/> coerced as <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.</returns>
    /// <exception cref="NotSupportedException"><paramref name="obj"/> could not be converted to <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.</exception>
    public virtual IEnumerable<T> Coerce(object obj)
    {
        if (TryCast(obj, out IEnumerable<T> result))
            return result;
        if (_backingCoersion.TryCast(obj, out T e))
            return [e];
        if (obj is IEnumerable g)
            return g.Cast<object>().Select(_backingCoersion.Coerce);
        return [_backingCoersion.Coerce(obj)];
    }

    /// <summary>
    /// Normalizes the specified <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">The value to normalize.</param>
    /// <returns>The normalized <see cref="IEnumerable{T}"/>.</returns>
    public virtual IEnumerable<T> Normalize(IEnumerable<T> obj) => obj;

    object ICoersion.Normalize(object obj) => Normalize((IEnumerable<T>)obj);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public virtual bool Equals(IEnumerable<T> x, IEnumerable<T> y)
    {
        if (x is null)
            return y is null;
        if (y is null)
            return false;
        return ReferenceEquals(x, y) || x.SequenceEqual(y, _backingCoersion);
    }

    public virtual int GetHashCode(IEnumerable<T> obj) => obj.GetAggregateHashCode(_backingCoersion);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Attempts to cast an object as <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The cast value, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast as <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>; otherwise, <see langword="false"/>.</returns>
    public virtual bool TryCast(object obj, [MaybeNullWhen(false)] out IEnumerable<T> result)
    {
        if (obj is null)
            result = null;
        else if (obj is IEnumerable<T> t)
            result = t;
        else
        {
            result = null;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Attempts to coerce an object to <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The value cast or converted to <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast, converted, or parsed to <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public virtual bool TryCoerce(object obj, [MaybeNullWhen(false)] out IEnumerable<T> result)
    {
        if (TryCast(obj, out result))
            return true;
        if (_backingCoersion.TryCast(obj, out T e))
            result = [e];
        else if (obj is IEnumerable g)
        {
            LinkedList<T> items = new();
            if (g.Cast<object>().All(o =>
            {
                if (_backingCoersion.TryCoerce(o, out e))
                {
                    _ = items.AddLast(e);
                    return true;
                }
                return false;
            }))
                result = items;
            else
            {
                result = null;
                return false;
            }
        }
        else if (_backingCoersion.TryCoerce(obj, out e))
            result = [e];
        else
        {
            result = null;
            return false;
        }
        return true;
    }

    object ICoersion.Cast(object obj) => Cast(obj);

    object ICoersion.Coerce(object obj) => Cast(obj);

    bool ICoersion.TryCast(object obj, out object result)
    {
        bool r = TryCast(obj, out IEnumerable<T> e);
        result = e;
        return r;
    }

    bool ICoersion.TryCoerce(object obj, out object result)
    {
        bool r = TryCoerce(obj, out IEnumerable<T> e);
        result = e;
        return r;
    }

    bool IEqualityComparer.Equals(object x, object y) => TryCast(x, out IEnumerable<T> a) && TryCast(y, out IEnumerable<T> b) ? Equals(a, b) :
        Equals(x, y);

    int IEqualityComparer.GetHashCode(object obj) => (obj is null) ? 0 : (TryCast(obj, out IEnumerable<T> t) ? GetHashCode(t) : obj.GetHashCode());
}

/// <summary>
/// Abstract class for coersing objects as <see cref="IEnumerable{T}"/>.
/// </summary>
/// <typeparam name="TElement">The coerced element type.</typeparam>
/// <typeparam name="TEnumerable">The coerced result type.</typeparam>
/// <param name="elementCoersion">Object for coersing individual element values.</param>
/// <returns>An object for coersing objects as <typeparamref name="TEnumerable"/>.</returns>
public abstract class EnumerableCoersion<TElement, TEnumerable>(ICoersion<TElement> elementCoersion) : ICoersion<TEnumerable>
    where TEnumerable : class, IEnumerable<TElement>
{
    private readonly EnumerableCoersion<TElement> _backingCoersion = new(elementCoersion);

    Type ICoersion.ValueType => typeof(TEnumerable);

    /// <summary>
    /// Creates an object for coersing values to <typeparamref name="TEnumerable"/>>.
    /// </summary>
    public EnumerableCoersion() : this(null) { }

    /// <summary>
    /// Casts the specified object as type <typeparamref name="TEnumerable"/>.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><paramref name="obj"/> cast as type <typeparamref name="TEnumerable"/>.</returns>
    /// <exception cref="InvalidCastException"><paramref name="obj"/> could not be cast as type <typeparamref name="TEnumerable"/>.</exception>
    /// <exception cref="FormatException"><paramref name="obj"/> was a character sequence that could not be parsed as type <typeparamref name="TEnumerable"/>.</exception>
    public virtual TEnumerable Cast(object obj) => (TEnumerable)obj;

    /// <summary>
    /// Coerces the specified object to type <typeparamref name="TEnumerable"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <returns><paramref name="obj"/> coerced as type <typeparamref name="TEnumerable"/>.</returns>
    /// <exception cref="NotSupportedException"><paramref name="obj"/> could not be converted to type <typeparamref name="TEnumerable"/>.</exception>
    public virtual TEnumerable Coerce(object obj)
    {
        if (TryCast(obj, out TEnumerable result))
            return result;
        return CreateFromEnumerable(_backingCoersion.Coerce(obj));
    }

    /// <summary>
    /// Normalizes the specified value.
    /// </summary>
    /// <param name="obj">The value to normalize.</param>
    /// <returns>The normalized value.</returns>
    public virtual TEnumerable Normalize([DisallowNull] TEnumerable obj) => obj;

    object ICoersion.Normalize(object obj) => Normalize((TEnumerable)obj);

    /// <summary>
    /// Attempts to cast the specified object to type <typeparamref name="TEnumerable"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The object that was cast to type <typeparamref name="TEnumerable"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast to type <typeparamref name="TEnumerable"/>; otherwise, <see langword="false"/>.</returns>
    public virtual bool TryCast(object obj, [MaybeNullWhen(false)] out TEnumerable result)
    {
        if (obj is null)
            result = null;
        else if (obj is TEnumerable t)
            result = t;
        else
        {
            result = null;
            return false;
        }
        return true;
    }

    /// <summary>
    /// Attempts to coerce an object to type <typeparamref name="TEnumerable"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The value cast or converted to type <typeparamref name="TEnumerable"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast, converted, or parsed to type <typeparamref name="TEnumerable"/>; otherwise, <see langword="false"/>.</returns>
    public virtual bool TryCoerce(object obj, [MaybeNullWhen(false)] out TEnumerable result)
    {
        if (TryCast(obj, out result))
            return true;
        if (_backingCoersion.TryCoerce(obj, out IEnumerable<TElement> e))
            return TryCreateFromEnumerable(e, out result);
        result = default;
        return false;
    }

    /// <summary>
    /// Creates a <typeparamref name="TEnumerable"/> from an <see cref="IEnumerable{T}"/> with element type <typeparamref name="TElement"/>.
    /// </summary>
    /// <param name="elements">The source <see cref="IEnumerable{T}"/>.</param>
    /// <returns>A <typeparamref name="TEnumerable"/> object.</returns>
    /// <exception cref="NotSupportedException"><paramref name="elements"/> could not be converted to type <typeparamref name="TEnumerable"/>.</exception>
    protected abstract TEnumerable CreateFromEnumerable(IEnumerable<TElement> elements);

    /// <summary>
    /// Attempts to create a <typeparamref name="TEnumerable"/> from an <see cref="IEnumerable{T}"/> with element type <typeparamref name="TElement"/>.
    /// </summary>
    /// <param name="elements">The source <see cref="IEnumerable{T}"/>.</param>
    /// <param name="result">The <typeparamref name="TEnumerable"/> object.</param>
    /// <returns><see langword="true"/> if <paramref name="elements"/> could be converted to type <typeparamref name="TEnumerable"/>;
    /// otherwise, <see langword="false"/>.</returns>
    protected abstract bool TryCreateFromEnumerable(IEnumerable<TElement> elements, out TEnumerable result);

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public virtual bool Equals(TEnumerable x, TEnumerable y) => _backingCoersion.Equals(x, y);

    public virtual int GetHashCode(TEnumerable obj) => _backingCoersion.GetHashCode(obj);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    object ICoersion.Cast(object obj) => Cast(obj);

    object ICoersion.Coerce(object obj) => Coerce(obj);

    bool ICoersion.TryCast(object obj, out object result)
    {
        bool r = TryCast(obj, out TEnumerable e);
        result = e;
        return r;
    }

    bool ICoersion.TryCoerce(object obj, out object result)
    {
        bool r = TryCast(obj, out TEnumerable e);
        result = e;
        return r;
    }

    bool IEqualityComparer.Equals(object x, object y) => ((IEqualityComparer)_backingCoersion).Equals(x, y);

    int IEqualityComparer.GetHashCode(object obj) => ((IEqualityComparer)_backingCoersion).GetHashCode(obj);
}
