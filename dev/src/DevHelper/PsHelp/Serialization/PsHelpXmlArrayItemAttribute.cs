using System;
using System.Xml.Serialization;

namespace DevHelper.PsHelp.Serialization
{
    public class PsHelpXmlArrayItemAttribute : XmlArrayItemAttribute
    {
        public PsHelpXmlArrayItemAttribute(ElementName elementName) : base(elementName.LocalName())
        {
            Namespace = elementName.NamespaceDefinition().URL();
        }

        public PsHelpXmlArrayItemAttribute(ElementName elementName, Type type) : base(elementName.LocalName(), type)
        {
            Namespace = elementName.NamespaceDefinition().URL();
        }
    }
}
