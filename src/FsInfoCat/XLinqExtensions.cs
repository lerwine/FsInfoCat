using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat;

/// <summary>
/// Extension methods for XLinq objects.
/// </summary>
public static class XLinqExtensions
{
    /// <summary>
    /// XML namespace for FsInfoCat exports.
    /// </summary>
    public const string XmlNamespace_FsInfoCatExport = "http://git.erwinefamily.net/FsInfoCat/V1/FsInfoCatExport.xsd";

    /// <summary>
    /// XML namespace for object FsInfoCat exports.
    /// </summary>
    public static readonly XNamespace XNamespace_FsInfoCatExport = XNamespace.Get(XmlNamespace_FsInfoCatExport);

    /// <summary>
    /// Creates an XML name with <see cref="XNamespace_FsInfoCatExport"/>.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static XName ToFsInfoCatExportXmlns(this string name) => XNamespace_FsInfoCatExport.GetName(name);

    /// <summary>
    /// Represents preference for encoding text nodes in XML
    /// </summary>
    public enum NormalizeXTextOption
    {
        /// <summary>
        /// Merge adjacent text nodes as a single <see cref="XCData"/> node when any of the original text nodes are a <see cref="XCData"/> node.
        /// </summary>
        PreferCData,

        /// <summary>
        /// Merge adjacent text nodes as a single <see cref="XText"/> node when any of the original text nodes are not a <see cref="XCData"/> node.
        /// </summary>
        PreferXText,

        /// <summary>
        /// Replace/merge all <see cref="XCData"/> nodes with simple <see cref="XText"/> nodes.
        /// </summary>
        NoCData,

        /// <summary>
        /// Replace/merge all <see cref="XText"/> nodes with <see cref="XCData"/> nodes.
        /// </summary>
        AllCData,

        /// <summary>
        /// Replace all <see cref="XText"/> nodes containing at least one non-whitespace character into <see cref="XCData"/> nodes.
        /// </summary>
        /// <remarks>Merged adjacent <see cref="XText"/> nodes without non-whitespace characters follow the same behavior as <see cref="PreferCData"/>.</remarks>
        NonWhiteSpaceToCData,

        /// <summary>
        /// Convert all <see cref="XText"/> nodes containing at least one line separator character into a <see cref="XCData"/> node.
        /// </summary>
        /// <remarks>Merged adjacent <see cref="XText"/> nodes without line separator characters follow the same behavior as <see cref="PreferCData"/>.</remarks>
        MultilineToCData,

        /// <summary>
        /// Convert all <see cref="XText"/> nodes containing at least one non-whitespace or line separator character into a <see cref="XCData"/> node.
        /// </summary>
        /// <remarks>Merged adjacent <see cref="XText"/> nodes without line separator or non-whitespace characters follow the same behavior
        /// as <see cref="PreferCData"/>.</remarks>
        MultilineOrNonWhiteSpaceToCData
    }

    /// <summary>
    /// Gets the adjacent nodes that are of the same type.
    /// </summary>
    /// <typeparam name="T">The type of adjacent XML node to look for.</typeparam>
    /// <param name="node">The node.</param>
    /// <returns>The enumerable adjacent XML nodes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="node"/> is <see langword="null"/>.</exception>
    public static IEnumerable<T> GetAdjacentNodes<T>([DisallowNull] this T node) where T : XNode
    {
        ArgumentNullException.ThrowIfNull(node);
        while (node.PreviousNode is T p)
            node = p;
        yield return node;
        while (node.NextNode is T t)
        {
            yield return t;
            node = t;
        }
    }

