using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace FsInfoCat.Generator
{
    public static class ExtensionMethods
    {
        public const string ModelNamespaceURI = "";

        public static readonly XNamespace ModelNamespace = XNamespace.Get(ModelNamespaceURI);

        public static XName GetXName(this XmlNames name) => ModelNamespace.GetName(name.ToString("F"));

        public static IEnumerable<XElement> Ancestors<T>(this IEnumerable<T> source, XmlNames name) where T : XNode => source.Ancestors(GetXName(name));

        public static IEnumerable<XElement> AncestorsAndSelf(this IEnumerable<XElement> source, XmlNames name) => source.AncestorsAndSelf(GetXName(name));

        public static XAttribute Attribute(this XElement element, XmlNames name) => element?.Attribute(name.ToString("F"));

        #region GetAttributeValue

        public static string GetAttributeValue(this XElement element, XmlNames name) => element?.Attribute(name.ToString("F"))?.Value;

        public static bool? GetAttributeBooleanValue(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out bool result) ? (bool?)result : null;

        public static DateTime? GetAttributeDateTimeValue(this XElement element, XmlNames name, XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind) =>
            element.TryGetAttributeValue(name, mode, out DateTime result) ? (DateTime?)result : null;

        public static TimeSpan? GetAttributeTimeSpanValue(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out TimeSpan result) ? (TimeSpan?)result : null;

        public static Guid? GetAttributeGuidValue(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out Guid result) ? (Guid?)result : null;

        public static byte? GetAttributeByteValue(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out byte result) ? (byte?)result : null;

        public static sbyte? GetAttributeSByteValue(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out sbyte result) ? (sbyte?)result : null;

        public static short? GetAttributeInt16Value(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out short result) ? (short?)result : null;

        public static ushort? GetAttributeUInt16Value(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out ushort result) ? (ushort?)result : null;

        public static int? GetAttributeInt32Value(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out int result) ? (short?)result : null;

        public static uint? GetAttributeUInt32Value(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out uint result) ? (uint?)result : null;

        public static long? GetAttributeInt64Value(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out long result) ? (long?)result : null;

        public static ulong? GetAttributeUInt64Value(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out ulong result) ? (ulong?)result : null;

        public static float? GetAttributeSingleValue(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out float result) ? (float?)result : null;

        public static double? GetAttributeDoubleValue(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out double result) ? (double?)result : null;

        public static decimal? GetAttributeDecimalValue(this XElement element, XmlNames name) => element.TryGetAttributeValue(name, out decimal result) ? (decimal?)result : null;

        #endregion

        #region TryGetAttributeValue

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out string result)
        {
            XAttribute attribute = element?.Attribute(name.ToString("F"));
            if (attribute is null)
            {
                result = default;
                return false;
            }
            result = attribute.Value;
            return true;
        }

        public static bool TryGetAttributeValue(this XElement element, XName name, out string result)
        {
            XAttribute attribute;
            if (name is null || element is null || (attribute = element?.Attribute(name)) is null)
            {
                result = default;
                return false;
            }
            result = attribute.Value;
            return true;
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out bool result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out bool result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out DateTime result) =>
            TryGetAttributeValue(element, name, XmlDateTimeSerializationMode.RoundtripKind, out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out DateTime result) =>
            TryGetAttributeValue(element, name, XmlDateTimeSerializationMode.RoundtripKind, out result);

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, XmlDateTimeSerializationMode mode, out DateTime result) =>
            (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(mode, out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, XmlDateTimeSerializationMode mode, out DateTime result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }


        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out TimeSpan result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out TimeSpan result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out Guid result) => (element?.Attribute(name)?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out Guid result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out byte result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out byte result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out sbyte result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out sbyte result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out short result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out short result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out ushort result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out ushort result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out int result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out int result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out uint result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out uint result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out long result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out long result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out ulong result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out ulong result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out float result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out float result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out double result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out double result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        public static bool TryGetAttributeValue(this XElement element, XmlNames name, out decimal result) => (element?.Attribute(name.ToString("F"))?.Value).TryConvertFromXml(out result);

        public static bool TryGetAttributeValue(this XElement element, XName name, out decimal result)
        {
            if (element is null || name is null)
            {
                result = default;
                return false;
            }
            return (element.Attribute(name)?.Value).TryConvertFromXml(out result);
        }

        #endregion

        #region Attributes

        public static IEnumerable<XAttribute> Attributes(this XElement element, XmlNames name) => element?.Attributes(name.ToString("F")) ?? Enumerable.Empty<XAttribute>();

        public static IEnumerable<XAttribute> Attributes(this IEnumerable<XElement> source, XmlNames name) => source?.Attributes(name.ToString("F")) ?? Enumerable.Empty<XAttribute>();

        #endregion

        #region Descendants

        public static IEnumerable<XElement> Descendants(this XContainer container, XmlNames name) => container?.Descendants(GetXName(name)) ?? Enumerable.Empty<XElement>();

        public static IEnumerable<XElement> Descendants<T>(this IEnumerable<T> source, XmlNames name) where T : XContainer => source?.Descendants(GetXName(name)) ?? Enumerable.Empty<XElement>();

        public static IEnumerable<XElement> DescendantsAndSelf(this IEnumerable<XElement> source, XmlNames name) => source?.DescendantsAndSelf(GetXName(name)) ?? Enumerable.Empty<XElement>();

        #endregion

        #region Elements

        public static XElement Element(this XContainer container, XmlNames name) => container?.Element(GetXName(name));

        public static IEnumerable<XElement> Elements(this XContainer container, XmlNames name) => container?.Elements(GetXName(name)) ?? Enumerable.Empty<XElement>();

        public static IEnumerable<XElement> Elements(this IEnumerable<XElement> source, XmlNames name) => source?.Elements(GetXName(name)) ?? Enumerable.Empty<XElement>();

        #endregion

        #region ElementsWithAttribute

        #region string

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            if (value is null)
                container.Elements().Where(element => element.Attribute(attributeName) is null);
            if (comparer is null) comparer = StringComparer.CurrentCulture;
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out string result) && comparer.Equals(result, value));
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            if (value is null)
                source.Elements().Where(element => element.Attribute(attributeName) is null);
            if (comparer is null) comparer = StringComparer.CurrentCulture;
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out string result) && comparer.Equals(result, value));
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            if (value is null)
                container.Elements(GetXName(elementName)).Where(element => element.Attribute(attributeName) is null);
            if (comparer is null) comparer = StringComparer.CurrentCulture;
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out string result) && comparer.Equals(result, value));
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            if (value is null)
                source.Elements(GetXName(elementName)).Where(element => element.Attribute(attributeName) is null);
            if (comparer is null) comparer = StringComparer.CurrentCulture;
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out string result) && comparer.Equals(result, value));
        }

        #endregion

        #region bool

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, bool value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out bool result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, bool value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out bool result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, bool value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out bool result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, bool value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out bool result) && result == value);
        }

        #endregion

        #region DateTime

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, DateTime value, XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, mode, out DateTime result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, DateTime value, XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(attributeName, mode, out DateTime result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, DateTime value, XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, mode, out DateTime result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, DateTime value, XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, mode, out DateTime result) && result == value);
        }

        #endregion

        #region TimeSpan

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, TimeSpan value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out TimeSpan result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, TimeSpan value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out TimeSpan result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, TimeSpan value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out TimeSpan result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, TimeSpan value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out TimeSpan result) && result == value);
        }

        #endregion

        #region Guid

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, Guid value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out Guid result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, Guid value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out Guid result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, Guid value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out Guid result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, Guid value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out Guid result) && result == value);
        }

        #endregion

        #region byte

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, byte value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out byte result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, byte value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out byte result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, byte value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out byte result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, byte value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out byte result) && result == value);
        }

        #endregion

        #region sbyte

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, sbyte value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out sbyte result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, sbyte value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out sbyte result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, sbyte value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out sbyte result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, sbyte value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out sbyte result) && result == value);
        }

        #endregion

        #region short

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, short value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out short result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, short value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out short result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, short value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out short result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, short value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out short result) && result == value);
        }

        #endregion

        #region ushort

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, ushort value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out ushort result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, ushort value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out ushort result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, ushort value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out ushort result) && result == value);
        }


        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, ushort value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out ushort result) && result == value);
        }


        #endregion

        #region int

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, int value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out int result) && result == value);
        }


        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, int value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out int result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, int value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out int result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, int value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out int result) && result == value);
        }

        #endregion

        #region uint

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, uint value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out uint result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, uint value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out uint result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, uint value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out uint result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, uint value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out uint result) && result == value);
        }

        #endregion

        #region long

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, long value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out long result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, long value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out long result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, long value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out long result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, long value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out long result) && result == value);
        }

        #endregion

        #region ulong

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, ulong value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out ulong result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, ulong value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out ulong result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, ulong value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out ulong result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, ulong value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out ulong result) && result == value);
        }

        #endregion

        #region float

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, float value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out float result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, float value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out float result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, float value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out float result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, float value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out float result) && result == value);
        }

        #endregion

        #region double

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, double value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out double result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, double value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out double result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, double value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out double result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, double value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out double result) && result == value);
        }

        #endregion

        #region decimal

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames attributeName, decimal value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements().Where(element => element.TryGetAttributeValue(xName, out decimal result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, decimal value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements().Where(element => element.TryGetAttributeValue(xName, out decimal result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this XContainer container, XmlNames elementName, XmlNames attributeName, decimal value)
        {
            if (container is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return container.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out decimal result) && result == value);
        }

        public static IEnumerable<XElement> ElementsWithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, decimal value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out decimal result) && result == value);
        }

        #endregion

        #endregion

        #region WithAttribute

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            if (value is null)
                source.Where(element => element.Attribute(attributeName) is null);
            if (comparer is null) comparer = StringComparer.CurrentCulture;
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out string result) && comparer.Equals(result, value));
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames elementName, XmlNames attributeName, bool value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Elements(elementName).Where(element => element.TryGetAttributeValue(xName, out bool result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, DateTime value, XmlDateTimeSerializationMode mode = XmlDateTimeSerializationMode.RoundtripKind)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(attributeName, mode, out DateTime result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, TimeSpan value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out TimeSpan result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, Guid value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out Guid result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, byte value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out byte result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, sbyte value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out sbyte result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, short value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out short result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, ushort value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out ushort result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, int value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out int result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, uint value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out uint result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, long value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out long result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, ulong value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out ulong result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, float value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out float result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, double value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out double result) && result == value);
        }

        public static IEnumerable<XElement> WithAttribute(this IEnumerable<XElement> source, XmlNames attributeName, decimal value)
        {
            if (source is null) return Enumerable.Empty<XElement>();
            XName xName = XName.Get(attributeName.ToString("F"));
            return source.Where(element => element.TryGetAttributeValue(xName, out decimal result) && result == value);
        }

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

        public static TextSpan ToTextSpan(this string text, LinePosition start, LinePosition end)
        {
            if (start < end) throw new ArgumentException($"{nameof(start)} must be less than {nameof(end)} {nameof(LinePosition)}", nameof(start));
            char[] lineBreakChars = new[]{ '\r', '\n' };
            int startIndex = 0, lineIndex = 0;
            do
            {
                int nextIndex = text.IndexOfAny(lineBreakChars, startIndex);
                if (nextIndex < 0 || (startIndex = nextIndex + 1) == text.Length || (text[nextIndex] == '\r' && text[startIndex] == '\n' && ++startIndex == text.Length))
                {
                    if (++lineIndex < start.Line) throw new ArgumentOutOfRangeException(nameof(start));
                    break;
                }
            } while (++lineIndex < start.Line);
            int endIndex = startIndex;
            if ((startIndex += start.Character) >= text.Length) throw new ArgumentOutOfRangeException(nameof(start));
            while (lineIndex < end.Line)
            {
                lineIndex++;
                int nextIndex = text.IndexOfAny(lineBreakChars, endIndex);
                if (nextIndex < 0 || (endIndex = nextIndex + 1) == text.Length || (text[nextIndex] == '\r' && text[endIndex] == '\n' && ++endIndex == text.Length))
                {
                    if (lineIndex < end.Line) throw new ArgumentOutOfRangeException(nameof(end));
                    break;
                }
            }
            if ((endIndex += start.Character) > text.Length) throw new ArgumentOutOfRangeException(nameof(end));
            return TextSpan.FromBounds(startIndex, endIndex);
        }

        public static bool TryGetLocation(this XmlException exception, string text, string filePath, out Location result)
        {
            if(text is null || exception is null || exception.LineNumber < 1 || exception.LinePosition < 1)
            {
                result = default;
                return false;
            }
            LinePosition startPosition = new LinePosition(exception.LineNumber - 1, exception.LinePosition - 1);
            LinePositionSpan positionSpan = new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1));
            result = Location.Create(filePath, ToTextSpan(text, positionSpan.Start, positionSpan.End), positionSpan);
            return true;
        }

        public static Location GetLocation(this XmlException exception, string text, string filePath, out LinePosition startPosition)
        {
            if (text is null || exception is null)
                startPosition = new LinePosition(0, 0);
            else
                startPosition = new LinePosition(Math.Max(1, exception.LineNumber) - 1, Math.Max(1, exception.LinePosition) - 1);
            LinePositionSpan positionSpan = new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1));
            return Location.Create(filePath, ToTextSpan(text, positionSpan.Start, positionSpan.End), positionSpan);
        }

        public static LinePositionSpan GetPositionSpan(this XmlSchemaException exception, TextLineCollection lines, out TextSpan textSpan, out string message)
        {
            if (exception is null)
            {
                textSpan = TextSpan.FromBounds(0, 1);
                message = null;
                return new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 1));
            }
            int lineIndex = Math.Max(1, exception.LineNumber) - 1;
            int colIndex = Math.Max(1, exception.LinePosition) - 1;
            TextLine textLine = lines.FirstOrDefault(l => l.LineNumber == lineIndex);
            LinePosition startPosition = new LinePosition(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            textSpan = TextSpan.FromBounds(start, start + 1);
            message = string.IsNullOrWhiteSpace(exception?.Message) ? null : exception.Message;
            return new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1));
        }

        public static LinePositionSpan GetPositionSpan(this XmlException exception, TextLineCollection lines, out TextSpan textSpan, out string message)
        {
            if (exception is null)
            {
                textSpan = TextSpan.FromBounds(0, 1);
                message = null;
                return new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 1));
            }
            int lineIndex = Math.Max(1, exception.LineNumber) - 1;
            int colIndex = Math.Max(1, exception.LinePosition) - 1;
            TextLine textLine = lines.FirstOrDefault(l => l.LineNumber == lineIndex);
            LinePosition startPosition = new LinePosition(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            textSpan = TextSpan.FromBounds(start, start + 1);
            message = string.IsNullOrWhiteSpace(exception?.Message) ? null : exception.Message;
            return new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1));
        }

        public static LinePositionSpan GetPositionSpan(this IXmlLineInfo obj, TextLineCollection lines, out TextSpan textSpan)
        {
            if (obj is null)
            {
                textSpan = TextSpan.FromBounds(0, 1);
                return new LinePositionSpan(new LinePosition(0, 0), new LinePosition(0, 1));
            }
            int lineIndex = Math.Max(1, obj.LineNumber) - 1;
            int colIndex = Math.Max(1, obj.LinePosition) - 1;
            TextLine textLine = lines.FirstOrDefault(l => l.LineNumber == lineIndex);
            LinePosition startPosition = new LinePosition(lineIndex, colIndex);
            int start = textLine.Span.Start + colIndex;
            textSpan = TextSpan.FromBounds(start, start + 1);
            return new LinePositionSpan(startPosition, new LinePosition(startPosition.Line, startPosition.Character + 1));
        }
    }
}
