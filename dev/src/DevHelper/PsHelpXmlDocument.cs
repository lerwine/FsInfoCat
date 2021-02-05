using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Xml;
using System.Xml.Schema;

namespace DevHelper
{
    public class PsHelpXmlDocument : XmlDocument
    {
        public const string NAMESPACE_URI_MSH = "http://msh";
        public const string NAMESPACE_URI_MAML = "http://schemas.microsoft.com/maml/2004/10";
        public const string NAMESPACE_URI_COMMAND = "http://schemas.microsoft.com/maml/dev/command/2004/10";
        public const string NAMESPACE_URI_DEV = "http://schemas.microsoft.com/maml/dev/2004/10";
        public const string NAMESPACE_URI_MSHELP = "http://msdn.microsoft.com/mshelp";
        public const string NAMESPACE_URI_XSI = "http://www.w3.org/2001/XMLSchema-instance";
        public const string NAMESPACE_URI_XMLNS = "http://www.w3.org/2000/xmlns/";
        private const string XML_ATTRIBUTE_NAME_SCHEMA = "schema";
        private const string XML_ATTRIBUTE_VALUE_MAML = "maml";
        private FileInfo _savedLocation;
        private bool _hasChanges;
        private XmlDocument _original = new XmlDocument();

        public bool IsPsHelpXmlDocument(XmlDocument source)
        {
            XmlElement documentElement;
            if (source is null || (documentElement = source.DocumentElement) is null)
                return false;
            XmlQNameAttribute qNameAttribute = XmlQNameAttribute.Map[PsHelpNodeName.helpItems];
            NamespaceAttribute namespaceAttribute;
            if (qNameAttribute.LocalName != documentElement.LocalName || documentElement.NamespaceURI != (namespaceAttribute = NamespaceAttribute.Map[qNameAttribute.Prefix]).URI)
                return false;
            XmlAttribute xmlAttribute = documentElement.SelectSingleNode("@" + XML_ATTRIBUTE_NAME_SCHEMA) as XmlAttribute;
            return (null != xmlAttribute && xmlAttribute.Value == XML_ATTRIBUTE_VALUE_MAML);
        }

        public PsHelpXmlDocument(string path)
        {
            _savedLocation = (string.IsNullOrEmpty(path)) ? null : new FileInfo(path);
            _hasChanges = _savedLocation is null || !_savedLocation.Exists;
            if (_hasChanges)
                AppendChild(CreateElement(PsHelpNodeName.helpItems)).Attributes.Append(CreateAttribute(XML_ATTRIBUTE_NAME_SCHEMA)).Value = XML_ATTRIBUTE_VALUE_MAML;
            else
            {
                _original.Load(_savedLocation.FullName);
                if (_original.DocumentElement is null)
                    throw new InvalidDataException(_savedLocation.FullName + " contains no root element");
                if (IsPsHelpXmlDocument(_original))
                    foreach (XmlNode node in _original.ChildNodes)
                        AppendChild(ImportNode(node, true));
            }
        }

        public PsHelpXmlDocument()
        {
            AppendChild(CreateElement(PsHelpNodeName.helpItems)).Attributes.Append(CreateAttribute("schema")).Value = "maml";
        }

        public XmlElement CreateElement(PsHelpNodeName name)
        {
            XmlQNameAttribute qNameAttribute = XmlQNameAttribute.Map[name];
            string localName = qNameAttribute.LocalName;
            NamespaceAttribute namespaceAttribute = NamespaceAttribute.Map[qNameAttribute.Prefix];
            string prefix = namespaceAttribute.Prefix;
            if (namespaceAttribute.ElementFormQualified)
            {
                string p = GetPrefixOfNamespace(namespaceAttribute.URI);
                return CreateElement((string.IsNullOrEmpty(p)) ? namespaceAttribute.Prefix : p, localName,
                    namespaceAttribute.URI);
            }
            return CreateElement(localName, namespaceAttribute.URI);
        }

