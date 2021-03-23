using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class IPV4HostAddress : HostNameType, IHostType
    {
        private string _address = "";
        [XmlAttribute]
        public string Address
        {
            get => _address;
            set => _address = value ?? "";
        }
    }
}
