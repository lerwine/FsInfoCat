using System.Xml.Serialization;

namespace DevUtil.Types
{
    [XmlRoot(RootElementName)]
    public class StructType : DefinitionType
    {
        public const string RootElementName = "Struct";
    }
}
