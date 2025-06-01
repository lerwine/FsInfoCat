using System.Collections.Generic;

namespace FsInfoCat;

/// <summary>
/// Coerces to a whitespace-normalized, trimmed string value.
/// </summary>
/// <param name="comparer">The string equality comparer to use.</param>
public class TrimmedNonNullStringCoersion(IEqualityComparer<string> comparer) : NonNullStringCoersion(comparer)
{
    /// <summary>
    /// Gets the default <see cref="TrimmedNonNullStringCoersion"/> instance.
    /// </summary>
    public static new readonly TrimmedNonNullStringCoersion Default = new(null);

    /// <summary>
    /// Casts the specified object as a <see cref="string"/>.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <returns><paramref name="obj"/> cast as a <see cref="string"/>.</returns>
    /// <exception cref="System.InvalidCastException"><paramref name="obj"/> could not be cast as a <see cref="string"/>.</exception>
    public override string Cast(object obj) => base.Cast(obj).Trim();

    /// <summary>
    /// Coerces the specified object to a <see cref="string"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <returns><paramref name="obj"/> coerced as a <see cref="string"/>.</returns>
    /// <exception cref="System.NotSupportedException"><paramref name="obj"/> could not be converted to a <see cref="string"/>.</exception>
    public override string Coerce(object obj) => base.Coerce(obj).Trim();

    /// <summary>
    /// Normalizes the specified value.
    /// </summary>
    /// <param name="obj">The value to normalize.</param>
    /// <returns>The normalized value.</returns>
    public override string Normalize(string obj) => (obj is null) ? "" : obj.Trim();

    /// <summary>
    /// Attempts to cast an object as a <see cref="string"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The cast value, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast as a <see cref="string"/>; otherwise, <see langword="false"/>.</returns>
    public override bool TryCast(object obj, out string result)
    {
        if (base.TryCast(obj, out result))
        {
            result = result.Trim();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Attempts to coerce an object to a <see cref="string"/>.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The value cast or converted to a <see cref="string"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast, converted, or parsed to a <see cref="string"/>;
    /// otherwise, <see langword="false"/>.</returns>
    public override bool TryCoerce(object obj, out string result)
    {
        if (base.TryCast(obj, out result))
        {
            result = result.Trim();
            return true;
        }
        return false;
    }
}
