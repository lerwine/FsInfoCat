using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Xml;

namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Extension methods for building <seealso cref="XmlNode">XmlNodes</seealso> using method chaining.
    /// </summary>
    public static class XmlExtensions
    {
        public static XmlElement NewDocument([NotNull] string localName)
        {
            XmlDocument ownerDocument = new XmlDocument();
            return (XmlElement)ownerDocument.AppendChild(ownerDocument.CreateElement(localName));
        }

        public static XmlElement AppendElement([NotNull] this XmlElement parent, [NotNull] string localName) => (XmlElement)parent.AppendChild(parent.OwnerDocument.CreateElement(localName));

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string value)
        {
            XmlAttribute attribute = parent.GetAttributeNode(localName);
            if (attribute is null)
            {
                if (value is null)
                    return parent;
                parent.SetAttribute(localName, value);
            }
            else if (value is null)
                parent.Attributes.Remove(attribute);
            else
                attribute.Value = value;
            return parent;
        }

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, string value)
        {
            if (value is null)
            {
                if (parent.IsEmpty)
                    return parent;
                if (parent.SelectSingleNode("*") is null)
                    parent.IsEmpty = true;
                XmlCharacterData[] textNodes = parent.SelectNodes("text()").Cast<XmlCharacterData>().Where(n => n.InnerText.Trim().Length > 0).ToArray();
                if (textNodes.Length > 0 || (textNodes = parent.SelectNodes("text()").Cast<XmlCharacterData>().Where(n => n.InnerText.Length > 0).ToArray()).Length > 0)
                    foreach (XmlCharacterData cd in textNodes)
                        parent.RemoveChild(cd);
            }
            else if (parent.IsEmpty || parent.SelectSingleNode("*") is null)
                parent.InnerText = value;
            else
            {
                XmlCharacterData[] textNodes = parent.SelectNodes("text()").Cast<XmlCharacterData>().ToArray();
                int len = textNodes.Length - 1;
                if (len > 0)
                {
                    if (textNodes.Any(n => n.InnerText.Trim().Length > 0))
                        textNodes = textNodes.Where(n => n.InnerText.Trim().Length > 0).ToArray();
                    else if (textNodes.Any(n => n.InnerText.Length > 0))
                        textNodes = textNodes.Where(n => n.InnerText.Length > 0).ToArray();
                    len = textNodes.Length - 1;
                    if (len > 0)
                        foreach (XmlCharacterData cd in textNodes.Take(len))
                            parent.RemoveChild(cd);
                }
                if (len < 0)
                    parent.AppendChild(parent.OwnerDocument.CreateTextNode(value));
                else
                {
                    XmlCharacterData cd = textNodes[len];
                    if (cd is XmlCDataSection xmlCData)
                        xmlCData.InnerText = value;
                    else if (cd is XmlText xmlText)
                        xmlText.InnerText = value;
                    else if (cd is XmlSignificantWhitespace significantWhitespace && value.Trim().Length == 0)
                        significantWhitespace.InnerText = value;
                    else
                        parent.ReplaceChild(parent.OwnerDocument.CreateTextNode(value), cd);
                }
            }
            return parent;
        }
    }
}
