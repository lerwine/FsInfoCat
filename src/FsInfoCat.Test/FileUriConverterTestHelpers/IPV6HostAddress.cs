using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    [XmlRoot("IPV6")]
    public class IPV6HostAddress : HostNameType
    {
        [XmlAttribute]
        public bool IsBracketed { get; set; }

        [XmlAttribute]
        public bool IsUnc { get; set; }

        [XmlAttribute]
        public bool IsDns { get; set; }

        public override UriHostInfo ToUriHostInfo(int? port = null)
        {
            string name = (IsUnc) ?
                (
                    IsBracketed ? Address.Replace("[", "").Replace("]", "") : Address
                ).Replace("-", ":").Replace(".ipv6-literal.net", "") :
                (IsBracketed ? Address.Replace("[", "").Replace("]", "") : Address);
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
