using System;

namespace FsInfoCat.UriParsing
{
    public class Authority : IAuthority
    {
        public UserInfo UserInfo { get; }

        public string Host { get; }

        public UriHostNameType HostNameType { get; }

        public string Port { get; }

        public int? PortNumber { get; }

        public bool IsWellFormed { get; }

        IUserInfo IAuthority.UserInfo => UserInfo;
    }
}
