using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using static CodeGeneration.Constants;

namespace CodeGeneration
{
    public static class XLinqExtensions
    {
        #region Generic XLinq Extensions

        public static TValue GetAnnotatedCacheValue<TCache, TValue>(this XElement target, Func<TCache> ifNotExist)
            where TCache : class, IElementCacheItem<TValue>
        {
            if (target is null)
                return default;
            TCache cacheItem = target.Annotation<TCache>();
            if (cacheItem is null)
            {
                cacheItem = ifNotExist();
                target.AddAnnotation(cacheItem);
            }
            return cacheItem.Value;
        }

        public static IEnumerable<XElement> GetElementsByNames(this XElement source, params XName[] names)
        {
            if (names is null || names.Length == 0)
                return source.Elements();
            if (names.Length > 1)
                return source.Elements().Where(e => names.Any(n => e.Name == n));
            return source.Elements(names[0]);
        }

        public static IEnumerable<XElement> GetElementsByNames(this IEnumerable<XElement> source, params XName[] names)
        {
            if (names is null || names.Length == 0)
                return source.Elements();
            if (names.Length > 1)
                return source.Elements().Where(e => names.Any(n => e.Name == n));
            return source.Elements(names[0]);
        }

        public static IEnumerable<XElement> GetElementsByAttributeValue(this IEnumerable<XElement> source, XName name, string value) => (value is null || source is null || !source.Any()) ?
            Enumerable.Empty<XElement>() : source.Attributes(name).Where(a => a.Value == value).Select(a => a.Parent);

        public static IEnumerable<XElement> GetElementsByAttributeValue(this IEnumerable<XElement> source, XName name, IEnumerable<string> values) =>
            (values is null || source is null || !values.Any() || !source.Any()) ? Enumerable.Empty<XElement>() :
            values.Distinct().SelectMany(v => source.Attributes(name).Where(a => a.Value == v)).Select(a => a.Parent);

        public static XText ToWhiteSpaceNormalized(this XText source)
        {
            string text = source.Value;
            if (NormalizeNewLineRegex.IsMatch(text))
                text = NormalizeNewLineRegex.Replace(text, Environment.NewLine);
            if (TrimOuterBlankLinesRegex.IsMatch(text))
                text = TrimOuterBlankLinesRegex.Replace(text, Environment.NewLine);
            if (NormalizeWsRegex.IsMatch(text))
                text = NormalizeWsRegex.Replace(text, " ");
            return new XText(StripWsRegex.IsMatch(text) ? StripWsRegex.Replace(text, "") : text);
        }

        public static XElement WsNormalizedWithoutElementNamespace(this XElement sourceParent)
        {
            if (sourceParent.Name.LocalName == XNAME_langword)
                return new XElement(XNamespace.None.GetName(XNAME_see), new XAttribute(XNAME_langword, sourceParent.Value));
            XElement resultElement = new(XNamespace.None.GetName(sourceParent.Name.LocalName));
            foreach (XAttribute attribute in sourceParent.Attributes())
                resultElement.Add(new XAttribute(attribute));
            XNode sourceNode = sourceParent.Nodes().FirstOrDefault(n => n is XElement || n is XText t && !string.IsNullOrWhiteSpace(t.Value));
            if (sourceNode is null)
                return resultElement;
            if (sourceNode is XText text)
                resultElement.Add(ToWhiteSpaceNormalized(text));
            else
                resultElement.Add(WsNormalizedWithoutElementNamespace((XElement)sourceNode));
            while ((sourceNode = sourceNode.NodesAfterSelf().FirstOrDefault(n => n is XElement || n is XText t && (!string.IsNullOrWhiteSpace(t.Value) || (t.NextNode is not null && t.NextNode is not XText)))) is not null)
            {
                if (sourceNode is XText xText)
                    resultElement.Add(ToWhiteSpaceNormalized(xText));
                else
                    resultElement.Add(WsNormalizedWithoutElementNamespace((XElement)sourceNode));
            }
            return resultElement;
        }

