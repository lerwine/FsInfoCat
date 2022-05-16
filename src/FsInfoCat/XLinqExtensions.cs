using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat
{
    public static class XLinqExtensions
    {
        public const string XmlNamespace_FsInfoCatExport = "http://git.erwinefamily.net/FsInfoCat/V1/FsInfoCatExport.xsd";

        public static readonly XNamespace XNamespace_FsInfoCatExport = XNamespace.Get(XmlNamespace_FsInfoCatExport);

        public static XName ToFsInfoCatExportXmlns(this string name) => XNamespace_FsInfoCatExport.GetName(name);

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
            // as <see cref="PreferCData"/>.</remarks>
            MultilineOrNonWhiteSpaceToCData
        }

        /// <summary>
        /// Gets the adjacent nodes that are of the same type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="node">The node.</param>
        /// <returns>IEnumerable&lt;T&gt;.</returns>
        /// <exception cref="ArgumentNullException">node</exception>
        public static IEnumerable<T> GetAdjacentNodes<T>([DisallowNull] this T node) where T : XNode
        {
            if (node is null)
                throw new ArgumentNullException(nameof(node));
            while (node.PreviousNode is T p)
                node = p;
            yield return node;
            while (node.NextNode is T t)
            {
                yield return t;
                node = t;
            }
        }

        public static string AttributeValueOrDefault([AllowNull] this XElement element, [DisallowNull] XName attributeName, string ifNotPresent = null)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                    return attribute.Value;
            }
            return ifNotPresent;
        }

        public static string GetAttributeValue([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string> ifNotPresent)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (ifNotPresent is null)
                throw new ArgumentNullException(nameof(ifNotPresent));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                    return attribute.Value;
            }
            return ifNotPresent();
        }

        public static bool TryGetAttributeValue([AllowNull] this XElement element, [DisallowNull] XName attributeName, out string result)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                {
                    result = attribute.Value;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static T AttributeValueOrDefault<T>([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string, T> ifPresent,
            T ifNotPresent = default)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (ifPresent is null)
                throw new ArgumentNullException(nameof(ifPresent));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                    return ifPresent(attribute.Value);
            }
            return ifNotPresent;
        }

        public static T GetAttributeValue<T>([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string, T> ifPresent,
            [DisallowNull] Func<T> ifNotPresent)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (ifPresent is null)
                throw new ArgumentNullException(nameof(ifPresent));
            if (ifNotPresent is null)
                throw new ArgumentNullException(nameof(ifNotPresent));
            if (element is not null)
            {
                XAttribute attribute = element.Attribute(attributeName);
                if (attribute is not null)
                    return ifPresent(attribute.Value);
            }
            return ifNotPresent();
        }

        public static bool TryGetAttributeValue<T>([AllowNull] this XElement element, [DisallowNull] XName attributeName, [DisallowNull] Func<string, T> converter, out T result)
        {
            if (attributeName is null)
                throw new ArgumentNullException(nameof(attributeName));
            if (converter is null)
                throw new ArgumentNullException(nameof(converter));
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

        public static bool TryConvertToEnumValue<TEnum>(string value, out TEnum result)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (!string.IsNullOrWhiteSpace(value) && Enum.TryParse(value.Trim(), out result))
                return true;
            result = default;
            return false;
        }

        private static readonly Regex WsRegex = new(@"[\s\r\n]+", RegexOptions.Compiled);

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

        public static bool TryConvertToGuid(string value, out Guid result)
        {
            if (!string.IsNullOrWhiteSpace(value))
                try
                {
                    result = XmlConvert.ToGuid(value);
                    return true;
                }
                catch { /* ignored intentionally */ }
            result = default;
            return false;
        }

        public static bool? GetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, bool? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToBoolean(value, out bool result))
                    return result;
            }
            return ifNotPresent;
        }

        public static bool GetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, bool ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToBoolean(value, out bool result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, out bool result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToBoolean(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeBoolean([AllowNull] this XElement element, [DisallowNull] XName attributeName, out bool? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToBoolean(value, out bool r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static DateTime? GetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, DateTime? ifNotPresent = null,
            XmlDateTimeSerializationMode dateTimeOption = XmlDateTimeSerializationMode.RoundtripKind)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToDateTime(value, dateTimeOption, out DateTime result))
                    return result;
            }
            return ifNotPresent;
        }

        public static DateTime GetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, DateTime ifNotPresent,
            XmlDateTimeSerializationMode dateTimeOption = XmlDateTimeSerializationMode.RoundtripKind)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToDateTime(value, dateTimeOption, out DateTime result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, XmlDateTimeSerializationMode dateTimeOption,
            out DateTime result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToDateTime(value, dateTimeOption, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, out DateTime result) =>
            TryGetAttributeDateTime(element, attributeName, XmlDateTimeSerializationMode.RoundtripKind, out result);

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
                if (TryConvertToDateTime(value, dateTimeOption, out DateTime r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static bool TryGetAttributeDateTime([AllowNull] this XElement element, [DisallowNull] XName attributeName, out DateTime? result) =>
            TryGetAttributeDateTime(element, attributeName, XmlDateTimeSerializationMode.RoundtripKind, out result);

        public static TimeSpan? GetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, TimeSpan? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToTimeSpan(value, out TimeSpan result))
                    return result;
            }
            return ifNotPresent;
        }

        public static TimeSpan GetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, TimeSpan ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToTimeSpan(value, out TimeSpan result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TimeSpan result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToTimeSpan(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeTimeSpan([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TimeSpan? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToTimeSpan(value, out TimeSpan r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static short? GetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, short? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToInt16(value, out short result))
                    return result;
            }
            return ifNotPresent;
        }

        public static short GetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, short ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt16(value, out short result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, out short result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt16(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeInt16([AllowNull] this XElement element, [DisallowNull] XName attributeName, out short? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToInt16(value, out short r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static int? GetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, int? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToInt32(value, out int result))
                    return result;
            }
            return ifNotPresent;
        }

        public static int GetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, int ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt32(value, out int result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, out int result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt32(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeInt32([AllowNull] this XElement element, [DisallowNull] XName attributeName, out int? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToInt32(value, out int r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static long? GetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, long? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToInt64(value, out long result))
                    return result;
            }
            return ifNotPresent;
        }

        public static long GetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, long ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt64(value, out long result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, out long result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToInt64(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeInt64([AllowNull] this XElement element, [DisallowNull] XName attributeName, out long? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToInt64(value, out long r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static TEnum? GetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName, TEnum? ifNotPresent = null)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToEnumValue(value, out TEnum result))
                    return result;
            }
            return ifNotPresent;
        }

        public static TEnum GetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName, TEnum ifNotPresent)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToEnumValue(value, out TEnum result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeEnumValue<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName, out TEnum result)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToEnumValue(value, out result))
                return true;
            result = default;
            return false;
        }

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
                if (TryConvertToEnumValue(value, out TEnum r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static IEnumerable<TEnum> GetAttributeEnumFlags<TEnum>([AllowNull] this XElement element, [DisallowNull] XName attributeName)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
                return GetEnumList<TEnum>(value);
            return null;
        }

        public static Guid? GetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, Guid? ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                if (TryConvertToGuid(value, out Guid result))
                    return result;
            }
            return ifNotPresent;
        }

        public static Guid GetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, Guid ifNotPresent)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToGuid(value, out Guid result))
                return result;
            return ifNotPresent;
        }

        public static bool TryGetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, out Guid result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && TryConvertToGuid(value, out result))
                return true;
            result = default;
            return false;
        }

        public static bool TryGetAttributeGuid([AllowNull] this XElement element, [DisallowNull] XName attributeName, out Guid? result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    result = null;
                    return true;
                }
                if (TryConvertToGuid(value, out Guid r))
                {
                    result = r;
                    return true;
                }
            }
            result = default;
            return false;
        }

        public static byte[] GetAttributeBytes([AllowNull] this XElement element, [DisallowNull] XName attributeName, byte[] ifNotPresent = null)
        {
            if (TryGetAttributeValue(element, attributeName, out string value))
            {
                if (value is null || (value = value.Trim()).Length == 0)
                    return null;
                return ByteArrayCoersion.Parse(value).ToArray();
            }
            return ifNotPresent;
        }

        public static bool TryGetAttributeBytes([AllowNull] this XElement element, [DisallowNull] XName attributeName, out byte[] result)
        {
            if (TryGetAttributeValue(element, attributeName, out string value) && ByteArrayCoersion.TryParse(value, out IEnumerable<byte> en))
            {
                result = en.ToArray();
                return true;
            }
            result = default;
            return false;
        }
    }

}
