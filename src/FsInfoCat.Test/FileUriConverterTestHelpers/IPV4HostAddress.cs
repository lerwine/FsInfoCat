using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    [XmlRoot("IPV6")]
    public class IPV4HostAddress : HostNameType
    {
        public override UriHostInfo ToUriHostInfo(int? port = null) => new UriHostInfo
        {
            Match = (port.HasValue) ? $"{Address}:{port.Value}" : Address,
            Value = Address,
            Port = port,
            Type = HostType.IPV4
        };
    }
}
