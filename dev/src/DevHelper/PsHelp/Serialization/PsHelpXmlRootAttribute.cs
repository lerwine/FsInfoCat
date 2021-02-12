using System.Xml.Serialization;

namespace DevHelper.PsHelp.Serialization
{
    public class PsHelpXmlRootAttribute : XmlRootAttribute
    {
        public PsHelpXmlRootAttribute(ElementName elementName) : base(elementName.LocalName())
        {
            Namespace = elementName.NamespaceDefinition().URL();
        }
    }
}
