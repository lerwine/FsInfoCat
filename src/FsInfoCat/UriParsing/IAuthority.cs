using System;

namespace FsInfoCat.UriParsing
{
    public interface IAuthority : IUriComponent
    {
        IUserInfo UserInfo { get; }
        string Host { get; }
        UriHostNameType HostNameType { get; }
        string Port { get; }
        int? PortNumber { get; }
    }
}
