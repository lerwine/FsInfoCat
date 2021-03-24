using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    [XmlRoot("HostName")]
    public class DnsOrBasicHostName : HostNameType
    {
        public override UriHostInfo ToUriHostInfo(int? port = null) => new UriHostInfo
        {
            Match = (port.HasValue) ? $"{Address}:{port.Value}" : Address,
            Value = Address,
            Port = port,
            Type = HostType.DNS
        };
    }
}
