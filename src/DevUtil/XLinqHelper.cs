using System;
using System.Xml;
using System.Xml.Linq;

namespace DevUtil
{

    public static class XLinqHelper
    {
        public const string NAMESPACE_md = "http://git.erwinefamily.net/FsInfoCat/V1/ModelDefinitions.xsd";
        private static readonly XNamespace _md = XNamespace.Get(NAMESPACE_md);
        private static readonly XNamespace _xsd = XNamespace.Get(System.Xml.Schema.XmlSchema.Namespace);
        public static XName GetName(string localName) { return XNamespace.None.GetName(XmlConvert.VerifyNCName(localName)); }
        public static XName GetXmlName(string localName) { return XNamespace.Xml.GetName(XmlConvert.VerifyNCName(localName)); }
        public static XName GetXmlnsName(string localName) { return XNamespace.Xmlns.GetName(XmlConvert.VerifyNCName(localName)); }
        public static XName GetXsdName(string localName) { return _xsd.GetName(XmlConvert.VerifyNCName(localName)); }
        public static XName GetMdName(string localName) { return _md.GetName(XmlConvert.VerifyNCName(localName)); }
        public static XAttribute NewAttribute(string localName, object value) { return new XAttribute(GetName(localName), value); }
        public static XElement NewElement(string localName, params object[] content) { return new XElement(GetName(localName), content); }
        public static XAttribute NewXmlnsAttribute(string localName, object value) { return new XAttribute(GetXmlnsName(localName), value); }
        public static XAttribute NewXsdAttribute(string localName, object value) { return new XAttribute(GetXsdName(localName), value); }
        public static XElement NewXsdElement(string localName, params object[] content) { return new XElement(GetXsdName(localName), content); }
        public static XAttribute NewMdAttribute(string localName, object value) { return new XAttribute(GetMdName(localName), value); }
        public static XElement NewMdElement(string localName, params object[] content) { return new XElement(GetMdName(localName), content); }
        public static void SetAttribute(XElement parent, string localName, object value)
        {
            XName name = GetName(localName);
            XAttribute atribute = parent.Attribute(name);
            if (atribute is null)
                parent.Add(new XAttribute(name, value));
            else
                atribute.SetValue(value);
        }
        public static void SetXsdAttribute(XElement parent, string localName, object value)
        {
            XName name = GetXsdName(localName);
            XAttribute atribute = parent.Attribute(name);
            if (atribute is null)
                parent.Add(new XAttribute(name, value));
            else
                atribute.SetValue(value);
        }
        public static void SetMdAttribute(XElement parent, string localName, object value)
        {
            XName name = GetMdName(localName);
            XAttribute atribute = parent.Attribute(name);
            if (atribute is null)
                parent.Add(new XAttribute(name, value));
            else
                atribute.SetValue(value);
        }
        public static DateTime? ToDateTime(string value, XmlDateTimeSerializationMode mode)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToDateTime(value, mode); }
            catch { return null; }
        }
        public static Guid? ToGuid(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToGuid(value); }
            catch { return null; }
        }
        public static bool? ToBoolean(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToBoolean(value); }
            catch { return null; }
        }
        public static decimal? ToDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToDecimal(value); }
            catch { return null; }
        }
        public static sbyte? ToSByte(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToSByte(value); }
            catch { return null; }
        }
        public static short? ToInt16(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToInt16(value); }
            catch { return null; }
        }
        public static int? ToInt32(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToInt32(value); }
            catch { return null; }
        }
        public static long? ToInt64(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToInt64(value); }
            catch { return null; }
        }
        public static byte? ToByte(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToByte(value); }
            catch { return null; }
        }
        public static ushort? ToUInt16(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToUInt16(value); }
            catch { return null; }
        }
        public static uint? ToUInt32(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToUInt32(value); }
            catch { return null; }
        }
        public static ulong? ToUInt64(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToUInt64(value); }
            catch { return null; }
        }
        public static float? ToSingle(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToSingle(value); }
            catch { return null; }
        }
        public static double? ToDouble(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToDouble(value); }
            catch { return null; }
        }
        public static TimeSpan? ToTimeSpan(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            try { return XmlConvert.ToTimeSpan(value); }
            catch { return null; }
        }
    }
}
