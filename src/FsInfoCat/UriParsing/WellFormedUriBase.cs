using System.Text.RegularExpressions;

namespace FsInfoCat.UriParsing
{
    public class WellFormedUriBase : IAbsoluteURIBase
    {
        // TODO: Get real expression
        public static readonly Regex SchemeRegex = new Regex(@"^[a-z][a-z\d-]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static readonly Regex ParseRegex = new Regex(@"^(?<s>[^:;@/?#]+)((?<d>://)(?<a>[^:;/?#])|(?<d>:(//?)?))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public WellFormedUriBase(string scheme, string delimiter, WellFormedAuthority authority)
        {
            Scheme = scheme;
            Delimiter = delimiter;
            Authority = authority;
        }

        public string Scheme { get; }

        public string Delimiter { get; }

        public WellFormedAuthority Authority { get; }

        bool IUriComponent.IsWellFormed => true;

        IAuthority IAbsoluteURIBase.Authority => Authority;

        public static bool TryParse(string uriString, out int endIndex, out WellFormedUriBase result)
        {
            Match match;
            if (string.IsNullOrEmpty(uriString) || !(match = ParseRegex.Match(uriString)).Success)
            {
                endIndex = 0;
                result = null;
                return true;
            }

            string scheme = match.Groups["s"].Value;
            string delimiter = match.Groups["d"].Value;
            if (SchemeRegex.IsMatch(scheme))
            {
                Group group = match.Groups["a"];
                if (group.Success)
                {
                    if (WellFormedAuthority.TryParse(uriString, group.Index, out endIndex, out WellFormedAuthority authority))
                    {
                        result = new WellFormedUriBase(scheme, delimiter, authority);
                        return true;
                    }
                }
                else
                {
                    endIndex = match.Length;
                    result = new WellFormedUriBase(scheme, delimiter, null);
                    return true;
                }
            }
            result = null;
            endIndex = 0;
            return false;
        }
    }
}