        public static XElement WithoutElementNamespace(this XElement sourceParent)
        {
            if (sourceParent.Name.LocalName == XNAME_langword)
                return new XElement(XNamespace.None.GetName(XNAME_see), new XAttribute(XNAME_langword, sourceParent.Value));
            XElement resultElement = new(XNamespace.None.GetName(sourceParent.Name.LocalName));
            foreach (XAttribute attribute in sourceParent.Attributes())
                resultElement.Add(new XAttribute(attribute));
            XNode sourceNode = sourceParent.Nodes().FirstOrDefault(n => n is XElement || n is XText t && !string.IsNullOrWhiteSpace(t.Value));
            if (sourceNode is null)
                return resultElement;
            if (sourceNode is XText text)
                resultElement.Add(new XText(text));
            else
                resultElement.Add(WithoutElementNamespace((XElement)sourceNode));
            while ((sourceNode = sourceNode.NodesAfterSelf().FirstOrDefault(n => n is XElement || n is XText t && (!string.IsNullOrWhiteSpace(t.Value) || (t.NextNode is not null && t.NextNode is not XText)))) is not null)
            {
                if (sourceNode is XText xText)
                    resultElement.Add(new XText(xText));
                else
                    resultElement.Add(WithoutElementNamespace((XElement)sourceNode));
            }
            return resultElement;
        }

        #endregion

        #region XML value parsing Methods

        static T? ToValue<T>(string text, Func<string, T> converter)
            where T : struct
        {
            if (text is not null && (text = text.Trim()).Length > 0)
                try { return converter(text); } catch { return null; }
            return null;
        }

        public static T? ElementToValue<T>(this XElement element, XName name, Func<string, T> converter)
            where T : struct => ((element = element?.Element(name)) is null || element.IsEmpty) ? null : ToValue(element.Value, converter);

        public static T? AttributeToValue<T>(this XElement element, XName name, Func<string, T> converter)
            where T : struct => ToValue(element?.Attribute(name)?.Value, converter);

        public static T? ToFirstValue<T>(IEnumerable<string> values, Func<string, T> converter)
            where T : struct
        {
            foreach (string s in values.Where(s => s is not null).Select(s => s.Trim()))
            {
                if (s.Length > 0)
                    try { return converter(s); } catch { /* okay to ignore */ }
            }
            return null;
        }

        public static T? ToFirstElementValue<T>(this IEnumerable<XElement> sourceElements, XName name, Func<string, T> converter)
            where T : struct => ToFirstValue(sourceElements.Elements(name).Select(e => (e is null || e.IsEmpty) ? null : e.Value), converter);

        public static T? ToFirstAttributeValue<T>(this IEnumerable<XElement> sourceElements, XName name, Func<string, T> converter)
            where T : struct => ToFirstValue(sourceElements.Attributes(name).Select(a => a?.Value), converter);

        public static T ToFirstObject<T>(IEnumerable<string> values, Func<string, T> converter)
            where T : class
        {
            foreach (string s in values.Where(s => s is not null).Select(s => s.Trim()))
            {
                if (s.Length > 0)
                    try
                    {
                        T result = converter(s);
                        if (result is not null)
                            return result;
                    }
                    catch { /* okay to ignore */ }
            }
            return null;
        }

        public static T ToFirstElementObject<T>(this IEnumerable<XElement> sourceElements, XName name, Func<string, T> converter)
            where T : class => ToFirstObject(sourceElements.Elements(name).Select(e => (e is null || e.IsEmpty) ? null : e.Value), converter);

        public static T ToFirstAttributeObject<T>(this IEnumerable<XElement> sourceElements, XName name, Func<string, T> converter)
            where T : class => ToFirstObject(sourceElements.Attributes(name).Select(a => a?.Value), converter);

        public static T ToObject<T>(string text, Func<string, T> converter)
            where T : class
        {
            if (text is not null && (text = text.Trim()).Length > 0)
                try { return converter(text); } catch { return null; }
            return null;
        }

        public static string ElementToString(this IEnumerable<XElement> sourceElements, XName name) => sourceElements?.Elements(name).Where(e => !e.IsEmpty)
            .Select(e => e.Value).FirstOrDefault();

        public static string ElementToString(this XElement element, XName name) => element?.Elements(name).Where(e => !e.IsEmpty)
            .Select(e => e.Value).FirstOrDefault();

