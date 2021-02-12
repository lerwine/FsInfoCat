using System.Xml.Serialization;

namespace DevHelper.PsHelp.Serialization
{
    public class PsHelpXmlArrayAttribute : XmlArrayAttribute
    {
        public PsHelpXmlArrayAttribute(ElementName elementName) : base(elementName.LocalName())
        {
            Namespace = elementName.NamespaceDefinition().URL();
        }
    }
}
