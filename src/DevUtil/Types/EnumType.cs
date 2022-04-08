using System.Xml.Serialization;

namespace DevUtil.Types
{
    [XmlRoot(RootElementName)]
    public class EnumType : DefinitionType
    {
        public const string RootElementName = "Enum";
    }
}
