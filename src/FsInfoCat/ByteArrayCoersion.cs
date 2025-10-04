using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Build.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FsInfoCat;

/// <summary>
/// Class for coersing objects as an <see cref="Array"/> of <see cref="byte"/> values.
/// </summary>
/// <remarks>
/// Initializes a new <see cref="ByteArrayCoersion"/> object.
/// </remarks>
public class ByteArrayCoersion(BinaryStringAffinity affinity = default) : EnumerableCoersion<byte, byte[]>()
{
    /// <summary>
    /// Gets the default <see cref="ByteArrayCoersion"/> object.
    /// </summary>
    public static readonly ByteArrayCoersion Default = new();

    /// <summary>
    /// The affinity to use when parsing byte values from characters.
    /// </summary>
    public BinaryStringAffinity Affinity { get; } = affinity;

    private static Queue<byte> ParseBinHex(ReadOnlySpan<char>.Enumerator enumerator)
    {
        static byte mapToByte(char c)
        {
            if (c < '0') return 255;
            if (c <= '9') return (byte)(c - 48);
            if (c < 'A') return 255;
            if (c <= 'Z') return (byte)(c - 65);
            return (byte)((c < 'a' || c > 'z') ? 255 : c - 97);
        }
        Queue<byte> bytes = new();
        while (enumerator.MoveNext())
        {
            byte v1 = mapToByte(enumerator.Current);
            if (v1 == 0xff) return null;
            byte v2 = mapToByte(enumerator.Current);
            if (v2 == 0xff) return null;
            bytes.Enqueue((byte)((v1 << 4) | v2));
        }
        return bytes;
    }

    /// <summary>
    /// Parses the given BinHex characters and returns the <see cref="byte"/> values.
    /// </summary>
    /// <param name="input">The BinHex encoded characters.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="byte"/> values decoded from <paramref name="input"/>.</returns>
    /// <exception cref="FormatException"><paramref name="input"/> is not a valid BinHex encoded sequence of characters.</exception>
    public static IEnumerable<byte> ParseBinHex(ReadOnlySpan<char> input)// => input.IsEmpty ? [] : ParseBinHex(input.GetEnumerator()) ?? throw new FormatException("");
    {
        if (input.IsEmpty) return [];
        if (input.Length % 2 == 0)
        {
            Queue<byte> bytes = ParseBinHex(input.GetEnumerator());
            if (bytes is not null)
                return bytes;
        }
        throw new FormatException("The input is not a valid BinHex string");
    }

    /// <summary>
    /// Parses the given string as BinHex characters and returns the <see cref="byte"/> values.
    /// </summary>
    /// <param name="input">The BinHex encoded string.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="byte"/> values decoded from <paramref name="input"/>.</returns>
    /// <exception cref="FormatException"><paramref name="input"/> is not a valid BinHex encoded string.</exception>
    public static IEnumerable<byte> ParseBinHex([AllowNull] string input) => (input is null) ? [] : ParseBinHex(input.AsSpan());

    /// <summary>
    /// Attempts to parse the given string as BinHex characters.
    /// </summary>
    /// <param name="input">The BinHex encoded string.</param>
    /// <param name="result">The byte values decoded from the BinHex encoded string, if successful.</param>
    /// <returns><see langword="true"/> if the <paramref name="input"/> is a valid BinHex encoded string; otherwise, <see langword="false"/></returns>
    public static bool TryParseBinHex(ReadOnlySpan<char> input, [NotNullWhen(true)] out IEnumerable<byte> result)
    {
        if (input.IsEmpty)
        {
            result = [];
            return true;
        }
        if (input.Length % 2 == 0)
            return (result = ParseBinHex(input.GetEnumerator())) is not null;
        result = null;
        return false;
    }

    /// <summary>
    /// Attempts to parse the given string as BinHex characters.
    /// </summary>
    /// <param name="input">The BinHex encoded string.</param>
    /// <param name="result">The byte values decoded from the BinHex encoded string, if successful.</param>
    /// <returns><see langword="true"/> if the <paramref name="input"/> is a valid BinHex encoded string; otherwise, <see langword="false"/></returns>
    public static bool TryParseBinHex([AllowNull] string input, [NotNullWhen(true)] out IEnumerable<byte> result)
    {
        if (input is not null) return TryParseBinHex(input.AsSpan(), out result);
        result = [];
        return true;
    }

