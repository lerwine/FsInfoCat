using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace DevHelper
{
    public static class XmlNameHelper
    {
        private static ReadOnlyDictionary<XmlDocumentNamespace, string> _uriMap;
        private static ReadOnlyDictionary<XmlDocumentNamespace, string> _prefixMap;
        private static ReadOnlyDictionary<XmlDocumentNamespace, string> _schemaLocationMap;
        private static ReadOnlyDictionary<XmlDocumentNamespace, bool> _elementFormQualifiedMap;
        public static ReadOnlyCollection<XmlDocumentNamespace> CommandNsNamespaces;
        private static Dictionary<PsHelpNodeName, string> _localNameMap = new Dictionary<PsHelpNodeName, string>();
        private static Dictionary<PsHelpNodeName, XmlDocumentNamespace> _nsValueMap = new Dictionary<PsHelpNodeName, XmlDocumentNamespace>();
        private static Dictionary<PsHelpNodeName, XmlQualifiedName> _qNameMap = new Dictionary<PsHelpNodeName, XmlQualifiedName>();

        static XmlNameHelper()
        {
            Type t = typeof(XmlDocumentNamespace);
            Dictionary<XmlDocumentNamespace, string> uriMap = new Dictionary<XmlDocumentNamespace, string>();
            Dictionary<XmlDocumentNamespace, string> prefixMap = new Dictionary<XmlDocumentNamespace, string>();
            Dictionary<XmlDocumentNamespace, string> schemaLocationMap = new Dictionary<XmlDocumentNamespace, string>();
            Dictionary<XmlDocumentNamespace, bool> elementFormQualifiedMap = new Dictionary<XmlDocumentNamespace, bool>();
            Collection<XmlDocumentNamespace> commandNsNamespaces = new Collection<XmlDocumentNamespace>();
            foreach (XmlDocumentNamespace value in Enum.GetValues(t))
            {
                string n = Enum.GetName(t, value);
                NamespaceAttribute a = t.GetField(n).GetCustomAttributes(typeof(NamespaceAttribute), false).OfType<NamespaceAttribute>().FirstOrDefault();
                if (a is null)
                {
                    uriMap.Add(value, "");
                    prefixMap.Add(value, n);
                    schemaLocationMap.Add(value, "");
                    schemaLocationMap.Add(value, "");
                    elementFormQualifiedMap.Add(value, true);
                }
                else
                {
                    uriMap.Add(value, a.URI);
                    prefixMap.Add(value, (a.Prefix is null) ? n : a.Prefix.AsValidNCName());
                    if (string.IsNullOrEmpty(a.SchemaLocation))
                    schemaLocationMap.Add(value, "");
                    else if (Uri.TryCreate(a.SchemaLocation, UriKind.Absolute, out Uri uri))
                        schemaLocationMap.Add(value, uri.AbsoluteUri);
                    else
                        schemaLocationMap.Add(value, Uri.EscapeUriString(a.SchemaLocation.Replace('\\', '/')));
                    if (a.IsCommandNS)
                        commandNsNamespaces.Add(value);
                    elementFormQualifiedMap.Add(value, a.ElementFormQualified);
                }
                _uriMap = new ReadOnlyDictionary<XmlDocumentNamespace, string>(uriMap);
                _prefixMap = new ReadOnlyDictionary<XmlDocumentNamespace, string>(prefixMap);
                _schemaLocationMap = new ReadOnlyDictionary<XmlDocumentNamespace, string>(schemaLocationMap);
                _elementFormQualifiedMap = new ReadOnlyDictionary<XmlDocumentNamespace, bool>(elementFormQualifiedMap);
                CommandNsNamespaces = new ReadOnlyCollection<XmlDocumentNamespace>(commandNsNamespaces);
            }
        }

        private static string _FromAttributes(PsHelpNodeName name, out XmlDocumentNamespace ns)
        {
            Type t = name.GetType();
            string n = Enum.GetName(t, name);
            XmlQNameAttribute attribute = t.GetField(n).GetCustomAttributes(typeof(XmlQNameAttribute), false)
                .OfType<XmlQNameAttribute>().FirstOrDefault();
            if (attribute is null)
                ns = XmlDocumentNamespace.None;
            else
            {
                ns = attribute.Prefix;
                if (!string.IsNullOrEmpty(attribute.LocalName))
                    n = attribute.LocalName.AsValidNCName();
            }
            return n;
        }

        public static string URI(this XmlDocumentNamespace value) => _uriMap[value];

        public static string Prefix(this XmlDocumentNamespace value) => _prefixMap[value];

        public static string SchemaLocation(this XmlDocumentNamespace value) => _schemaLocationMap[value];

        public static bool IsElementFormQualified(this XmlDocumentNamespace value) => _elementFormQualifiedMap[value];

        public static XmlQualifiedName QualifiedName(this PsHelpNodeName name) => new XmlQualifiedName(name.LocalName(out string namespaceURI), namespaceURI);

        public static string LocalName(this PsHelpNodeName name)
        {
            lock(_nsValueMap)
            {
                if (_localNameMap.ContainsKey(name))
                    return _localNameMap[name];
                string n = _FromAttributes(name, out XmlDocumentNamespace ns);
                _localNameMap.Add(name, n);
                _nsValueMap.Add(name, ns);
                return n;
            }
        }

        public static string LocalName(this PsHelpNodeName name, out XmlDocumentNamespace ns)
        {
            lock(_nsValueMap)
            {
                if (_localNameMap.ContainsKey(name))
                {
                    ns = _nsValueMap[name];
                    return _localNameMap[name];
                }
                string n = _FromAttributes(name, out ns);
                _localNameMap.Add(name, n);
                _nsValueMap.Add(name, ns);
                return n;
            }
        }

        public static string LocalName(this PsHelpNodeName name, out string namespaceURI)
        {
            string localName = name.LocalName(out XmlDocumentNamespace ns);
                namespaceURI = ns.URI();
            return localName;
        }

        internal static bool IsMatch(this PsHelpNodeName name, XmlElement element)
        {
            return !(element is null) && element.LocalName.Equals(name.LocalName(out XmlDocumentNamespace ns)) &&
                element.NamespaceURI.Equals(ns.URI());
        }

        public static string Prefix(this PsHelpNodeName name) => name.ToNamespaceValue().Prefix();

        public static XmlDocumentNamespace ToNamespaceValue(this PsHelpNodeName name)
        {
            lock(_nsValueMap)
            {
                if (_nsValueMap.ContainsKey(name))
                    return _nsValueMap[name];
                string n = _FromAttributes(name, out XmlDocumentNamespace ns);
                _localNameMap.Add(name, n);
                _nsValueMap.Add(name, ns);
                return ns;
            }
        }

        public static bool IsElementFormQualified(this PsHelpNodeName name) => name.ToNamespaceValue().IsElementFormQualified();

        public static XmlElement CreateElement(this PsHelpNodeName name, XmlDocument document)
        {
            string localName = name.LocalName(out string namespaceURI);
            string prefix = name.Prefix();
            if (name.IsElementFormQualified())
            {
                string p = document.GetPrefixOfNamespace(name.LocalName(out XmlDocumentNamespace ns));
                return document.CreateElement((string.IsNullOrEmpty(p)) ? ns.Prefix() : p, localName, namespaceURI);
            }
            return document.CreateElement(localName, namespaceURI);
        }

    }
}