        public XmlElement AddCommand(VerbsCommon verbs, string noun)
        {
            XmlElement commandElement = (XmlElement)DocumentElement.AppendChild(CreateElement(PsHelpNodeName.command));
            NamespaceAttribute xmlnsAttribute = NamespaceAttribute.Map[XmlDocumentNamespace.xmlns];
            NamespaceAttribute nsAttribute = NamespaceAttribute.Map[XmlDocumentNamespace.maml];
            commandElement.Attributes.Append(CreateAttribute(xmlnsAttribute.Prefix, nsAttribute.Prefix, xmlnsAttribute.URI)).Value = nsAttribute.URI;
            nsAttribute = NamespaceAttribute.Map[XmlDocumentNamespace.command];
            commandElement.Attributes.Append(CreateAttribute(xmlnsAttribute.Prefix, nsAttribute.Prefix, xmlnsAttribute.URI)).Value = nsAttribute.URI;
            nsAttribute = NamespaceAttribute.Map[XmlDocumentNamespace.dev];
            commandElement.Attributes.Append(CreateAttribute(xmlnsAttribute.Prefix, nsAttribute.Prefix, xmlnsAttribute.URI)).Value = nsAttribute.URI;
            XmlElement detailsElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.details));

            XmlElement descriptionElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.description));

            XmlElement syntaxElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.syntax));

            XmlElement parametersElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.parameters));

            XmlElement inputTypesElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.inputTypes));

            XmlElement returnValuesElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.returnValues));

            XmlElement terminatingErrorsElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.terminatingErrors));

            XmlElement nonTerminatingErrorsElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.nonTerminatingErrors));

            XmlElement alertSetElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.alertSet));

            XmlElement examplesElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.examples));

            XmlElement relatedLinksElement = (XmlElement)commandElement.AppendChild(CreateElement(PsHelpNodeName.relatedLinks));
        }
    }

    public enum PsHelpNodeName
    {
        [XmlQName(XmlDocumentNamespace.msh)]
        helpItems,

        [XmlQName(XmlDocumentNamespace.command)]
        command,

        [XmlQName(XmlDocumentNamespace.maml)]
        para,

        [XmlQName(XmlDocumentNamespace.maml)]
        description,

        [XmlQName(XmlDocumentNamespace.maml)]
        copyright,

        [XmlQName(XmlDocumentNamespace.command)]
        verb,

        [XmlQName(XmlDocumentNamespace.command)]
        noun,

        [XmlQName(XmlDocumentNamespace.command)]
        details,

        [XmlQName(XmlDocumentNamespace.command, LocalName = "command")]
        commandName,

        [XmlQName(XmlDocumentNamespace.maml)]
        name,

        [XmlQName(XmlDocumentNamespace.maml)]
        uri,

        [XmlQName(XmlDocumentNamespace.command)]
        syntax,

        [XmlQName(XmlDocumentNamespace.command)]
        syntaxItem,

        [XmlQName(XmlDocumentNamespace.command)]
        parameter,

        [XmlQName(XmlDocumentNamespace.dev)]
        type,

        [XmlQName(XmlDocumentNamespace.dev)]
        defaultValue,

        [XmlQName(XmlDocumentNamespace.command)]
        parameters,

        [XmlQName(XmlDocumentNamespace.command)]
        inputTypes,

        [XmlQName(XmlDocumentNamespace.command)]
        returnValues,

        [XmlQName(XmlDocumentNamespace.command)]
        terminatingErrors,

        [XmlQName(XmlDocumentNamespace.command)]
        nonTerminatingErrors,

        [XmlQName(XmlDocumentNamespace.maml)]
        alertSet,

        [XmlQName(XmlDocumentNamespace.command)]
        examples,

        [XmlQName(XmlDocumentNamespace.maml)]
        relatedLinks
    }

    public enum XmlDocumentNamespace
    {
        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_MSH, Prefix = "")]
        None,

        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_MSH, SchemaLocation = "Msh.xsd")]
        msh,

        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_MAML, IsCommandNS = true, SchemaLocation = "PSMaml/Maml.xsd", ElementFormQualified = true)]
        maml,

        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_COMMAND, IsCommandNS = true, SchemaLocation = "PSMaml/developerCommand.xsd", ElementFormQualified = true)]
        command,

        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_DEV, IsCommandNS = true, SchemaLocation = "PSMaml/developer.xsd", ElementFormQualified = true)]
        dev,

        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_MSHELP, IsCommandNS = true, ElementFormQualified = true)]
        MSHelp,
        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_XSI, ElementFormQualified = true)]
        xsi,
        [Namespace(PsHelpXmlDocument.NAMESPACE_URI_XMLNS, ElementFormQualified = true)]
        xmlns
    }

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

        // This is a positional argument
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

        public PsHelpXmlDocument(bool isCommandNS, bool elementFormQualified)
        {
            this.IsCommandNS = isCommandNS;
                this.ElementFormQualified = elementFormQualified;

        }
                public bool IsCommandNS { get; set; }

        public bool ElementFormQualified { get; set; }
    }
}
