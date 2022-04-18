using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace DevUtil
{
    public abstract partial class PsXmlNode
    {
        public class Document
        {
            private readonly XDocument _document;

            public PsXmlElement Root { get; }

            public Document(string localName, string namespaceName)
            {
                Root = new PsXmlElement(localName, namespaceName);
                (_document = new XDocument((XElement)Root.GetXNode())).AddAnnotation(this);
            }

            public Document(string expandedName)
            {
                Root = new PsXmlElement(expandedName);
                (_document = new XDocument((XElement)Root.GetXNode())).AddAnnotation(this);
            }

            private Document(XDocument document)
            {
                Root = PsXmlElement.GetAssociatedNode((_document = document).Root);
                document.AddAnnotation(this);
            }

            public static Document Parse(string text, bool preserveWhitespace, bool setLineInfo)
            {
                XDocument document = XDocument.Parse(text, preserveWhitespace ? (setLineInfo ? LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo : LoadOptions.PreserveWhitespace) :
                    setLineInfo ? LoadOptions.SetLineInfo : LoadOptions.None);
                if (document.Root is null)
                    throw new ArgumentException($"{nameof(text)} does not contain a root element", nameof(text));
                return new Document(document);
            }

            public static Document Load(string uri, bool preserveWhitespace, bool setLineInfo)
            {
                XDocument document = XDocument.Load(uri, preserveWhitespace ? (setLineInfo ? LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo : LoadOptions.PreserveWhitespace) :
                    setLineInfo ? LoadOptions.SetLineInfo : LoadOptions.None);
                if (document.Root is null)
                    throw new ArgumentException($"Document at {nameof(uri)} does not contain a root element", nameof(uri));
                return new Document(document);
            }

            public static Document Load(XmlReader reader, bool preserveWhitespace, bool setLineInfo)
            {
                XDocument document = XDocument.Load(reader, preserveWhitespace ? (setLineInfo ? LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo : LoadOptions.PreserveWhitespace) :
                    setLineInfo ? LoadOptions.SetLineInfo : LoadOptions.None);
                if (document.Root is null)
                    throw new ArgumentException($"Document from {nameof(reader)} does not contain a root element", nameof(reader));
                return new Document(document);
            }

            public static Document Load(Stream stream, bool preserveWhitespace, bool setLineInfo)
            {
                XDocument document = XDocument.Load(stream, preserveWhitespace ? (setLineInfo ? LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo : LoadOptions.PreserveWhitespace) :
                    setLineInfo ? LoadOptions.SetLineInfo : LoadOptions.None);
                if (document.Root is null)
                    throw new ArgumentException($"Document from {nameof(stream)} does not contain a root element", nameof(stream));
                return new Document(document);
            }

            public static Document Load(TextReader textReader, bool preserveWhitespace, bool setLineInfo)
            {
                XDocument document = XDocument.Load(textReader, preserveWhitespace ? (setLineInfo ? LoadOptions.PreserveWhitespace | LoadOptions.SetLineInfo : LoadOptions.PreserveWhitespace) :
                    setLineInfo ? LoadOptions.SetLineInfo : LoadOptions.None);
                if (document.Root is null)
                    throw new ArgumentException($"Document from {nameof(textReader)} does not contain a root element", nameof(textReader));
                return new Document(document);
            }

            public void Save(Stream stream, bool omitDuplicateNamespaces, bool disableFormattingsetLineInfo) => _document.Save(stream, omitDuplicateNamespaces ?
                (disableFormattingsetLineInfo ? SaveOptions.OmitDuplicateNamespaces | SaveOptions.DisableFormatting : SaveOptions.OmitDuplicateNamespaces) :
                    disableFormattingsetLineInfo ? SaveOptions.DisableFormatting : SaveOptions.None);

            public void Save(string fileName, bool omitDuplicateNamespaces, bool disableFormattingsetLineInfo) => _document.Save(fileName, omitDuplicateNamespaces ?
                (disableFormattingsetLineInfo ? SaveOptions.OmitDuplicateNamespaces | SaveOptions.DisableFormatting : SaveOptions.OmitDuplicateNamespaces) :
                    disableFormattingsetLineInfo ? SaveOptions.DisableFormatting : SaveOptions.None);

            public void Save(XmlWriter writer) => _document.Save(writer);

            public void Save(TextWriter textWriter, bool omitDuplicateNamespaces, bool disableFormattingsetLineInfo) => _document.Save(textWriter, omitDuplicateNamespaces ?
                (disableFormattingsetLineInfo ? SaveOptions.OmitDuplicateNamespaces | SaveOptions.DisableFormatting : SaveOptions.OmitDuplicateNamespaces) :
                    disableFormattingsetLineInfo ? SaveOptions.DisableFormatting : SaveOptions.None);
        }

        public class PsXmlEqualityComparer : IEqualityComparer<PsXmlNode>
        {
            private readonly XNodeEqualityComparer _equalityComparer;

            public static PsXmlEqualityComparer Instance { get; } = new();

            private PsXmlEqualityComparer() => _equalityComparer = XNode.EqualityComparer;

            public bool Equals(PsXmlNode x, PsXmlNode y) => (x is null) ? y is null : x is not null && (ReferenceEquals(x, y) || _equalityComparer.Equals(x.GetXNode(), y.GetXNode()));

            public int GetHashCode([DisallowNull] PsXmlNode obj) => (obj is null) ? 0 : _equalityComparer.GetHashCode(obj.GetXNode());
        }
    }
}