    private static ReadOnlySpan<byte> B64BitsLookup =>
    [
        0x3e, // +
        0xff, 0xff, 0xff,
        0x3f, // /
        0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, // 0-9
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, // A-Z
        0xff, 0xff, 0xff, 0xff, 0xff, 0xff,
        0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f, 0x30, 0x31, 0x32, 0x33 // a-z
    ];

    private static Queue<byte> ParseBase64(ReadOnlySpan<char>.Enumerator enumerator)
    {
        static bool moveNextNonWs(ReadOnlySpan<char>.Enumerator en, out char result)
        {
            while (en.MoveNext())
            {
                result = en.Current;
                if (!char.IsWhiteSpace(result))
                    return true;
            }
            result = default;
            return false;
        }
        static byte mapToByte(char c) => (c < '+' || c > 'z') ? (byte)0xff : B64BitsLookup[c - 43];
        Queue<byte> bytes = new();
        while (moveNextNonWs(enumerator, out char e))
        {
            byte v1 = mapToByte(e);
            if (v1 == 0xff || !moveNextNonWs(enumerator, out e))
                return null;
            byte v2 = mapToByte(e);
            if (v2 == 0xff || !moveNextNonWs(enumerator, out e))
                return null;
            if (e == '=')
            {
                if ((v2 & 0x0f) != 0 || !enumerator.MoveNext() || enumerator.Current != '=')
                    return null;
                while (enumerator.MoveNext())
                    if (!char.IsWhiteSpace(enumerator.Current))
                        return null;
                bytes.Enqueue((byte)((v1 << 2) | (v2 >> 4)));
                break;
            }
            byte v3 = mapToByte(e);
            if (v3 == 0xff || !moveNextNonWs(enumerator, out e))
                return null;
            bytes.Enqueue((byte)((v1 << 2) | (v2 >> 4)));
            if (e == '=')
            {
                if ((v3 & 0x03) != 0)
                    return null;
                while (enumerator.MoveNext())
                    if (!char.IsWhiteSpace(enumerator.Current))
                        return null;
                bytes.Enqueue((byte)((v2 << 4) | (v3 >> 2)));
                break;
            }
            byte v4 = mapToByte(e);
            if (v4 == 0xff)
                return null;
            bytes.Enqueue((byte)((v2 << 4) | (v3 >> 2)));
            bytes.Enqueue((byte)((v3 << 6) | v4));
        }
        return bytes;
    }

    /// <summary>
    /// Parses the given Base-64 encoded characters and returns the <see cref="byte"/> values.
    /// </summary>
    /// <param name="input">The Base-64 encoded string.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="byte"/> values decoded from <paramref name="input"/>.</returns>
    /// <exception cref="FormatException"><paramref name="input"/> is not a valid Base-64 encoded sequence of characters.</exception>
    public static IEnumerable<byte> ParseBase64(ReadOnlySpan<char> input) => input.IsEmpty ? [] : ParseBase64(input.GetEnumerator()) ?? throw new FormatException("The input is not a valid Base-64 string");

    /// <summary>
    /// Parses the given string as Base-64 encoded string and returns the <see cref="byte"/> values.
    /// </summary>
    /// <param name="input">The Base-64 encoded string.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="byte"/> values decoded from <paramref name="input"/>.</returns>
    /// <exception cref="FormatException"><paramref name="input"/> is not a valid Base-64 string.</exception>
    public static IEnumerable<byte> ParseBase64([AllowNull] string input) => (input is null) ? [] : ParseBase64(input.AsSpan());

    /// <summary>
    /// Attempts to parse the given Base-64 encoded characters.
    /// </summary>
    /// <param name="input">The Base-64 encoded string.</param>
    /// <param name="result">The byte values decoded from the Base-64 encoded characters, if successful.</param>
    /// <returns><see langword="true"/> if the <paramref name="input"/> is a valid Base-64 encoded sequence of characters; otherwise, <see langword="false"/></returns>
    public static bool TryParseBase64(ReadOnlySpan<char> input, [NotNullWhen(true)] out IEnumerable<byte> result)
    {
        if (input.IsEmpty)
        {
            result = [];
            return true;
        }

        return (result = ParseBase64(input.GetEnumerator())) is not null;
    }

