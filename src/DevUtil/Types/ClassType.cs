using System.Xml.Serialization;

namespace DevUtil.Types
{
    [XmlRoot(RootElementName)]
    public class ClassType : RefType
    {
        public const string RootElementName = "Class";
    }
}
