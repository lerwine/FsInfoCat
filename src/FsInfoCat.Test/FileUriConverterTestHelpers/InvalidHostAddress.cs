using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class InvalidHostAddress : IHostType
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
