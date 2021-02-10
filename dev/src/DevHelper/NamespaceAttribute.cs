using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DevHelper
{
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class NamespaceAttribute : System.Attribute
    {
        public static ReadOnlyDictionary<XmlDocumentNamespace, NamespaceAttribute> Map { get; }

        static NamespaceAttribute()
        {
            Type t = typeof(XmlDocumentNamespace);
            Dictionary<XmlDocumentNamespace, NamespaceAttribute> map = new Dictionary<XmlDocumentNamespace, NamespaceAttribute>();
            foreach (XmlDocumentNamespace ns in Enum.GetValues(t))
            {
                string n = Enum.GetName(t, ns);
                NamespaceAttribute a = t.GetField(n).GetCustomAttributes(typeof(NamespaceAttribute), false).OfType<NamespaceAttribute>().FirstOrDefault();
                if (a is null)
                    a = new NamespaceAttribute(null) { Prefix = n };
                else if (a.Prefix is null)
                    a.Prefix = n;
                map.Add(ns, a);
            }
            Map = new ReadOnlyDictionary<XmlDocumentNamespace, NamespaceAttribute>(map);
        }

        private readonly string _uri;
        private string _schemaLocation = null;
        private string _prefix = null;

        public NamespaceAttribute(string absoluteUri)
        {
            _uri = (Uri.TryCreate(absoluteUri, UriKind.Absolute, out Uri uri)) ? uri.AbsolutePath : "";
        }

        public string URI => _uri;

        public string SchemaLocation
        {
            get => _schemaLocation ?? "";
            set
            {
                if (null != _schemaLocation)
                    throw new InvalidOperationException();
                if (string.IsNullOrEmpty(value))
                    _schemaLocation = "";
                else if (Uri.TryCreate(value, UriKind.Absolute, out Uri uri))
                    _schemaLocation = uri.AbsoluteUri;
                else
                    _schemaLocation = Uri.EscapeUriString(value.Replace('\\', '/'));
            }
        }

        public string Prefix
        {
            get => _prefix ?? "";
            set
            {
                if (null != _prefix)
                    throw new InvalidOperationException();
                _prefix = XmlQNameAttribute.EnsureLocalName(value);
            }
        }

        public bool IsCommandNS { get; set; }

        public bool ElementFormQualified { get; set; }
    }
}