    /// <summary>
    /// Attempts to parse the given string as Base-64.
    /// </summary>
    /// <param name="input">The Base-64 encoded string.</param>
    /// <param name="result">The byte values decoded from the Base-64 encoded string, if successful.</param>
    /// <returns><see langword="true"/> if the <paramref name="input"/> is a valid Base-64 encoded string; otherwise, <see langword="false"/></returns>
    public static bool TryParseBase64([AllowNull] string input, [NotNullWhen(true)] out IEnumerable<byte> result)
    {
        if (input is not null) return TryParseBase64(input.AsSpan(), out result);
        result = [];
        return true;
    }

    /// <summary>
    /// Parses the given string as an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values.
    /// </summary>
    /// <param name="input">The BinHex, UUID, or Base-64 string.</param>
    /// <param name="affinity">The affinity to use when the <paramref name="input"/> could be parsed as 2 or more formats.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="byte"/> values parsed from <paramref name="input"/>.</returns>
    /// <exception cref="FormatException"><paramref name="input"/> could not be parsed as an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values.</exception>
    public static IEnumerable<byte> Parse([AllowNull] string input, BinaryStringAffinity affinity = default) => (input is null) ? [] : Parse(input.AsSpan(), affinity);

    /// <summary>
    /// Parses the given charcter values as an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values.
    /// </summary>
    /// <param name="input">The BinHex, UUID, or Base-64 string.</param>
    /// <param name="affinity">The affinity to use when the <paramref name="input"/> could be parsed as 2 or more formats.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="byte"/> values parsed from <paramref name="input"/>.</returns>
    /// <exception cref="FormatException"><paramref name="input"/> could not be parsed as an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values.</exception>
    public static IEnumerable<byte> Parse(ReadOnlySpan<char> input, BinaryStringAffinity affinity = default)
    {
        if (input.IsEmpty) return [];
        IEnumerable<byte> result;
        Guid guid;
        switch (affinity)
        {
            case BinaryStringAffinity.UUID_B64_BinHex:
                if (TypeConversionMethods.TryConvertToGuid(input, out guid))
                    return guid.ToByteArray();
                if (TryParseBase64(input, out result) || TryParseBinHex(input, out result))
                    return result;
                throw new FormatException("The input is not a valid UUID, Base-64, or BinHex string");
            case BinaryStringAffinity.B64_UUID_BinHex:
                if (TryParseBase64(input, out result))
                    return result;
                if (TypeConversionMethods.TryConvertToGuid(input, out guid))
                    return guid.ToByteArray();
                if (TryParseBinHex(input, out result))
                    return result;
                throw new FormatException("The input is not a valid Base-64, UUID, or BinHex string");
            case BinaryStringAffinity.B64_BinHex_UUID:
                if (TryParseBase64(input, out result) || TryParseBinHex(input, out result))
                    return result;
                if (TypeConversionMethods.TryConvertToGuid(input, out guid))
                    return guid.ToByteArray();
                throw new FormatException("The input is not a valid Base-64, BinHex, or UUID string");
            case BinaryStringAffinity.BinHex_B64_UUID:
                if (TryParseBinHex(input, out result) || TryParseBase64(input, out result))
                    return result;
                if (TypeConversionMethods.TryConvertToGuid(input, out guid))
                    return guid.ToByteArray();
                throw new FormatException("The input is not a valid BinHex, Base-64, or UUID string");
            case BinaryStringAffinity.BinHex_UUID_B64:
                if (TryParseBinHex(input, out result) )
                    return result;
                if (TypeConversionMethods.TryConvertToGuid(input, out guid))
                    return guid.ToByteArray();
                if (TryParseBase64(input, out result))
                    return result;
                throw new FormatException("The input is not a valid BinHex, UUID, or Base-64 string");
            default: // UUID_BinHex_B64
                if (TypeConversionMethods.TryConvertToGuid(input, out guid))
                    return guid.ToByteArray();
                if (TryParseBinHex(input, out result) || TryParseBase64(input, out result))
                    return result;
                throw new FormatException("The input is not a valid UUID, BinHex, or Base-64 string");
        }
    }

