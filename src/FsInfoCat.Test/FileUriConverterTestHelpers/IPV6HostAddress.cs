using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    [XmlRoot("IPV6")]
    public class IPV6HostAddress : HostNameType
    {
        [XmlAttribute]
        public IPV6Type Type { get; set; }

        [XmlAttribute]
        public bool IsDns { get; set; }

        public override UriHostInfo ToUriHostInfo(int? port = null)
        {
            string name = Type switch
            {
                IPV6Type.UNC => Address.ToLower().Replace("-", ":").Replace(".ipv6-literal.net", ""),
                IPV6Type.Bracketed => Address.Replace("[", "").Replace("]", ""),
                _ => Address
            };
            return new UriHostInfo
            {
                Match = (port.HasValue) ? $"{name}:{port.Value}" : name,
                Value = name,
                Port = port,
                Type = HostType.IPV6
            };
        }
    }
}
