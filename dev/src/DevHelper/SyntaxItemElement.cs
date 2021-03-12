using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using FsInfoCat.Util;

namespace DevHelper
{
    public class SyntaxItemElement : BaseElement
    {
        public const string URI_NAMESPACE_PARAMETER_SET = "parameterSet";

        private static readonly ReadOnlyCollection<PsHelpNodeName> _allElements = new ReadOnlyCollection<PsHelpNodeName>(
            new PsHelpNodeName[] { PsHelpNodeName.name, PsHelpNodeName.parameter }
        );

        public string ParameterSetName
        {
            get
            {
                XmlAttribute attribute = (XmlAttribute)Xml.SelectSingleNode("@attribute");
                if (!(attribute is null))
                {
                    string[] segments = attribute.Value.Split(":");
                    if (segments.Length == 4 && segments[0] == UriHelper.URI_SCHEME_URN && segments[1] == URI_NAMESPACE_PARAMETER_SET && segments[2] == UriHelper.URN_NAMESPACE_ID)
                        return Uri.UnescapeDataString(segments[3]);
                }

                return null;
            }
            set
            {
                XmlAttribute attribute = (XmlAttribute)Xml.SelectSingleNode("@attribute");
                if (value is null)
                {
                    if (!(attribute is null))
                        Xml.Attributes.Remove(attribute);
                }
                else
                    ((attribute is null) ? Xml.Attributes.Append(OwnerDocument.CreateAttribute("attribute")) : attribute).Value = $"{UriHelper.URI_SCHEME_URN}:{URI_NAMESPACE_PARAMETER_SET}:{UriHelper.URN_NAMESPACE_ID}:{Uri.EscapeDataString(value)}";
            }
        }

        public string Name
        {
            get => (NameElement.IsEmpty) ? "" : NameElement.InnerText;
            set => NameElement.InnerText = value;
        }

        internal XmlElement NameElement => EnsureElement(PsHelpNodeName.name, _allElements);

        public IEnumerable<SyntaxParameterElement> GetParameterElements()
        {
            throw new NotImplementedException();
        }

        public SyntaxItemElement(XmlElement xmlElement) : base(xmlElement)
        {
            if (!PsHelpNodeName.syntaxItem.IsMatch(xmlElement))
                throw new ArgumentOutOfRangeException(nameof(xmlElement));
            foreach (PsHelpNodeName name in _allElements)
                EnsureElement(name, _allElements);
        }
    }
}
