using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.CodeAnalysis;

namespace FsInfoCat.Generator
{
    public static class XmlExtensionMethods
    {
        public static readonly XNamespace ModelNamespace = XNamespace.None;

        public static string GetXmlLocalName(this XmlNames name) => name.ToString("F");

        public static XName GetModelXName(this XmlNames name) => ModelNamespace.GetName(name.GetXmlLocalName());

        public static XName GetLocalXName(this XmlNames name) => XNamespace.None.GetName(name.GetXmlLocalName());

        public static IEnumerable<XElement> Ancestors<T>(this IEnumerable<T> source, XmlNames name) where T : XNode => source.Ancestors(GetModelXName(name));

        public static IEnumerable<XElement> AncestorsAndSelf(this IEnumerable<XElement> source, XmlNames name) => source.AncestorsAndSelf(GetModelXName(name));

        public static XAttribute Attribute(this XElement element, XmlNames name) => element?.Attribute(GetLocalXName(name));

        #region GetAttributeValue

        public static string GetAttributeValue(this XElement element, XName name!!) => element?.Attribute(name)?.Value;

        public static string GetAttributeValue(this XElement element, XmlNames name) => GetAttributeValue(element, GetLocalXName(name));

        public static string GetAttributeValue(this XElement element, XName name!!, string defaultValue!!) => element?.Attribute(name)?.Value ?? defaultValue;

        public static string GetAttributeValue(this XElement element, XmlNames name, string defaultValue!!) => GetAttributeValue(element, GetLocalXName(name) ?? defaultValue);

        public static bool? GetAttributeBooleanValue(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out bool result) ? result : null;

        public static bool? GetAttributeBooleanValue(this XElement element, XmlNames name) => GetAttributeBooleanValue(element, GetLocalXName(name));

        public static bool GetAttributeBooleanValue(this XElement element, XName name!!, bool defaultValue) => element.TryGetAttributeValue(name, out bool result) ? result : defaultValue;

        public static bool GetAttributeBooleanValue(this XElement element, XmlNames name, bool defaultValue) => element.TryGetAttributeValue(GetLocalXName(name), out bool result) ? result : defaultValue;

