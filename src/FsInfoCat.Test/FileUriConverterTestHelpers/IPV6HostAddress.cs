namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class IPV6HostAddress : HostNameType, IHostType
    {
        private string _address = "";
        public string Address
        {
            get => _address;
            set => _address = value ?? "";
        }

        public IPV6Type Type { get; set; }

        public bool IsDns { get; set; }
    }
}
