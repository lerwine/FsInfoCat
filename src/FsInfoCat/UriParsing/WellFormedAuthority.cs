using System;
using System.Text.RegularExpressions;

namespace FsInfoCat.UriParsing
{
    public class WellFormedAuthority : IAuthority
    {
        // TODO: Get real expression
        public static readonly Regex HostNameRegex = new Regex(@"^[a-z][a-z\d-]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static readonly Regex ParseRegex = new Regex(@"\G
(
    (?<u>[^:;/?#@]*)
    (
        :
        (?<c>[^;/?#@]*)
    )?
    @
)?
(?<h>[^:;/?#@]+)
(
    :
    (?<p>\d*)
)?
(?=[/?#;]|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        private WellFormedAuthority(WellFormedUserInfo userInfo, string hostName, UriHostNameType hostNameType, string portString, ushort? portNumber)
        {
            UserInfo = userInfo;
            Host = hostName;
            HostNameType = hostNameType;
            Port = portString;
            PortNumber = portNumber;
        }

        public WellFormedUserInfo UserInfo { get; }

        public string Host { get; }

        public UriHostNameType HostNameType { get; }

        public string Port { get; }

        public int? PortNumber { get; }

        bool IUriComponent.IsWellFormed => true;

        IUserInfo IAuthority.UserInfo => UserInfo;

        internal static bool TryParse(string uriString, int startat, out int endIndex, out WellFormedAuthority result)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                endIndex = startat;
                result = null;
                return startat == 0;
            }
            Match match = ParseRegex.Match(uriString, startat);
            if (match.Success)
            {
                endIndex = startat + match.Length;
                string hostName = match.Groups["h"].Value;
                UriHostNameType hostNameType;
                Match hm = HostNameRegex.Match(hostName);
                if (!hm.Success)
                {
                    endIndex = startat;
                    result = null;
                    return false;
                }
                Group group = hm.Groups["ipv6"];
                if (group.Success)
                {
                    hostNameType = UriHostNameType.IPv6;
                    hostName = group.Value;
                }
                else if (hm.Groups["ipv2"].Success)
                    hostNameType = UriHostNameType.IPv4;
                else if (hm.Groups["dns"].Success)
                    hostNameType = UriHostNameType.Dns;
                else
                    hostNameType = UriHostNameType.Basic;

                group = match.Groups["p"];
                string portString;
                ushort? portNumber;
                if (group.Success)
                {
                    portString = group.Value;
                    if (group.Length == 0)
                        portNumber = null;
                    else if (uint.TryParse(portString, out uint p) && p < 65536)
                        portNumber = (ushort)p;
                    else
                    {
                        endIndex = startat;
                        result = null;
                        return false;
                    }
                }
                else
                {
                    portString = null;
                    portNumber = null;
                }
                group = match.Groups["u"];
                if (group.Success)
                {
                    if (WellFormedUserInfo.TryCreate(group.Value, ((group = match.Groups["p"]) is null) ? null : group.Value, out WellFormedUserInfo userInfo))
                        result = new WellFormedAuthority(userInfo, hostName, hostNameType, portString, portNumber);
                    else
                    {
                        endIndex = startat;
                        result = null;
                        return false;
                    }
                }
                else
                    result = new WellFormedAuthority(null, hostName, hostNameType, portString, portNumber);
                return true;
            }
            endIndex = startat;
            result = null;
            return false;
        }
    }
}
