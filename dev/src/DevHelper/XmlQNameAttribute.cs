using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace DevHelper
{
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class XmlQNameAttribute : System.Attribute
    {
        public static ReadOnlyDictionary<PsHelpNodeName, XmlQNameAttribute> Map { get; }

        static XmlQNameAttribute()
        {
            Type t = typeof(PsHelpNodeName);
            Dictionary<PsHelpNodeName, XmlQNameAttribute> map = new Dictionary<PsHelpNodeName, XmlQNameAttribute>();
            foreach (PsHelpNodeName name in Enum.GetValues(t))
            {
                string n = Enum.GetName(t, name);
                XmlQNameAttribute attribute = t.GetField(n).GetCustomAttributes(typeof(XmlQNameAttribute), false).OfType<XmlQNameAttribute>().FirstOrDefault();
                if (attribute is null)
                    attribute = new XmlQNameAttribute(XmlDocumentNamespace.None) { LocalName = n };
                else if (string.IsNullOrWhiteSpace(attribute._localName))
                        attribute._localName = n;
                map.Add(name, attribute);
            }
            Map = new ReadOnlyDictionary<PsHelpNodeName, XmlQNameAttribute>(map);
        }

        private readonly XmlDocumentNamespace _prefix;
        private string _localName = null;

        // This is a positional argument
        public XmlQNameAttribute(XmlDocumentNamespace prefix)
        {
            _prefix = prefix;
        }

        public XmlDocumentNamespace Prefix => _prefix;

        public static string EnsureLocalName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;
            if (XmlConvert.IsStartNCNameChar(name[0]))
            {
                if (name.Length == 1)
                    return name;
                return name.Substring(0, 1) + string.Join("_", name.Substring(1).Split('_').Select(s => (s.Length > 0) ? XmlConvert.EncodeLocalName(s) : s));
            }
            if (name.Length == 1)
                return XmlConvert.EncodeLocalName(name.Substring(0, 1));
            return XmlConvert.EncodeLocalName(name.Substring(0, 1)) + string.Join("_", name.Substring(1).Split('_').Select(s => (s.Length > 0) ? XmlConvert.EncodeLocalName(s) : s));
        }

        public string LocalName
        {
            get => _localName ?? "";
            set
            {
                if (null != _localName)
                    throw new InvalidOperationException();
                _localName = EnsureLocalName(value);
            }
        }
    }
}
