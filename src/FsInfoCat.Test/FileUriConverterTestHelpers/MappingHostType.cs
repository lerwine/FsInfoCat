using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public enum MappingHostType
    {
        Unknown,
        DNS,
        IPV4,
        IPV6,
        [XmlEnum(Name = "IPV6.UNC")]
        IPV6_UINC
    }
}
