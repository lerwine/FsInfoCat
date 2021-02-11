using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Xml;

namespace DevHelper
{
    public abstract class BaseElement
    {
        public PsHelpXmlDocument OwnerDocument { get; }

        protected XmlElement Xml { get; }

        protected ReadOnlyCollection<string> GetParaStrings(XmlElement element)
        {
            if (null == element || element.IsEmpty)
                return new ReadOnlyCollection<string>(new string[0]);
            return new ReadOnlyCollection<string>(element.SelectNodes(OwnerDocument.ToXPathName(PsHelpNodeName.para))
                .Cast<XmlElement>().Select(e => (e.IsEmpty) ? "" : e.InnerText)
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
                    element.AppendChild(PsHelpNodeName.para.CreateElement(OwnerDocument)).InnerText = s;
            }
        }

        protected XmlElement EnsureElement(PsHelpNodeName name, IEnumerable<PsHelpNodeName> sequence)
        {
            Monitor.Enter(Xml);
            try
            {
                return Xml.EnsureChildElementInSequence(name.LocalName(out string ns), ns,
                    sequence.Select(s => s.QualifiedName()));
            }
            finally { Monitor.Exit(Xml); }
        }

        protected BaseElement(XmlElement xmlElement)
        {
            if (xmlElement is null)
                throw new ArgumentNullException(nameof(xmlElement));
            OwnerDocument = (PsHelpXmlDocument)xmlElement.OwnerDocument;
            Xml = xmlElement;
        }

        internal static void AddTo(BaseElement item, XmlElement parentElement)
        {
            if (!ReferenceEquals(item.Xml.OwnerDocument, parentElement.OwnerDocument))
                throw new InvalidOperationException();
            if (item.Xml.ParentNode is null)
                parentElement.AppendChild(item.Xml);
            else if (!ReferenceEquals(item.Xml.ParentNode, parentElement))
                throw new InvalidOperationException();
        }

        internal static bool IsContainedBy(BaseElement item, XmlElement parentElement) => null != item && null != parentElement && ReferenceEquals(item.Xml.ParentNode, parentElement);

        internal static bool RemoveFrom(BaseElement item, XmlElement parentElement)
        {
            if (null != item && null != parentElement && ReferenceEquals(item.Xml.ParentNode, parentElement))
            {
                parentElement.RemoveChild(item.Xml);
                return true;
            }
            return false;
        }
    }
}
