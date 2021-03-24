using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public abstract class HostNameType : BaseHostType
    {
        [XmlAttribute]
        public bool HasOuterWS { get; set; }

        private string _value = "";
        [XmlText]
        public string Value
        {
            get => _value;
            set => _value = value ?? "";
        }
        public abstract UriHostInfo ToUriHostInfo(int? port = null);
    }
}
