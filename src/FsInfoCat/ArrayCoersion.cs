using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat;

/// <summary>
/// Class for coersing objects as an <see cref="System.Array"/>.
/// </summary>
/// <typeparam name="T">The coerced element type.</typeparam>
public class ArrayCoersion<T> : EnumerableCoersion<T, T[]>
{
    /// <summary>
    /// Gets the default <see cref="ArrayCoersion{T}"/> instance.
    /// </summary>
    public static readonly ArrayCoersion<T> Default = new();

    /// <summary>
    /// Intializes a new <see cref="ArrayCoersion{T}"/> instance using the specified element coersion object.
    /// </summary>
    /// <param name="elementCoersion">The object that coerces individual values as type <typeparamref name="T"/>.</param>
    public ArrayCoersion(ICoersion<T> elementCoersion) : base(elementCoersion) { }

    /// <summary>
    /// Intializes a new <see cref="ArrayCoersion{T}"/> instance with default element coersion.
    /// </summary>
    public ArrayCoersion() : base() { }

    /// <summary>
    /// Creates an array of <typeparamref name="T"/> values from an <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="elements">The source <see cref="IEnumerable{T}"/>.</param>
    /// <returns>An array of <typeparamref name="T"/> values from <paramref name="elements"/>.</returns>
    protected override T[] CreateFromEnumerable([AllowNull] IEnumerable<T> elements) => elements?.ToArray();

    /// <summary>
    /// Attempts to create an array of <typeparamref name="T"/> values from an <see cref="IEnumerable{T}"/> with element type <typeparamref name="T"/>.
    /// </summary>
    /// <param name="elements">The source <see cref="IEnumerable{T}"/>.</param>
    /// <param name="result">Returns an array of <typeparamref name="T"/> values from <paramref name="elements"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="elements"/> could be converted to an array of <typeparamref name="T"/> values with element type <typeparamref name="T"/>;
    /// otherwise, <see langword="false"/>.</returns>
    protected override bool TryCreateFromEnumerable([AllowNull] IEnumerable<T> elements, out T[] result)
    {
        result = elements?.ToArray();
        return true;
    }
}
