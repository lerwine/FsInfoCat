using System.Xml.Serialization;

namespace DevUtil.Types
{
    [XmlRoot(RootElementName)]
    public class EntityType : RefType
    {
        public const string RootElementName = "Entity";
    }
}
