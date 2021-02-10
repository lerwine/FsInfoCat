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
}
