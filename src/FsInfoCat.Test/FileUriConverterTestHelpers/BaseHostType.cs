using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public abstract class BaseHostType
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
