using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml;

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
        private Dictionary<PsHelpNodeName, string> _xPathNameCache = new Dictionary<PsHelpNodeName, string>();
        private FileInfo _savedLocation;
        private bool _hasChanges;
        public XmlNamespaceManager NamespaceManager { get;  }
        private readonly XmlDocument _original = new XmlDocument();

        public bool IsPsHelpXmlDocument(XmlDocument source)
        {
            XmlElement documentElement;
            if (!(source is null || (documentElement = source.DocumentElement) is null) &&
                PsHelpNodeName.helpItems.IsMatch(documentElement))
            {
                XmlAttribute xmlAttribute = documentElement.SelectSingleNode("@" + XML_ATTRIBUTE_NAME_SCHEMA) as XmlAttribute;
                return (null != xmlAttribute && xmlAttribute.Value == XML_ATTRIBUTE_VALUE_MAML);
            }
            return false;
        }

        public PsHelpXmlDocument(string path)
        {
            _savedLocation = (string.IsNullOrEmpty(path)) ? null : new FileInfo(path);
            _hasChanges = _savedLocation is null || !_savedLocation.Exists;
            if (_hasChanges)
                AppendChild(PsHelpNodeName.helpItems.CreateElement(this)).Attributes.Append(CreateAttribute(XML_ATTRIBUTE_NAME_SCHEMA)).Value = XML_ATTRIBUTE_VALUE_MAML;
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
            NamespaceManager.AddNamespace(XmlDocumentNamespace.msh.Prefix(), XmlDocumentNamespace.msh.URI());
            foreach (XmlDocumentNamespace ns in XmlNameHelper.CommandNsNamespaces)
                NamespaceManager.AddNamespace(ns.Prefix(), ns.URI());
        }

        public PsHelpXmlDocument()
        {
            AppendChild(PsHelpNodeName.helpItems.CreateElement(this)).Attributes.Append(CreateAttribute("schema")).Value = "maml";
            NamespaceManager = new XmlNamespaceManager(NameTable);
            NamespaceManager.AddNamespace(XmlDocumentNamespace.msh.Prefix(), XmlDocumentNamespace.msh.URI());
            foreach (XmlDocumentNamespace ns in XmlNameHelper.CommandNsNamespaces)
                NamespaceManager.AddNamespace(ns.Prefix(), ns.URI());
        }


        public string ToXPathName(PsHelpNodeName name)
        {
            Monitor.Enter(_xPathNameCache);
            try
            {
                if (_xPathNameCache.ContainsKey(name))
                    return _xPathNameCache[name];
                string localName = name.LocalName(out string namespaceURI);
                // XmlQNameAttribute qNameAttribute = XmlQNameAttribute.Map[name];
                // NamespaceAttribute namespaceAttribute = NamespaceAttribute.Map[(qNameAttribute.Prefix == XmlDocumentNamespace.None) ? XmlDocumentNamespace.msh : qNameAttribute.Prefix];
                string p = GetPrefixOfNamespace(namespaceURI);
                if (string.IsNullOrEmpty(p))
                    p = name.Prefix();
                p = $"{p}:{localName}";
                _xPathNameCache.Add(name, p);
                return p;
            }
            finally { Monitor.Exit(_xPathNameCache);  }
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
            XmlElement element = (XmlElement)DocumentElement.AppendChild(PsHelpNodeName.command.CreateElement(this));
            // NamespaceAttribute xmlnsAttribute = NamespaceAttribute.Map[XmlDocumentNamespace.xmlns];
            // NamespaceAttribute nsAttribute = NamespaceAttribute.Map[XmlDocumentNamespace.maml];

            element.Attributes.Append(CreateAttribute(XmlDocumentNamespace.xmlns.Prefix(),
                XmlDocumentNamespace.maml.Prefix(),
                XmlDocumentNamespace.xmlns.URI())).Value = XmlDocumentNamespace.maml.URI();
            element.Attributes.Append(CreateAttribute(XmlDocumentNamespace.xmlns.Prefix(),
                XmlDocumentNamespace.command.Prefix(),
                XmlDocumentNamespace.xmlns.URI())).Value = XmlDocumentNamespace.command.URI();
            element.Attributes.Append(CreateAttribute(XmlDocumentNamespace.xmlns.Prefix(),
                XmlDocumentNamespace.dev.Prefix(),
                XmlDocumentNamespace.xmlns.URI())).Value = XmlDocumentNamespace.dev.URI();
            commandElement = new CommandElement(element);
#warning Need to move functionality to CommandElement class or perhaps to a builder?
            // element = commandElement.DetailsElement;
            // element.SelectSingleNode(ToXPathName(PsHelpNodeName.commandName)).InnerText = $"{verb}-{noun}";
            // commandElement.Copyright = $"Copyright © Leonard Thomas Erwine {DateTime.Now.ToString("yyyy")}";
            // element.SelectSingleNode(ToXPathName(PsHelpNodeName.commandName)).InnerText = verb;
            // element.SelectSingleNode(ToXPathName(PsHelpNodeName.commandName)).InnerText = noun;
            // commandElement.DescriptionElement.AppendChild(CreateComment("Description goes here"));
            return true;
        }
    }
}
