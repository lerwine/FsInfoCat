using System.Xml.Serialization;

namespace DevHelper.PsHelp.Serialization
{
    public class PsHelpXmlElementAttribute : XmlElementAttribute
    {
        public PsHelpXmlElementAttribute(ElementName elementName) : base(elementName.LocalName())
        {
            Namespace = elementName.NamespaceDefinition().URL();
        }
    }
}