        public static string AttributeToString(this IEnumerable<XElement> sourceElements, XName name) => sourceElements?.Attributes(name)
            .Select(e => e.Value).FirstOrDefault();

        public static string AttributeToString(this XElement element, XName name) => element?.Attributes(name).Select(e => e.Value).FirstOrDefault();

        public static byte[] ElementToBinary(this IEnumerable<XElement> sourceElements, XName name) => ToFirstObject(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), Convert.FromBase64String);

        public static byte[] ElementToBinary(this XElement element, XName name) => ToObject(element.Element(name)?.Value, Convert.FromBase64String);

        public static byte[] AttributeToBinary(this IEnumerable<XElement> sourceElements, XName name) => ToFirstObject(sourceElements.Attributes(name)
            .Select(a => a.Value), Convert.FromBase64String);

        public static byte[] AttributeToBinary(this XElement element, XName name) => ToObject(element.Attribute(name)?.Value, Convert.FromBase64String);

        public static bool? ElementToBoolean(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToBoolean);

        public static bool? ElementToBoolean(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToBoolean);

        public static bool? AttributeToBoolean(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToBoolean);

        public static bool? AttributeToBoolean(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToBoolean);

        public static byte? ElementToByte(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToByte);

        public static byte? ElementToByte(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToByte);

        public static byte? AttributeToByte(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToByte);

        public static byte? AttributeToByte(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToByte);

        public static sbyte? ElementToSByte(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToSByte);

        public static sbyte? ElementToSByte(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToSByte);

        public static sbyte? AttributeToSByte(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToSByte);

        public static sbyte? AttributeToSByte(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToSByte);

        public static char? ElementToChar(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToChar);

        public static char? ElementToChar(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToChar);

        public static char? AttributeToChar(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToChar);

        public static char? AttributeToChar(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToChar);