    /// <summary>
    /// Gets the value of an XML attribute or a default value if the attribute is not present.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute</param>
    /// <param name="defaultValue">The default value if the attribute is not present</param>
    /// <returns>The value of an XML attribute or <paramref name="defaultValue"/> if the attribute is not present.</returns>
    public static string AttributeValueOrDefault([AllowNull] this XElement element, [DisallowNull] XName attributeName, string defaultValue = null)
    {
        ArgumentNullException.ThrowIfNull(attributeName);
        if (element is not null)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute is not null)
                return attribute.Value;
        }
        return defaultValue;
    }

    /// <summary>
    /// Gets the value of an XML attribute or a default value if the attribute is not present.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute</param>
    /// <param name="getDefaultValue">Produces the default value if the attribute is not present</param>
    /// <returns>The value of an XML attribute or the value returned by <paramref name="getDefaultValue"/> if the attribute is not present.</returns>
    public static string GetAttributeValue([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string> getDefaultValue)
    {
        ArgumentNullException.ThrowIfNull(attributeName);
        ArgumentNullException.ThrowIfNull(getDefaultValue);
        if (element is not null)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute is not null)
                return attribute.Value;
        }
        return getDefaultValue();
    }

    /// <summary>
    /// Attempts to get the value of an XML attribute
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute</param>
    /// <param name="result">Returns the value of the attribute or <see langword="null"/> if the attribute was not present.</param>
    /// <returns><see langword="true"/> if the attribute was present; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeValue([AllowNull] this XElement element, [DisallowNull] XName attributeName, [MaybeNullWhen(true)] out string result)
    {
        ArgumentNullException.ThrowIfNull(attributeName);
        if (element is not null)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute is not null)
            {
                result = attribute.Value;
                return true;
            }
        }
        result = null;
        return false;
    }

    /// <summary>
    /// Gets the converted value of an XML attribute or a default value if the attribute is not present.
    /// </summary>
    /// <typeparam name="T">The type of converted value.</typeparam>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute</param>
    /// <param name="convert">Produces the converted value if the attribute is present.</param>
    /// <param name="ifNotPresent">Optional delegate that produces the default value if the attribute is not present.</param>
    /// <returns>The converted value of an XML attribute; otherwise if the attribute is not present, <see langword="null"/> if <paramref name="ifNotPresent"/> is <see langword="null"/>,
    /// or the value returned by <paramref name="ifNotPresent"/>.</returns>
    public static T AttributeValueOrDefault<T>([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string, T> convert,
        T ifNotPresent = default)
    {
        ArgumentNullException.ThrowIfNull(attributeName);
        ArgumentNullException.ThrowIfNull(convert);
        if (element is not null)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute is not null)
                return convert(attribute.Value);
        }
        return ifNotPresent;
    }

    /// <summary>
    /// Gets the converted value of an XML attribute or a default value if the attribute is not present.
    /// </summary>
    /// <typeparam name="T">The type of converted value.</typeparam>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute</param>
    /// <param name="convert">Produces the converted value if the attribute is present.</param>
    /// <param name="ifNotPresent">Delegate that produces the default value if the attribute is not present.</param>
    /// <returns>The converted value of an XML attribute; otherwise if the attribute is not present, the value returned by <paramref name="ifNotPresent"/>.</returns>
    public static T GetAttributeValue<T>([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string, T> convert,
        [DisallowNull] Func<T> ifNotPresent)
    {
        ArgumentNullException.ThrowIfNull(attributeName);
        ArgumentNullException.ThrowIfNull(convert);
        ArgumentNullException.ThrowIfNull(ifNotPresent);
        if (element is not null)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute is not null)
                return convert(attribute.Value);
        }
        return ifNotPresent();
    }

    /// <summary>
    /// Attempts to get the converted value of an XML element.
    /// </summary>
    /// <typeparam name="T">The type of converted value.</typeparam>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute</param>
    /// <param name="converter">The delegate that attempts to convert the attribute value.</param>
    /// <param name="result">The converted attribute value if the attribute was present and its value could be converted.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeValue<T>([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string, T> converter, [MaybeNullWhen(true)] out T result)
    {
        ArgumentNullException.ThrowIfNull(attributeName);
        ArgumentNullException.ThrowIfNull(converter);
        if (element is not null)
        {
            XAttribute attribute = element.Attribute(attributeName);
            if (attribute is not null)
            {
                result = converter(attribute.Value);
                return true;
            }
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Gets a boolean attribute value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute</param>
    /// <returns>The attribute value converted to boolean, or <see langword="null"/> if the attribute was not present or its value could not be converted.</returns>
    public static bool? GetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && value is not null && (value = value.Trim()).Length > 0 && TypeConversionMethods.TryConvertToBoolean(value, out bool result))
            return result;
        return null;
    }

    /// <summary>
    /// Gets a boolean attribute value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="defaultValue">The default value if the attribute is not present or could not be converted.</param>
    /// <returns>The converted value or the <paramref name="defaultValue"/> if the attribute was not present or its value could not be converted.</returns>
    public static bool GetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, bool defaultValue)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToBoolean(value, out bool result))
            return result;
        return defaultValue;
    }

    /// <summary>
    /// Attempts to get a boolean attribute value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value or <see langword="false"/> if the attribute was not present or its value could not be converted.</param>
    /// <returns><see langword="true"/> if the attribute was present and could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, out bool result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToBoolean(value, out result))
            return true;
        result = false;
        return false;
    }

    /// <summary>
    /// Attempts to get a boolean attribute value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">Returns the converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and was either empty or could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, [MaybeNullWhen(false)] out bool? result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                return true;
            }
            if (TypeConversionMethods.TryConvertToBoolean(value, out bool r))
            {
                result = r;
                return true;
            }
        }
        result = null;
        return false;
    }

    /// <summary>
    /// Gets an attribute value as a <see cref="DateTime"/> value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="dateTimeOption">The optional date/time conversion option.</param>
    /// <returns>The converted date/time value if it could be converted; otherwise, <see langword="null"/>.</returns>
    public static DateTime? GetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName,
        XmlDateTimeSerializationMode dateTimeOption = XmlDateTimeSerializationMode.RoundtripKind)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (value is null || (value = value.Trim()).Length == 0)
                return null;
            if (TypeConversionMethods.TryConvertToDateTime(value, dateTimeOption, out DateTime result))
                return result;
        }
        return null;
    }

    /// <summary>
    /// Gets an attribute value as a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="defaultValue">The optional default value if the attribute was not present or its value could not be converted.</param>
    /// <param name="dateTimeOption">The optional date/time conversion option.</param>
    /// <returns>The converted date/time value if it could be converted; otherwise, the default value.</returns>
    public static DateTime GetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, DateTime defaultValue,
        XmlDateTimeSerializationMode dateTimeOption = XmlDateTimeSerializationMode.RoundtripKind)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToDateTime(value, dateTimeOption, out DateTime result))
            return result;
        return defaultValue;
    }

    /// <summary>
    /// Attempts to get an attribute value as a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="dateTimeOption">The date/time conversion option.</param>
    /// <param name="result">Returns the converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, XmlDateTimeSerializationMode dateTimeOption,
        out DateTime result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToDateTime(value, dateTimeOption, out result))
            return true;
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to get an attribute value as a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">Returns the converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, out DateTime result) =>
        TryGetAttributeDateTime(element, attributeName, XmlDateTimeSerializationMode.RoundtripKind, out result);

    /// <summary>
    /// Attempts to get an attribute value as a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="dateTimeOption">The date/time conversion option.</param>
    /// <param name="result">Returns the converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, XmlDateTimeSerializationMode dateTimeOption,
        out DateTime? result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                return true;
            }
            if (TypeConversionMethods.TryConvertToDateTime(value, dateTimeOption, out DateTime r))
            {
                result = r;
                return true;
            }
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to get an attribute value as a <see cref="DateTime"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">Returns the converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, out DateTime? result) =>
        TryGetAttributeDateTime(element, attributeName, XmlDateTimeSerializationMode.RoundtripKind, out result);

    /// <summary>
    /// Gets an attribute value as a <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns></returns>
    public static TimeSpan? GetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (value is null || (value = value.Trim()).Length == 0)
                return null;
            if (TypeConversionMethods.TryConvertToTimeSpan(value, out TimeSpan result))
                return result;
        }
        return null;
    }

    /// <summary>
    /// Gets an attribute value as a <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="defaultValue">The default value to return if the attribute was not present or its value could not be converted.</param>
    /// <returns>The converted value or <paramref name="defaultValue"/> if the attribute was not present or its value could not be converted.</returns>
    public static TimeSpan GetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, TimeSpan defaultValue)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToTimeSpan(value, out TimeSpan result))
            return result;
        return defaultValue;
    }

    /// <summary>
    /// Attempts to convert an attribute value to a <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value could be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TimeSpan result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToTimeSpan(value, out result))
            return true;
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert an attribute value to a <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was empty or able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TimeSpan? result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                return true;
            }
            if (TypeConversionMethods.TryConvertToTimeSpan(value, out TimeSpan r))
            {
                result = r;
                return true;
            }
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Gets an attribute value as a 16-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The converted value or <see langword="null"/> if the attribute was not present or could not be converted.</returns>
    public static short? GetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (value is null || (value = value.Trim()).Length == 0)
                return null;
            if (TypeConversionMethods.TryConvertToInt16(value, out short result))
                return result;
        }
        return null;
    }

    /// <summary>
    /// Gets an attribute value as a 16-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="defaultValue">The default value to return if the attribute was not present or its value could not be converted.</param>
    /// <returns>The converted value or <paramref name="defaultValue"/> if the attribute was not present or could not be converted.</returns>
    public static short GetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, short defaultValue)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToInt16(value, out short result))
            return result;
        return defaultValue;
    }

    /// <summary>
    /// Attempts to convert an attribute value to a 16-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was empty or able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, out short result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToInt16(value, out result))
            return true;
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert an attribute value to a 16-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, out short? result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                return true;
            }
            if (TypeConversionMethods.TryConvertToInt16(value, out short r))
            {
                result = r;
                return true;
            }
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Gets an attribute value as a 32-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The converted value or <see langword="null"/> if the attribute was not present or could not be converted.</returns>
    public static int? GetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (value is null || (value = value.Trim()).Length == 0)
                return null;
            if (TypeConversionMethods.TryConvertToInt32(value, out int result))
                return result;
        }
        return null;
    }

    /// <summary>
    /// Gets an attribute value as a 32-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="defaultValue">The default value to return if the attribute was not present or its value could not be converted.</param>
    /// <returns>The converted value or <paramref name="defaultValue"/> if the attribute was not present or could not be converted.</returns>
    public static int GetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, int defaultValue)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToInt32(value, out int result))
            return result;
        return defaultValue;
    }

    /// <summary>
    /// Attempts to convert an attribute value to a 32-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, out int result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToInt32(value, out result))
            return true;
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert an attribute value to a 32-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was empty or able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, out int? result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                return true;
            }
            if (TypeConversionMethods.TryConvertToInt32(value, out int r))
            {
                result = r;
                return true;
            }
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Gets an attribute value as a 64-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The converted value or <see langword="null"/> if the attribute was not present or could not be converted.</returns>
    public static long? GetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (value is null || (value = value.Trim()).Length == 0)
                return null;
            if (TypeConversionMethods.TryConvertToInt64(value, out long result))
                return result;
        }
        return null;
    }

    /// <summary>
    /// Gets an attribute value as a 64-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="defaultValue">The default value to return if the attribute was not present or its value could not be converted.</param>
    /// <returns>The converted value or <paramref name="defaultValue"/> if the attribute was not present or could not be converted.</returns>
    public static long GetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, long defaultValue)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToInt64(value, out long result))
            return result;
        return defaultValue;
    }

    /// <summary>
    /// Attempts to convert an attribute value to a 64-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, out long result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToInt64(value, out result))
            return true;
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert an attribute value to a 64-bit integer.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was empty or able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, out long? result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                return true;
            }
            if (TypeConversionMethods.TryConvertToInt64(value, out long r))
            {
                result = r;
                return true;
            }
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Gets an attribute value as a <see cref="Enum"/> value.
    /// </summary>
    /// <typeparam name="TEnum">The type of value to convert to.</typeparam>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The converted value or <see langword="null"/> if the attribute was not present or could not be converted.</returns>
    public static TEnum? GetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName)
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (value is null || (value = value.Trim()).Length == 0)
                return null;
            if (TypeConversionMethods.TryConvertToEnumValue(value, out TEnum result))
                return result;
        }
        return null;
    }

    /// <summary>
    /// Gets an attribute value as a <see cref="Enum"/> value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="defaultValue">The default value to return if the attribute was not present or its value could not be converted.</param>
    /// <returns>The converted value or <paramref name="defaultValue"/> if the attribute was not present or could not be converted.</returns>
    public static TEnum GetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName, TEnum defaultValue)
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToEnumValue(value, out TEnum result))
            return result;
        return defaultValue;
    }

    /// <summary>
    /// Attempts to convert an attribute value to an <see cref="Enum"/> value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TEnum result)
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToEnumValue(value, out result))
            return true;
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert an attribute value to an <see cref="Enum"/> value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was empty or able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TEnum? result)
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                return true;
            }
            if (TypeConversionMethods.TryConvertToEnumValue(value, out TEnum r))
            {
                result = r;
                return true;
            }
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Gets an attribute value as a <see cref="Enum"/> value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns></returns>
    public static IEnumerable<TEnum> GetAttributeEnumFlags<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName)
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
            return TypeConversionMethods.GetEnumList<TEnum>(value);
        return null;
    }

    /// <summary>
    /// Gets an attribute value as a <see cref="Guid"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The converted value or <see langword="null"/> if the attribute was not present or could not be converted.</returns>
    public static Guid? GetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (value is null || (value = value.Trim()).Length == 0)
                return null;
            if (TypeConversionMethods.TryConvertToGuid(value, out Guid result))
                return result;
        }
        return null;
    }

    /// <summary>
    /// Gets an attribute value as a <see cref="Guid"/>.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="defaultValue">The default value to return if the attribute was not present or its value could not be converted.</param>
    /// <returns>The converted value or <paramref name="defaultValue"/> if the attribute was not present or could not be converted.</returns>
    public static Guid GetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, Guid defaultValue)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToGuid(value, out Guid result))
            return result;
        return defaultValue;
    }

    /// <summary>
    /// Attempts to convert an attribute value to a <see cref="Guid"/> value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, out Guid result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && TypeConversionMethods.TryConvertToGuid(value, out result))
            return true;
        result = default;
        return false;
    }

    /// <summary>
    /// Attempts to convert an attribute value to a <see cref="Guid"/> value.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was empty or able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, out Guid? result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = null;
                return true;
            }
            if (TypeConversionMethods.TryConvertToGuid(value, out Guid r))
            {
                result = r;
                return true;
            }
        }
        result = default;
        return false;
    }

    /// <summary>
    /// Gets an attribute value as a byte array.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="affinity">The affinity to use when the attribute value could be parsed as 2 or more formats.</param>
    /// <returns>The converted value or <see langword="null"/> if the attribute was not present or could not be converted.</returns>
    public static byte[] GetAttributeBytes([AllowNull] this XElement element, [DisallowNull] XName attributeName, BinaryStringAffinity affinity)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (value is null || (value = value.Trim()).Length == 0)
                return null;
            return [.. ByteArrayCoersion.Parse(value, affinity)];
        }
        return null;
    }

    /// <summary>
    /// Gets an attribute value as a byte array.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="affinity">The affinity to use when the attribute value could be parsed as 2 or more formats.</param>
    /// <param name="defaultValue">The default value to return if the attribute was not present or its value could not be converted.</param>
    /// <returns>The converted value or <paramref name="defaultValue"/> if the attribute was not present or could not be converted.</returns>
    public static byte[] GetAttributeBytes([AllowNull] this XElement element, [DisallowNull] XName attributeName, BinaryStringAffinity affinity, [DisallowNull] byte[] defaultValue)
    {
        if (TryGetAttributeValue(element, attributeName, out string value))
        {
            if (value is null || (value = value.Trim()).Length == 0)
                return null;
            return [.. ByteArrayCoersion.Parse(value, affinity)];
        }
        return defaultValue;
    }

    /// <summary>
    /// Attempts to convert an attribute value to an array of byte values.
    /// </summary>
    /// <param name="element">The XML element to look for the attribute on.</param>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <param name="affinity">The affinity to use when the attribute value could be parsed as 2 or more formats.</param>
    /// <param name="result">The converted value if the attribute was present and could be converted; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the attribute was present and its value was able to be converted; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetAttributeBytes([AllowNull] this XElement element, [DisallowNull] XName attributeName, BinaryStringAffinity affinity, out byte[] result)
    {
        if (TryGetAttributeValue(element, attributeName, out string value) && ByteArrayCoersion.TryParse(value, affinity, out IEnumerable<byte> en))
        {
            result = [.. en];
            return true;
        }
        result = default;
        return false;
    }
}
