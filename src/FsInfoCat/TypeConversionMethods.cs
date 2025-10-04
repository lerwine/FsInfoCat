using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;

namespace FsInfoCat;

/// <summary>
/// Methods for type conversion.
/// </summary>
public static class TypeConversionMethods
{
    /// <summary>
    /// Attempts to convert a string value to a boolean value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="result">The converted value if the <paramref name="value"/> was able to be converted.</param>
    /// <returns><see langword="true"/> of the <paramref name="value"/> could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryConvertToBoolean(string value, out bool result)
    {
        if (!string.IsNullOrWhiteSpace(value))
            try
            {
                result = XmlConvert.ToBoolean(value);
                return true;
            }
            catch { /* ignored intentionally */ }
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert a string value to a <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="dateTimeOption">The conversion option to use.</param>
    /// <param name="result">The converted value if the <paramref name="value"/> was able to be converted.</param>
    /// <returns><see langword="true"/> of the <paramref name="value"/> could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryConvertToDateTime(string value, XmlDateTimeSerializationMode dateTimeOption, out DateTime result)
    {
        if (!string.IsNullOrWhiteSpace(value))
            try
            {
                result = XmlConvert.ToDateTime(value, dateTimeOption);
                return true;
            }
            catch { /* ignored intentionally */ }
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert a string value to a <see cref="TimeSpan"/> value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="result">The converted value if the <paramref name="value"/> was able to be converted.</param>
    /// <returns><see langword="true"/> of the <paramref name="value"/> could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryConvertToTimeSpan(string value, out TimeSpan result)
    {
        if (!string.IsNullOrWhiteSpace(value))
            try
            {
                result = XmlConvert.ToTimeSpan(value);
                return true;
            }
            catch { /* ignored intentionally */ }
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert a string value to a signed 16-bit integer value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="result">The converted value if the <paramref name="value"/> was able to be converted.</param>
    /// <returns><see langword="true"/> of the <paramref name="value"/> could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryConvertToInt16(string value, out short result)
    {
        if (!string.IsNullOrWhiteSpace(value))
            try
            {
                result = XmlConvert.ToInt16(value);
                return true;
            }
            catch { /* ignored intentionally */ }
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert a string value to a signed 32-bit integer value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="result">The converted value if the <paramref name="value"/> was able to be converted.</param>
    /// <returns><see langword="true"/> of the <paramref name="value"/> could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryConvertToInt32(string value, out int result)
    {
        if (!string.IsNullOrWhiteSpace(value))
            try
            {
                result = XmlConvert.ToInt32(value);
                return true;
            }
            catch { /* ignored intentionally */ }
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert a string value to a signed 64-bit integer value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="result">The converted value if the <paramref name="value"/> was able to be converted.</param>
    /// <returns><see langword="true"/> of the <paramref name="value"/> could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryConvertToInt64(string value, out long result)
    {
        if (!string.IsNullOrWhiteSpace(value))
            try
            {
                result = XmlConvert.ToInt64(value);
                return true;
            }
            catch { /* ignored intentionally */ }
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to parse a string value to a <see cref="Enum"/> value.
    /// </summary>
    /// <typeparam name="TEnum">The type of converted <see cref="Enum"/> value.</typeparam>
    /// <param name="value">The string value to convert.</param>
    /// <param name="result">The converted value if the <paramref name="value"/> was able to be converted.</param>
    /// <returns><see langword="true"/> of the <paramref name="value"/> could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryConvertToEnumValue<TEnum>(string value, out TEnum result)
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        if (!string.IsNullOrWhiteSpace(value) && Enum.TryParse(value.Trim(), out result))
            return true;
        result = default;
        return false;
    }

    private static readonly Regex WsRegex = new(@"[\s\r\n]+", RegexOptions.Compiled);

    /// <summary>
    /// Attempts to parse a string value to <see cref="Enum"/> values.
    /// </summary>
    /// <typeparam name="TEnum">The type of converted <see cref="Enum"/> value.</typeparam>
    /// <param name="value">The string value to convert.</param>
    /// <returns>An enumerable of <typeparamref name="TEnum"/> values.</returns>
    public static IEnumerable<TEnum> GetEnumList<TEnum>(string value)
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        if (value is not null && (value = value.Trim()).Length > 0)
            foreach (string n in WsRegex.Split(value))
            {
                if (!Enum.TryParse(n, out TEnum result))
                    throw new ArgumentOutOfRangeException(nameof(value));
                yield return result;
            }
    }

    /// <summary>
    /// Attempts to parse a string value to a <see cref="Guid"/> value.
    /// </summary>
    /// <param name="value">The string value to convert.</param>
    /// <param name="result">The converted value if the <paramref name="value"/> was able to be converted.</param>
    /// <returns><see langword="true"/> of the <paramref name="value"/> could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryConvertToGuid(string value, out Guid result)
    {
        if (value is not null && value.Length > 31)
        {
            if (value[0] != 'u')
                return Guid.TryParse(value, out result);
            if (value.Length > 40 && value[1] == 'r' && value[2] == 'n' && value[3] == ':' && value[4] == 'u' && value[5] == 'u' && value[6] == 'i' && value[7] == 'd' && value[8] == ':')
                return Guid.TryParse(Uri.UnescapeDataString(value[9..]), out result);
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to parse a sequence of character values to a <see cref="Guid"/> value.
    /// </summary>
    /// <param name="input">The character values to convert.</param>
    /// <param name="result">The converted value if the <paramref name="input"/> was able to be converted.</param>
    /// <returns><see langword="true"/> of the <paramref name="input"/> could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryConvertToGuid(ReadOnlySpan<char> input, out Guid result)
    {
        if (input.Length > 31)
        {
            if (input[0] != 'u')
                return Guid.TryParse(input, out result);
            if (input.Length > 40 && input[1] == 'r' && input[2] == 'n' && input[3] == ':' && input[4] == 'u' && input[5] == 'u' && input[6] == 'i' && input[7] == 'd' && input[8] == ':')
                return Guid.TryParse(Uri.UnescapeDataString(new string(input[9..])), out result);
        }
        result = default;
        return false;
    }
}
