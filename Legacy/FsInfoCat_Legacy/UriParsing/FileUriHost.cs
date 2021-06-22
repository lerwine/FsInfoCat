using System;

namespace FsInfoCat.UriParsing
{
    public class FileUriHost : IAuthority
    {
        public string Host { get; }

        public UriHostNameType HostNameType { get; }

        IUserInfo IAuthority.UserInfo => null;

        string IAuthority.Port => null;

        int? IAuthority.PortNumber => null;

        bool IUriComponent.IsWellFormed => true;
    }
}