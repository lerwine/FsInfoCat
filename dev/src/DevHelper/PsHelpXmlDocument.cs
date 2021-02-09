using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;
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
        public XmlNamespaceManager NamespaceManager { get;  }
        private readonly XmlDocument _original = new XmlDocument();

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
            NamespaceManager = new XmlNamespaceManager(NameTable);
            foreach (NamespaceAttribute attr in (new XmlDocumentNamespace[] { XmlDocumentNamespace.msh })
                    .Concat(Enum.GetValues(typeof(XmlDocumentNamespace)).Cast<XmlDocumentNamespace>()).Select(ns => NamespaceAttribute.Map[ns])
                    .Where(a => a.IsCommandNS))
                NamespaceManager.AddNamespace(attr.Prefix, attr.URI);

        }

        public PsHelpXmlDocument()
        {
            AppendChild(CreateElement(PsHelpNodeName.helpItems)).Attributes.Append(CreateAttribute("schema")).Value = "maml";
            NamespaceManager = new XmlNamespaceManager(NameTable);
            foreach (NamespaceAttribute attr in (new XmlDocumentNamespace[] { XmlDocumentNamespace.msh })
                    .Concat(Enum.GetValues(typeof(XmlDocumentNamespace)).Cast<XmlDocumentNamespace>()).Select(ns => NamespaceAttribute.Map[ns])
                    .Where(a => a.IsCommandNS))
                NamespaceManager.AddNamespace(attr.Prefix, attr.URI);
        }

        private Dictionary<PsHelpNodeName, string> _xPathNameCache = new Dictionary<PsHelpNodeName, string>();

        public string ToXPathName(PsHelpNodeName name)
        {
            Monitor.Enter(_xPathNameCache);
            try
            {
                if (_xPathNameCache.ContainsKey(name))
                    return _xPathNameCache[name];
                XmlQNameAttribute qNameAttribute = XmlQNameAttribute.Map[name];
                NamespaceAttribute namespaceAttribute = NamespaceAttribute.Map[(qNameAttribute.Prefix == XmlDocumentNamespace.None) ? XmlDocumentNamespace.msh : qNameAttribute.Prefix];
                string p = GetPrefixOfNamespace(namespaceAttribute.URI);
                if (string.IsNullOrEmpty(p))
                    p = namespaceAttribute.Prefix;
                p = $"{p}:{qNameAttribute.LocalName}";
                _xPathNameCache.Add(name, p);
                return p;
            }
            finally { Monitor.Exit(_xPathNameCache);  }
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

        public bool FindCommand(string verb, string noun, out CommandElement result)
        {
            if (verb is null)
                throw new ArgumentNullException(nameof(verb));
            if (noun is null)
                throw new ArgumentNullException(nameof(noun));
            if (verb is null)
                throw new ArgumentNullException(nameof(verb));
            if (noun is null)
                throw new ArgumentNullException(nameof(noun));
            if (verb.Trim().Length == 0)
                throw new ArgumentNullException(nameof(verb));
            if (noun.Trim().Length == 0)
                throw new ArgumentNullException(nameof(noun));
            XmlConvert.VerifyNCName(verb);
            XmlConvert.VerifyNCName(noun);

            XmlElement commandElement = SelectSingleNode($"{ToXPathName(PsHelpNodeName.helpItems)}/{ToXPathName(PsHelpNodeName.command)}[{ToXPathName(PsHelpNodeName.verb)}='{verb}' and {ToXPathName(PsHelpNodeName.noun)}='{noun}']", NamespaceManager) as XmlElement;
            if (commandElement is null)
            {
                result = null;
                return false;
            }
            result = new CommandElement(commandElement);
            return true;
        }

        /// <summary>
        /// Creates new command if it doesn't already exist.
        /// </summary>
        /// <param name="verb">Name of verb</param>
        /// <param name="noun">Nane of noun</param>
        /// <param name="commandElement">The new or existing command element</param>
        /// <returns><c>true</c> if a new command was added; otherwise<c>false</c> to indicate that the command already exists.</returns>
        public bool TryCreateCommand(string verb, string noun, out CommandElement commandElement)
        {
            if (FindCommand(verb, noun, out commandElement))
                return false;
            XmlElement element = (XmlElement)DocumentElement.AppendChild(CreateElement(PsHelpNodeName.command));
            NamespaceAttribute xmlnsAttribute = NamespaceAttribute.Map[XmlDocumentNamespace.xmlns];
            NamespaceAttribute nsAttribute = NamespaceAttribute.Map[XmlDocumentNamespace.maml];
            element.Attributes.Append(CreateAttribute(xmlnsAttribute.Prefix, nsAttribute.Prefix, xmlnsAttribute.URI)).Value = nsAttribute.URI;
            nsAttribute = NamespaceAttribute.Map[XmlDocumentNamespace.command];
            element.Attributes.Append(CreateAttribute(xmlnsAttribute.Prefix, nsAttribute.Prefix, xmlnsAttribute.URI)).Value = nsAttribute.URI;
            nsAttribute = NamespaceAttribute.Map[XmlDocumentNamespace.dev];
            element.Attributes.Append(CreateAttribute(xmlnsAttribute.Prefix, nsAttribute.Prefix, xmlnsAttribute.URI)).Value = nsAttribute.URI;
            commandElement = new CommandElement(element);
            element = commandElement.DetailsElement;
            element.SelectSingleNode(ToXPathName(PsHelpNodeName.commandName)).InnerText = $"{verb}-{noun}";
            commandElement.Copyright = $"Copyright © Leonard Thomas Erwine {DateTime.Now.ToString("yyyy")}";
            element.SelectSingleNode(ToXPathName(PsHelpNodeName.commandName)).InnerText = verb;
            element.SelectSingleNode(ToXPathName(PsHelpNodeName.commandName)).InnerText = noun;
            commandElement.DescriptionElement.AppendChild(CreateComment("Description goes here"));
            return true;
        }
    }

    public abstract class BaseElement
    {
        public PsHelpXmlDocument OwnerDocument { get; }
        protected XmlElement Xml { get; }

        protected ReadOnlyCollection<string> GetParaStrings(XmlElement element)
        {
            if (null == element || element.IsEmpty)
                return new ReadOnlyCollection<string>(new string[0]);
            return new ReadOnlyCollection<string>(element.SelectNodes(OwnerDocument.ToXPathName(PsHelpNodeName.para)).Cast<XmlElement>().Select(e => (e.IsEmpty) ? "" : e.InnerText)
                .Where(e => e.Trim().Length > 0).ToArray());
        }

        protected void SetParaStrings(XmlElement element, IEnumerable<string> paragraphs)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));
            element.RemoveAll();
            if (paragraphs is null)
                return;
            foreach (string s in paragraphs)
            {
                if (!string.IsNullOrWhiteSpace(s))
                    element.AppendChild(OwnerDocument.CreateElement(PsHelpNodeName.para)).InnerText = s;
            }
        }

        protected XmlElement EnsureElement(PsHelpNodeName name)
        {
            Monitor.Enter(Xml);
            try
            {
                XmlElement element = Xml.SelectSingleNode(OwnerDocument.ToXPathName(name)) as XmlElement;
                if (element is null)
                    element = Xml.AppendChild(OwnerDocument.CreateElement(name)) as XmlElement;
                return element;
            }
            finally { Monitor.Exit(Xml); }
        }

        public BaseElement(XmlElement xmlElement)
        {
            if (xmlElement is null)
                throw new ArgumentNullException(nameof(xmlElement));
            OwnerDocument = (PsHelpXmlDocument)xmlElement.OwnerDocument;
            Xml = xmlElement;
        }
    }


    public class CommandElement : BaseElement
    {
        private XmlElement EnsureDetailsElement(PsHelpNodeName name)
        {
            Monitor.Enter(Xml);
            try
            {
                XmlElement element = Xml.SelectSingleNode($"{OwnerDocument.ToXPathName(PsHelpNodeName.details)}/{OwnerDocument.ToXPathName(name)}") as XmlElement;
                if (element is null)
                    element = DetailsElement.AppendChild(OwnerDocument.CreateElement(name)) as XmlElement;
                return element;
            }
            finally { Monitor.Exit(Xml); }
        }

        private string GetDetailsString(PsHelpNodeName name)
        {
            XmlElement element = EnsureDetailsElement(name);
            return (element.IsEmpty) ? "" : element.InnerText;
        }

        public string Name => GetDetailsString(PsHelpNodeName.commandName);

        public IEnumerable<string> Synopsis
        {
            get => GetParaStrings(EnsureDetailsElement(PsHelpNodeName.description));
            set => SetParaStrings(EnsureDetailsElement(PsHelpNodeName.description), value);
        }

        public string Verb => GetDetailsString(PsHelpNodeName.verb);

        public string Noun => GetDetailsString(PsHelpNodeName.noun);

        public string Copyright
        {
            get => GetDetailsString(PsHelpNodeName.copyright);
            set => EnsureDetailsElement(PsHelpNodeName.copyright).InnerText = value;
        }

        public Version Version
        {
            get
            {
                string s = GetDetailsString(PsHelpNodeName.version);
                if (s.Length > 0 && Version.TryParse(s, out Version result))
                    return result;
                return null;
            }
            set => EnsureDetailsElement(PsHelpNodeName.copyright).InnerText = (value is null) ? "" : value.ToString();
        }

        public IEnumerable<string> Description
        {
            get => GetParaStrings(DescriptionElement);
            set => SetParaStrings(DescriptionElement, value);
        }

        public XmlElement DetailsElement => EnsureElement(PsHelpNodeName.details);

        public XmlElement DescriptionElement => EnsureElement(PsHelpNodeName.description);

        public XmlElement SyntaxElement => EnsureElement(PsHelpNodeName.syntax);

        public XmlElement ParametersElement => EnsureElement(PsHelpNodeName.parameters);

        public XmlElement InputTypesElement => EnsureElement(PsHelpNodeName.inputTypes);

        public XmlElement ReturnValuesElement => EnsureElement(PsHelpNodeName.returnValues);

        public XmlElement TerminatingErrorsElement => EnsureElement(PsHelpNodeName.terminatingErrors);

        public XmlElement NonTerminatingErrorsElement => EnsureElement(PsHelpNodeName.nonTerminatingErrors);

        public XmlElement AlertSetElement => EnsureElement(PsHelpNodeName.alertSet);

        public XmlElement ExamplesElement => EnsureElement(PsHelpNodeName.examples);

        public XmlElement RelatedLinksElement => EnsureElement(PsHelpNodeName.relatedLinks);

        public CommandElement(XmlElement commandElement) : base(commandElement)
        {
            foreach (PsHelpNodeName name in new PsHelpNodeName[] { PsHelpNodeName.details, PsHelpNodeName.description, PsHelpNodeName.syntax, PsHelpNodeName.parameters,
                    PsHelpNodeName.inputTypes, PsHelpNodeName.returnValues, PsHelpNodeName.terminatingErrors, PsHelpNodeName.nonTerminatingErrors,
                    PsHelpNodeName.alertSet, PsHelpNodeName.examples, PsHelpNodeName.relatedLinks })
                EnsureElement(name);
            foreach (PsHelpNodeName name in new PsHelpNodeName[] { PsHelpNodeName.name, PsHelpNodeName.description, PsHelpNodeName.copyright, PsHelpNodeName.verb,
                    PsHelpNodeName.noun, PsHelpNodeName.version })
                EnsureDetailsElement(name);
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

        [XmlQName(XmlDocumentNamespace.dev)]
        version,

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
