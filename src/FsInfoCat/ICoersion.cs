using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat
{
    /// <summary>
    /// Interface ICoersion
    /// Implements the <see cref="System.Collections.IEqualityComparer" />
    /// </summary>
    /// <seealso cref="System.Collections.IEqualityComparer" />
    public interface ICoersion : IEqualityComparer
    {
        /// <summary>
        /// Gets the target coersion type.
        /// </summary>
        /// <value>The type that this coersion object supports.</value>
        Type ValueType { get; }

        /// <summary>
        /// Casts the specified object as the type specified by the <see cref="ValueType"/> property.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <returns>The object that was cast as the type specified by the <see cref="ValueType"/> property.</returns>
        object Cast(object obj);

        /// <summary>
        /// Coerces the specified object to the type specified by the <see cref="ValueType"/> property.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <returns>The object that was coerced to the type specified by the <see cref="ValueType"/> property.</returns>
        object Coerce(object obj);

        /// <summary>
        /// Normalizes the specified object.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <returns>The normalized object.</returns>
        object Normalize(object obj);

        /// <summary>
        /// Attempts to cast the specified object as the type specified by the <see cref="ValueType"/> property.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <param name="result">The object that was cast as the type specified by the <see cref="ValueType"/> property, if successful.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast as the type specified by the <see cref="ValueType"/> property;
        /// otherwise, <see langword="false"/>.</returns>
        bool TryCast(object obj, out object result);

        /// <summary>
        /// Attempts to coerce the specified object to the type specified by the <see cref="ValueType"/> property.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <param name="result">The object that was cast or converted to the type specified by the <see cref="ValueType"/> property, if successful.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast or converted to the type specified by the <see cref="ValueType"/> property;
        /// otherwise, <see langword="false"/>.</returns>
        bool TryCoerce(object obj, out object result);
    }

    /// <summary>
    /// Interface ICoersion
    /// Implements the <see cref="FsInfoCat.ICoersion" />
    /// Implements the <see cref="System.Collections.Generic.IEqualityComparer{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="FsInfoCat.ICoersion" />
    /// <seealso cref="System.Collections.Generic.IEqualityComparer{T}" />
    public interface ICoersion<T> : ICoersion, IEqualityComparer<T>
    {
        /// <summary>
        /// Casts the specified object as type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns><paramref name="obj"/> cast as type <typeparamref name="T"/>.</returns>
        /// <exception cref="InvalidCastException"><paramref name="obj"/> could not be cast as type <typeparamref name="T"/>.</exception>
        new T Cast(object obj);

        /// <summary>
        /// Coerces the specified object to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <returns><paramref name="obj"/> coerced as type <typeparamref name="T"/>.</returns>\
        /// <exception cref="NotSupportedException"><paramref name="obj"/> could not be converted to type <typeparamref name="T"/>.</exception>
        new T Coerce(object obj);

        /// <summary>
        /// Normalizes the specified value.
        /// </summary>
        /// <param name="obj">The value to normalize.</param>
        /// <returns>The normalized value.</returns>
        T Normalize(T obj);

        /// <summary>
        /// Attempts to cast an object as type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <param name="result">The cast value, if successful.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast as type <typeparamref name="T"/>; otherwise, <see langword="false"/>.</returns>
        bool TryCast(object obj, out T result);

        /// <summary>
        /// Attempts to coerce an object to type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="obj">The input object.</param>
        /// <param name="result">The value cast or converted to type <typeparamref name="T"/>, if successful.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast or converted to type <typeparamref name="T"/>;
        /// otherwise, <see langword="false"/>.</returns>
        bool TryCoerce(object obj, out T result);
    }
}
