using System.Xml.Serialization;

namespace DevUtil.Types
{
    [XmlRoot(RootElementName)]
    public class InterfaceType : DefinitionType
    {
        public const string RootElementName = "Interface";
    }
}