    /// <summary>
    /// Attempts to parse the given string as an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values.
    /// </summary>
    /// <param name="input">The BinHex, UUID, or Base-64 string.</param>
    /// <param name="result">Returns an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values parsed from <paramref name="input"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="input"/> was parsed as an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryParse([AllowNull] string input, [NotNullWhen(true)] out IEnumerable<byte> result) => TryParse(input, default, out result);

    /// <summary>
    /// Attempts to parse the given string as an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values.
    /// </summary>
    /// <param name="input">The BinHex, UUID, or Base-64 string.</param>
    /// <param name="affinity">The affinity to use when the <paramref name="input"/> could be parsed as 2 or more formats.</param>
    /// <param name="result">Returns an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values parsed from <paramref name="input"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="input"/> was parsed as an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryParse([AllowNull] string input, BinaryStringAffinity affinity, [NotNullWhen(true)] out IEnumerable<byte> result)
    {
        if (input is not null) return TryParse(input.AsSpan(), affinity, out result);
        result = [];
        return true;
    }

    /// <summary>
    /// Attempts to parse the given charcter values as an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values.
    /// </summary>
    /// <param name="input">The BinHex, UUID, or Base-64 string.</param>
    /// <param name="affinity">The affinity to use when the <paramref name="input"/> could be parsed as 2 or more formats.</param>
    /// <param name="result">Returns an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values parsed from <paramref name="input"/>, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="input"/> was parsed as an <see cref="IEnumerable{T}"/> of <see cref="byte"/> values;
    /// otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> input, BinaryStringAffinity affinity, [NotNullWhen(true)] out IEnumerable<byte> result)
    {
        if (input.Length == 0)
        {
            result = [];
            return true;
        }
        throw new NotImplementedException();
    }

    /// <summary>
    /// Creates an <see cref="Array"/> of <see cref="byte"/> values from an enumeration of byte values.
    /// </summary>
    /// <param name="elements">The byte values to convert.</param>
    /// <returns>The byte values as an <see cref="Array"/>.</returns>
    protected override byte[] CreateFromEnumerable([AllowNull] IEnumerable<byte> elements) => elements?.ToArray();

    /// <summary>
    /// Attempst to create an <see cref="Array"/> of <see cref="byte"/> values from an enumeration of byte values.
    /// </summary>
    /// <param name="elements">The byte values to convert.</param>
    /// <param name="result">Returns the byte values as an <see cref="Array"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="elements"/> was <see langword="null"/> or converted to an <see cref="Array"/> of <see cref="byte"/> values;
    /// otherwise, <see langword="false"/>.</returns>
    protected override bool TryCreateFromEnumerable([AllowNull] IEnumerable<byte> elements, out byte[] result)
    {
        result = elements?.ToArray();
        return true;
    }

    /// <summary>
    /// Coerces the specified object to an <see cref="Array"/> of <see cref="byte"/> values.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <returns><paramref name="obj"/> coerced as an <see cref="Array"/> of <see cref="byte"/> values.</returns>
    /// <exception cref="NotSupportedException"><paramref name="obj"/> could not be converted to an <see cref="Array"/> of <see cref="byte"/> values.</exception>
    /// <exception cref="FormatException"><paramref name="obj"/> was a character sequence that could not be parsed as an <see cref="Array"/> of <see cref="byte"/> values.</exception>
    public override byte[] Coerce(object obj) => obj switch
    {
        byte[] arr => arr,
        string ec => [.. Parse(ec, Affinity)],
        _ => base.Coerce(obj),
    };


    /// <summary>
    /// Attempts to coerce an object to an <see cref="Array"/> of <see cref="byte"/> values.
    /// </summary>
    /// <param name="obj">The input object.</param>
    /// <param name="result">The value cast or converted to an <see cref="Array"/> of <see cref="byte"/> values, if successful.</param>
    /// <returns><see langword="true"/> if <paramref name="obj"/> could be cast, converted, or parsed as an <see cref="Array"/> of <see cref="byte"/> values; otherwise, <see langword="false"/>.</returns>
    public override bool TryCoerce(object obj, out byte[] result)
    {
        switch (obj)
        {
            case byte[] arr:
                result = arr;
                break;
            case string ec:
                result = [.. Parse(ec, Affinity)];
                break;
            default:
                return base.TryCoerce(obj, out result);
        }
        return true;
    }
}