        public static DateTime? ElementToDateTime(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), t => XmlConvert.ToDateTime(t, XmlDateTimeSerializationMode.RoundtripKind));

        public static DateTime? ElementToDateTime(this XElement element, XName name) => ToValue(element.Element(name)?.Value,
            t => XmlConvert.ToDateTime(t, XmlDateTimeSerializationMode.RoundtripKind));

        public static DateTime? AttributeToDateTime(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), t => XmlConvert.ToDateTime(t, XmlDateTimeSerializationMode.RoundtripKind));

        public static DateTime? AttributeToDateTime(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value,
            t => XmlConvert.ToDateTime(t, XmlDateTimeSerializationMode.RoundtripKind));

        public static Guid? ElementToGuid(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToGuid);

        public static Guid? ElementToGuid(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToGuid);

        public static Guid? AttributeToGuid(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToGuid);

        public static Guid? AttributeToGuid(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToGuid);

        public static short? ElementToInt16(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToInt16);

        public static short? ElementToInt16(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToInt16);

        public static short? AttributeToInt16(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToInt16);

        public static short? AttributeToInt16(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToInt16);

        public static int? ElementToInt32(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToInt32);

        public static int? ElementToInt32(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToInt32);

        public static int? AttributeToInt32(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToInt32);

        public static int? AttributeToInt32(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToInt32);

        public static long? ElementToInt64(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToInt64);

        public static long? ElementToInt64(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToInt64);

        public static long? AttributeToInt64(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToInt64);

        public static long? AttributeToInt64(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToInt64);

        public static ushort? ElementToUInt16(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToUInt16);

        public static ushort? ElementToUInt16(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToUInt16);

        public static ushort? AttributeToUInt16(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToUInt16);

        public static ushort? AttributeToUInt16(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToUInt16);

        public static uint? ElementToUInt32(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToUInt32);

        public static uint? ElementToUInt32(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToUInt32);

        public static uint? AttributeToUInt32(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToUInt32);

        public static uint? AttributeToUInt32(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToUInt32);

        public static ulong? ElementToUInt64(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToUInt64);

        public static ulong? ElementToUInt64(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToUInt64);

        public static ulong? AttributeToUInt64(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToUInt64);

        public static ulong? AttributeToUInt64(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToUInt64);

        public static decimal? ElementToDecimal(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToDecimal);

        public static decimal? ElementToDecimal(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToDecimal);

        public static decimal? AttributeToDecimal(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToDecimal);

        public static decimal? AttributeToDecimal(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToDecimal);

        public static double? ElementToDouble(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToDouble);

        public static double? ElementToDouble(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToDouble);

        public static double? AttributeToDouble(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToDouble);

        public static double? AttributeToDouble(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToDouble);

        public static float? ElementToSingle(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToSingle);

        public static float? ElementToSingle(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToSingle);

        public static float? AttributeToSingle(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToSingle);

        public static float? AttributeToSingle(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToSingle);

        public static TimeSpan? ElementToTimeSpan(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), XmlConvert.ToTimeSpan);

        public static TimeSpan? ElementToTimeSpan(this XElement element, XName name) => ToValue(element.Element(name)?.Value, XmlConvert.ToTimeSpan);

        public static TimeSpan? AttributeToTimeSpan(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), XmlConvert.ToTimeSpan);

        public static TimeSpan? AttributeToTimeSpan(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, XmlConvert.ToTimeSpan);

        public static DriveType? ElementToDriveType(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Elements(name)
            .Select(e => e.IsEmpty ? null : e.Value), s => Enum.Parse<DriveType>(s));

        public static DriveType? ElementToDriveType(this XElement element, XName name) => ToValue(element.Element(name)?.Value, s => Enum.Parse<DriveType>(s));

        public static DriveType? AttributeToDriveType(this IEnumerable<XElement> sourceElements, XName name) => ToFirstValue(sourceElements.Attributes(name)
            .Select(a => a.Value), s => Enum.Parse<DriveType>(s));

        public static DriveType? AttributeToDriveType(this XElement element, XName name) => ToValue(element.Attribute(name)?.Value, s => Enum.Parse<DriveType>(s));

        #endregion

        #region Entity Definition XML Methods

        public static IEnumerable<XElement> GetAllRootEntityElements(this XDocument entityDefinitions) => entityDefinitions.Root?.Elements(XNAME_Root).Elements(XNAME_Entity);

        public static IEnumerable<XElement> GetAllRootEnumTypeElements(this XDocument entityDefinitions) => entityDefinitions.Root?.Elements(XNAME_Root).Elements(XNAME_EnumTypes).Elements();

        public static IEnumerable<XElement> GetAllRootPropertyElements(this XDocument entityDefinitions) => entityDefinitions.GetAllRootEntityElements().Elements(XNAME_Properties).Elements();

        public static IEnumerable<XElement> GetAllRootEnumFieldElements(this XDocument entityDefinitions) => entityDefinitions.GetAllRootEnumTypeElements().Elements(XNAME_Field);

        public static IEnumerable<XElement> GetAllLocalEntityElements(this XDocument entityDefinitions) => entityDefinitions.GetAllRootEntityElements()
            .Concat(entityDefinitions.Root?.Elements(XNAME_Local).Elements(XNAME_Entity));

        public static IEnumerable<XElement> GetAllLocalEnumTypeElements(this XDocument entityDefinitions) => entityDefinitions.GetAllRootEnumTypeElements()
            .Concat(entityDefinitions.Root?.Elements(XNAME_Local).Elements(XNAME_EnumTypes).Elements());

        public static IEnumerable<XElement> GetAllLocalPropertyElements(this XDocument entityDefinitions) => entityDefinitions.GetAllRootPropertyElements()
            .Concat(entityDefinitions.GetAllLocalEntityElements().Elements(XNAME_Properties).Elements());

        public static IEnumerable<XElement> GetAllLocalEnumFieldElements(this XDocument entityDefinitions) => entityDefinitions.GetAllRootEnumFieldElements()
            .Concat(entityDefinitions.GetAllLocalEnumTypeElements().Elements(XNAME_Properties).Elements());

        public static IEnumerable<XElement> GetAllUpstreamEntityElements(this XDocument entityDefinitions) => entityDefinitions.GetAllRootEntityElements()
            .Concat(entityDefinitions.Root?.Elements(XNAME_Upstream).Elements(XNAME_Entity));

        public static IEnumerable<XElement> GetAllUpstreamEnumTypeElements(this XDocument entityDefinitions) => entityDefinitions.GetAllRootEnumTypeElements()
            .Concat(entityDefinitions.Root?.Elements(XNAME_Upstream).Elements(XNAME_EnumTypes).Elements());

        public static IEnumerable<XElement> GetAllUpstreamPropertyElements(this XDocument entityDefinitions) => entityDefinitions.GetAllRootPropertyElements()
            .Concat(entityDefinitions.GetAllUpstreamEntityElements().Elements(XNAME_Properties).Elements());

        public static IEnumerable<XElement> GetAllUpstreamEnumFieldElements(this XDocument entityDefinitions) => entityDefinitions.GetAllRootEnumFieldElements()
            .Concat(entityDefinitions.GetAllUpstreamEnumTypeElements().Elements(XNAME_Properties).Elements());

        public static XElement FindRootEntityByName(this XDocument entityDefinitions, string name) =>
            entityDefinitions.GetAllRootEntityElements().GetElementsByAttributeValue(XNAME_Name, name).FirstOrDefault();

        public static XElement FindRootEnumTypeByName(this XDocument entityDefinitions, string name) =>
            entityDefinitions.GetAllRootEnumTypeElements().GetElementsByAttributeValue(XNAME_Name, name).FirstOrDefault();

        public static XElement FindLocalEntityByName(this XDocument entityDefinitions, string name) =>
            entityDefinitions.GetAllLocalEntityElements().GetElementsByAttributeValue(XNAME_Name, name).FirstOrDefault();

        public static XElement FindLocalEnumTypeByName(this XDocument entityDefinitions, string name) =>
            entityDefinitions.GetAllLocalEnumTypeElements().GetElementsByAttributeValue(XNAME_Name, name).FirstOrDefault();

        public static XElement FindUpstreamEntityByName(this XDocument entityDefinitions, string name) =>
            entityDefinitions.GetAllUpstreamEntityElements().GetElementsByAttributeValue(XNAME_Name, name).FirstOrDefault();

        public static XElement FindUpstreamEnumTypeByName(this XDocument entityDefinitions, string name) =>
            entityDefinitions.GetAllUpstreamEnumTypeElements().GetElementsByAttributeValue(XNAME_Name, name).FirstOrDefault();

        public static XElement FindRootPropertyByFullName(this XDocument entityDefinitions, string fullName) =>
            entityDefinitions.GetAllRootPropertyElements().GetElementsByAttributeValue(XNAME_FullName, fullName).FirstOrDefault();

        public static XElement FindRootFieldByFullName(this XDocument entityDefinitions, string fullName) =>
            entityDefinitions.GetAllRootEnumFieldElements().GetElementsByAttributeValue(XNAME_FullName, fullName).FirstOrDefault();

        public static XElement FindLocalPropertyByFullName(this XDocument entityDefinitions, string fullName) =>
            entityDefinitions.GetAllLocalPropertyElements().GetElementsByAttributeValue(XNAME_FullName, fullName).FirstOrDefault();

        public static XElement FindLocalFieldByFullName(this XDocument entityDefinitions, string fullName) =>
            entityDefinitions.GetAllLocalEnumFieldElements().GetElementsByAttributeValue(XNAME_FullName, fullName).FirstOrDefault();

        public static XElement FindUpstreamPropertyByFullName(this XDocument entityDefinitions, string fullName) =>
            entityDefinitions.GetAllUpstreamPropertyElements().GetElementsByAttributeValue(XNAME_FullName, fullName).FirstOrDefault();

        public static XElement FindUpstreamFieldByFullName(this XDocument entityDefinitions, string fullName) =>
            entityDefinitions.GetAllUpstreamEnumFieldElements().GetElementsByAttributeValue(XNAME_FullName, fullName).FirstOrDefault();

        public static bool IsScopeElement(this XElement element)
        {
            if (element is null)
                return false;
            XElement root = element.Document?.Root;
            if (root is null)
                return element.Name == XNAME_Root || element.Name == XNAME_Local || element.Name == XNAME_Upstream;
            return ReferenceEquals(root, element.Parent);
        }

        public static bool IsEntityElement(this XElement element) => element is not null && element.Name == XNAME_Entity && (element.Parent is null || IsScopeElement(element.Parent));

        public static bool IsEnumTypeElement(this XElement element) => element is not null && element.Parent?.Name == XNAME_EnumTypes && (element.Parent?.Parent is null ||
            IsScopeElement(element.Parent?.Parent));

        public static bool IsPropertyElement(this XElement element) => element is not null && element.Parent?.Name == XNAME_Properties && IsEntityElement(element.Parent?.Parent);

        public static bool IsEnumFieldElement(this XElement element) => element is not null && element.Name == XNAME_Field && IsEnumTypeElement(element.Parent);

        public static XElement GetCurrentScopeElement(this XElement refElement)
        {
            XElement root = refElement.Document?.Root;
            return (root is null) ? null : refElement.AncestorsAndSelf().FirstOrDefault(e => ReferenceEquals(root, e.Parent));
        }

        public static XElement GetCurrentEntityElement(this XElement refElement) => refElement?.AncestorsAndSelf().FirstOrDefault(IsEntityElement);

        public static XElement GetCurrentPropertyElement(this XElement refElement) => refElement?.AncestorsAndSelf().FirstOrDefault(IsPropertyElement);

        public static IEnumerable<XElement> GetAllEntityElements(this XElement refElement)
        {
            XElement scopeElement = GetCurrentScopeElement(refElement);
            if (scopeElement is null)
                return Enumerable.Empty<XElement>();
            if (scopeElement.Name != XNAME_Root)
                return scopeElement.Elements(XNAME_Entity).Concat(scopeElement.Ancestors().Take(1).Elements(XNAME_Root).Elements(XNAME_Entity));
            return scopeElement.Elements(XNAME_Entity);
        }

        public static IEnumerable<XElement> GetAllPropertyElements(this XElement refElement) => GetAllEntityElements(refElement).Elements(XNAME_Properties).Elements();

        public static IEnumerable<XElement> GetAllEnumTypeElements(this XElement refElement)
        {
            XElement scopeElement = GetCurrentScopeElement(refElement);
            if (scopeElement is null)
                return Enumerable.Empty<XElement>();
            if (scopeElement.Name != XNAME_Root)
            {
                XElement rootElement = scopeElement.Parent?.Element(XNAME_Root);
                if (rootElement is not null)
                    return scopeElement.Elements(XNAME_EnumTypes).Concat(scopeElement.Ancestors().Take(1).Elements(XNAME_Root).Elements(XNAME_EnumTypes)).Elements();
            }
            return scopeElement.Elements(XNAME_EnumTypes).Elements();
        }

        public static IEnumerable<XElement> GetAllEnumFieldElements(this XElement refElement) => GetAllEnumTypeElements(refElement).Elements(XNAME_Field);

        public static XElement FindCurrentEntityPropertyByName(this XElement refElement, string name) => (name is null) ? null :
            GetCurrentEntityElement(refElement)?.Element(XNAME_Properties).Attributes(XNAME_Name).Where(a => a.Value == name).Select(a => a.Parent).FirstOrDefault();

        public static XElement FindEntityByName(this XElement refElement, string name) => GetAllEntityElements(refElement).GetElementsByAttributeValue(XNAME_Name, name).FirstOrDefault();

        public static IEnumerable<XElement> FindEntitiesByNames(this XElement refElement, IEnumerable<string> names) => GetAllEntityElements(refElement).GetElementsByAttributeValue(XNAME_Name, names);

        public static XElement FindEnumTypeByName(this XElement refElement, string name) => GetAllEnumTypeElements(refElement).GetElementsByAttributeValue(XNAME_Name, name).FirstOrDefault();

        public static XElement FindPropertyByFullName(this XElement refElement, string fullName) => GetAllPropertyElements(refElement).GetElementsByAttributeValue(XNAME_FullName, fullName).FirstOrDefault();

        public static XElement FindFieldByFullName(this XElement refElement, string fullName) => GetAllEnumFieldElements(refElement).GetElementsByAttributeValue(XNAME_FullName, fullName).FirstOrDefault();

        #endregion
    }
}
