using System;
using System.Collections.Generic;
using System.Text;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public class DnsOrBasicHostName : HostNameType, IHostType
    {
        private string _address = "";
        public string Address
        {
            get => _address;
            set => _address = value ?? "";
        }
    }
}