        public static DateTime? GetAttributeDateTimeValue(this XElement element, XName name!!, XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            element.TryGetAttributeValue(name, mode, out DateTime result) ? result : null;

        public static DateTime? GetAttributeDateTimeValue(this XElement element, XmlNames name, XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            GetAttributeDateTimeValue(element, GetLocalXName(name), mode);

        public static TimeSpan? GetAttributeTimeSpanValue(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out TimeSpan result) ? result : null;

        public static TimeSpan? GetAttributeTimeSpanValue(this XElement element, XmlNames name) => GetAttributeTimeSpanValue(element, GetLocalXName(name));

        public static Guid? GetAttributeGuidValue(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out Guid result) ? result : null;

        public static Guid? GetAttributeGuidValue(this XElement element, XmlNames name) => GetAttributeGuidValue(element, GetLocalXName(name));

        public static byte? GetAttributeByteValue(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out byte result) ? result : null;

        public static byte? GetAttributeByteValue(this XElement element, XmlNames name) => GetAttributeByteValue(element, GetLocalXName(name));

        public static sbyte? GetAttributeSByteValue(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out sbyte result) ? result : null;

        public static sbyte? GetAttributeSByteValue(this XElement element, XmlNames name) => GetAttributeSByteValue(element, GetLocalXName(name));

        public static short? GetAttributeInt16Value(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out short result) ? result : null;

        public static short? GetAttributeInt16Value(this XElement element, XmlNames name) => GetAttributeInt16Value(element, GetLocalXName(name));

        public static ushort? GetAttributeUInt16Value(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out ushort result) ? result : null;

        public static ushort? GetAttributeUInt16Value(this XElement element, XmlNames name) => GetAttributeUInt16Value(element, GetLocalXName(name));

        public static int? GetAttributeInt32Value(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out int result) ? result : null;

        public static int? GetAttributeInt32Value(this XElement element, XmlNames name) => GetAttributeInt32Value(element, GetLocalXName(name));

        public static uint? GetAttributeUInt32Value(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out uint result) ? result : null;

        public static uint? GetAttributeUInt32Value(this XElement element, XmlNames name) => GetAttributeUInt32Value(element, GetLocalXName(name));

        public static long? GetAttributeInt64Value(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out long result) ? result : null;

        public static long? GetAttributeInt64Value(this XElement element, XmlNames name) => GetAttributeInt64Value(element, GetLocalXName(name));

        public static ulong? GetAttributeUInt64Value(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out ulong result) ? result : null;

        public static ulong? GetAttributeUInt64Value(this XElement element, XmlNames name) => GetAttributeUInt64Value(element, GetLocalXName(name));

        public static float? GetAttributeSingleValue(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out float result) ? result : null;

        public static float? GetAttributeSingleValue(this XElement element, XmlNames name) => GetAttributeSingleValue(element, GetLocalXName(name));

        public static double? GetAttributeDoubleValue(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out double result) ? result : null;

        public static double? GetAttributeDoubleValue(this XElement element, XmlNames name) => GetAttributeDoubleValue(element, GetLocalXName(name));

        public static decimal? GetAttributeDecimalValue(this XElement element, XName name!!) => element.TryGetAttributeValue(name, out decimal result) ? result : null;

        public static decimal? GetAttributeDecimalValue(this XElement element, XmlNames name) => GetAttributeDecimalValue(element, GetLocalXName(name));

        #endregion

        #region TryGetAttributeValue

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out string result)
        {
            XAttribute attribute = element?.Attribute(GetLocalXName(name));
            if (attribute is null)
            {
                result = default;
                return false;
            }
            result = attribute.Value;
            return true;
        }

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out string result)
        {
            XAttribute attribute;
            if (element is null || (attribute = element?.Attribute(name)) is null)
            {
                result = default;
                return false;
            }
            result = attribute.Value;
            return true;
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out bool result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out bool result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out DateTime result) =>
            TryGetAttributeValue(element, name, XmlDateTimeSerializationMode.RoundtripKind, out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out DateTime result) =>
            TryGetAttributeValue(element, name, XmlDateTimeSerializationMode.RoundtripKind, out result);

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, XmlDateTimeSerializationMode mode, out DateTime result) =>
            (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(mode, out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, XmlDateTimeSerializationMode mode, out DateTime result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(mode, out result);
        }


        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out TimeSpan result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out TimeSpan result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out Guid result) => (element?.Attribute(name)?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out Guid result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out byte result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out byte result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out sbyte result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out sbyte result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out short result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out short result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out ushort result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out ushort result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out int result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out int result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out uint result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out uint result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out long result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out long result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out ulong result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out ulong result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out float result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out float result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out double result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out double result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out decimal result) => (element?.Attribute(GetLocalXName(name))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name!!, out decimal result)
        {
            if (element is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        #endregion

        #region Attributes

        public static IEnumerable<XAttribute> Attributes(this XElement element, XmlNames name) => element?.Attributes(GetLocalXName(name)) ?? Enumerable.Empty<XAttribute>();

        public static IEnumerable<XAttribute> Attributes(this IEnumerable<XElement> source, XmlNames name) => source?.Attributes(GetLocalXName(name)) ?? Enumerable.Empty<XAttribute>();

        #endregion

        #region Descendants

        public static IEnumerable<XElement> Descendants(this XContainer container, XmlNames name) => container?.Descendants(GetModelXName(name)) ?? Enumerable.Empty<XElement>();

        public static IEnumerable<XElement> Descendants<T>(this IEnumerable<T> source, XmlNames name) where T : XContainer => source?.Descendants(GetModelXName(name)) ?? Enumerable.Empty<XElement>();

        public static IEnumerable<XElement> DescendantsAndSelf(this IEnumerable<XElement> source, XmlNames name) => source?.DescendantsAndSelf(GetModelXName(name)) ?? Enumerable.Empty<XElement>();

        #endregion

        #region Elements

        public static XElement Element(this XContainer container, XmlNames name) => container?.Element(GetModelXName(name));

        public static IEnumerable<XElement> Elements(this XContainer container, XmlNames name) => container?.Elements(GetModelXName(name)) ?? Enumerable.Empty<XElement>();

        public static IEnumerable<XElement> Elements(this IEnumerable<XElement> source, XmlNames name) => source?.Elements(GetModelXName(name)) ?? Enumerable.Empty<XElement>();

        #endregion

        #region ElementsExcept

        public static IEnumerable<XElement> ElementsExcept(this XContainer container, XName name!!) => container?.Elements().Where(e => !name.Equals(e.Name)) ?? Enumerable.Empty<XElement>();

        public static IEnumerable<XElement> ElementsExcept(this XContainer container, XmlNames name) => ElementsExcept(container, GetModelXName(name));

        public static IEnumerable<XElement> ElementsExcept(this IEnumerable<XElement> source, XName name!!) => source?.Elements().Where(e => !name.Equals(e.Name)) ?? Enumerable.Empty<XElement>();

        public static IEnumerable<XElement> ElementsExcept(this IEnumerable<XElement> source, XmlNames name) => ElementsExcept(source, GetModelXName(name));

        #endregion

        #region ElementsWithAttribute

        #region string

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (value is null)
                return container.Elements().Where(element => element.Attribute(attributeName) is null);
            if (comparer is null) comparer = StringComparer.CurrentCulture;
            return container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out string result) && comparer.Equals(result, value));
        }

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (value is null)
                return source.Elements().Where(element => element.Attribute(attributeName) is null);
            if (comparer is null) comparer = StringComparer.CurrentCulture;
            return source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out string result) && comparer.Equals(result, value));
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, string value, IEqualityComparer<string> comparer = null) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value, comparer);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value, comparer);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, string value, IEqualityComparer<string> comparer = null) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value, comparer);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value, comparer);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (value is null)
                return container.Elements(elementName).Where(element => element.Attribute(attributeName) is null);
            if (comparer is null) comparer = StringComparer.CurrentCulture;
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out string result) && comparer.Equals(result, value));
        }

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (value is null)
                return source.Elements(elementName).Where(element => element.Attribute(attributeName) is null);
            if (comparer is null) comparer = StringComparer.CurrentCulture;
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out string result) && comparer.Equals(result, value));
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, string value, IEqualityComparer<string> comparer = null) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value, comparer);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, string value, IEqualityComparer<string> comparer = null) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value, comparer);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value, comparer);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value, comparer);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, string value, IEqualityComparer<string> comparer = null) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value, comparer);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, string value, IEqualityComparer<string> comparer = null) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value, comparer);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value, comparer);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value, comparer);

        #endregion

        #region bool

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(this XContainer container, XName attributeName, bool value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out bool result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName, bool value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out bool result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, bool value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, bool value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, bool value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(this XContainer container, XName elementName, XName attributeName, bool value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out bool result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName, XName attributeName, bool value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out bool result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, bool value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, bool value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region DateTime

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(this XContainer container, XName attributeName, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, mode, out DateTime result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, mode, out DateTime result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value, mode);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value, mode);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(this XContainer container, XName elementName, XName attributeName, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, mode, out DateTime result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName, XName attributeName, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, mode, out DateTime result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value, mode);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value, mode);

        #endregion

        #region TimeSpan

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, TimeSpan value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out TimeSpan result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, TimeSpan value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out TimeSpan result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, TimeSpan value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, TimeSpan value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, TimeSpan value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, TimeSpan value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, TimeSpan value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out TimeSpan result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, TimeSpan value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out TimeSpan result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, TimeSpan value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, TimeSpan value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, TimeSpan value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, TimeSpan value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, TimeSpan value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, TimeSpan value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, TimeSpan value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, TimeSpan value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region Guid

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, Guid value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out Guid result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, Guid value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out Guid result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, Guid value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, Guid value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, Guid value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, Guid value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, Guid value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out Guid result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, Guid value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out Guid result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, Guid value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, Guid value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, Guid value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, Guid value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, Guid value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, Guid value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, Guid value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, Guid value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region byte

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, byte value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out byte result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, byte value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out byte result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, byte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, byte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, byte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, byte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, byte value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out byte result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, byte value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out byte result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, byte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, byte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, byte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, byte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, byte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, byte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, byte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, byte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region sbyte

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, sbyte value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out sbyte result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, sbyte value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out sbyte result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, sbyte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, sbyte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, sbyte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, sbyte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, sbyte value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out sbyte result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, sbyte value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out sbyte result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, sbyte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, sbyte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, sbyte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, sbyte value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, sbyte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, sbyte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, sbyte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, sbyte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region short

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, short value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out short result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, short value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out short result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, short value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, short value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, short value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, short value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, short value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out short result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, short value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out short result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, short value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, short value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, short value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, short value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, short value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, short value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, short value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, short value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region ushort

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, ushort value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out ushort result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, ushort value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out ushort result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, ushort value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, ushort value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, ushort value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, ushort value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, ushort value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out ushort result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, ushort value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out ushort result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, ushort value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, ushort value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, ushort value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, ushort value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, ushort value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, ushort value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, ushort value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, ushort value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region int

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, int value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out int result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, int value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out int result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, int value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, int value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, int value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, int value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, int value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out int result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, int value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out int result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, int value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, int value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, int value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, int value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, int value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, int value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, int value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, int value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region uint

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, uint value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out uint result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, uint value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out uint result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, uint value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, uint value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, uint value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, uint value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, uint value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out uint result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, uint value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out uint result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, uint value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, uint value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, uint value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, uint value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, uint value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, uint value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, uint value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, uint value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region long

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, long value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out long result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, long value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out long result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, long value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, long value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, long value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, long value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, long value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out long result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, long value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out long result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, long value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, long value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, long value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, long value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, long value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, long value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, long value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, long value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region ulong

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, ulong value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out ulong result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, ulong value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out ulong result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, ulong value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, ulong value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, ulong value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, ulong value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, ulong value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out ulong result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, ulong value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out ulong result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, ulong value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, ulong value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, ulong value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, ulong value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, ulong value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, ulong value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, ulong value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, ulong value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region float

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, float value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out float result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, float value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out float result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, float value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, float value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, float value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, float value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, float value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out float result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, float value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out float result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, float value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, float value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, float value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, float value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, float value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, float value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, float value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, float value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region double

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, double value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out double result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, double value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out double result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, double value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, double value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, double value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, double value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, double value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out double result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, double value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out double result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, double value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, double value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, double value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, double value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, double value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, double value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, double value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, double value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #region decimal

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName attributeName, decimal value) =>
            container.Elements().Where(element => element.TryGetAttributeValue(attributeName, out decimal result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName attributeName, decimal value) =>
            source.Elements().Where(element => element.TryGetAttributeValue(attributeName, out decimal result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName attributeName!!, decimal value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, decimal value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName attributeName!!, decimal value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, decimal value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(attributeName), value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(XContainer container, XName elementName, XName attributeName, decimal value) =>
            container.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out decimal result) && result == value);

        private static IEnumerable<XElement> Backing_ElementsWithAttribute(IEnumerable<XElement> source, XName elementName, XName attributeName, decimal value) =>
            source.Elements(elementName).Where(element => element.TryGetAttributeValue(attributeName, out decimal result) && result == value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XName attributeName!!, decimal value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XName attributeName!!, decimal value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XName elementName!!, XmlNames attributeName, decimal value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, decimal value) =>
            (container is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(container, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XName attributeName!!, decimal value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XName attributeName!!, decimal value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), attributeName, value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XName elementName!!, XmlNames attributeName, decimal value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, elementName, GetLocalXName(attributeName), value);

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, decimal value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_ElementsWithAttribute(source, GetLocalXName(elementName), GetLocalXName(attributeName), value);

        #endregion

        #endregion

        #region WithAttribute

        #region string

        private static IEnumerable<XElement> Backing_WithAttribute(this IEnumerable<XElement> source, XName attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (value is null)
                source.Where(element => element.Attribute(attributeName) is null);
            if (comparer is null) comparer = StringComparer.CurrentCulture;
            return source.Where(element => element.TryGetAttributeValue(attributeName, out string result) && comparer.Equals(result, value));
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, string value, IEqualityComparer<string> comparer = null) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value, comparer);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value, comparer);

        #endregion

        #region bool

        private static IEnumerable<XElement> Backing_WithAttribute(this IEnumerable<XElement> source, XName attributeName, bool value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out bool result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, bool value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, bool value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region DateTime

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, mode, out DateTime result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value, mode);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, DateTime value,
                XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value, mode);

        #endregion

        #region TimeSpan

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, TimeSpan value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out TimeSpan result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, TimeSpan value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, TimeSpan value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region Guid

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, Guid value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out Guid result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, Guid value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, Guid value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region byte

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, byte value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out byte result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, byte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, byte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region sbyte

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, sbyte value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out sbyte result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, sbyte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, sbyte value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region short

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, short value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out short result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, short value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, short value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region ushort

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, ushort value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out ushort result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, ushort value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, ushort value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region int

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, int value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out int result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, int value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, int value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region uint

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, uint value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out uint result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, uint value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, uint value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region long

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, long value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out long result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, long value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, long value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region ulong

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, ulong value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out ulong result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, ulong value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, ulong value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region float

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, float value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out float result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, float value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, float value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region double

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, double value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out double result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, double value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, double value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #region decimal

        private static IEnumerable<XElement> Backing_WithAttribute(IEnumerable<XElement> source, XName attributeName, decimal value) =>
            source.Where(element => element.TryGetAttributeValue(attributeName, out decimal result) && result == value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XName attributeName!!, decimal value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, attributeName, value);

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, decimal value) =>
            (source is null) ? Enumerable.Empty<XElement>() : Backing_WithAttribute(source, GetLocalXName(attributeName), value);

        #endregion

        #endregion

        #region TryConvertFromXml

        public static bool TryConvertFromXml(this string value, out bool result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToBoolean(value); }
            catch
            {
                result = false;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out DateTime result) => TryConvertFromXml(value, XmlDateTimeSerializationMode.RoundtripKind, out result);

        public static bool TryConvertFromXml(this string value, XmlDateTimeSerializationMode mode, out DateTime result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToDateTime(value, mode); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out TimeSpan result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToTimeSpan(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out Guid result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToGuid(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out byte result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToByte(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out sbyte result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToSByte(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out short result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToInt16(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out ushort result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToUInt16(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out int result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToInt32(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out uint result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToUInt32(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out long result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToInt64(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out ulong result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToUInt64(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out float result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToSingle(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out double result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToDouble(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        public static bool TryConvertFromXml(this string value, out decimal result)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result = default;
                return false;
            }
            try { result = XmlConvert.ToDecimal(value); }
            catch
            {
                result = default;
                return false;
            }
            return true;
        }

        #endregion
    }
}
