using System.Text.RegularExpressions;

namespace FsInfoCat.UriParsing
{
    public class WellFormedUri : IAnyURI
    {
        // TODO: Get real expression
        public static readonly Regex FragmentRegex = new Regex(@"^[a-z][a-z\d-]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static readonly Regex ParseRegex = new Regex(@"\G(?<p>[^?#]*)(\?(?<q>[^#]*))?(#(?<f>.*))?$", RegexOptions.Compiled);

        private WellFormedUri(WellFormedUriBase absoluteBase, PathSegmentList<WellFormedPathSegment> segments, QueryParameterComponentList<WellFormedQueryParameter> query, string fragment)
        {
            AbsoluteBase = absoluteBase;
            Segments = segments;
            Query = query;
            Fragment = fragment;
        }

        public WellFormedUriBase AbsoluteBase { get; }

        public PathSegmentList<WellFormedPathSegment> Segments { get; }

        public QueryParameterComponentList<WellFormedQueryParameter> Query { get; }

        public string Fragment { get; }

        bool IUriComponent.IsWellFormed => true;

        IAbsoluteURIBase IAnyURI.AbsoluteBase => AbsoluteBase;

        IPathSegmentList<IUriPathSegment> IAnyURI.Segments => Segments?.GetGenericWraper();

        IUriComponentList<IUriParameterElement> IAnyURI.Query => Query?.GetGenericWraper();

        public static bool TryParse(string uriString, out WellFormedUri result)
        {
            if (string.IsNullOrEmpty(uriString))
            {
                result = new WellFormedUri(null, new PathSegmentList<WellFormedPathSegment>(true), null, null);
                return true;
            }
            Match match;
            if (WellFormedUriBase.TryParse(uriString, out int startIndex, out WellFormedUriBase absoluteBase) && (match = ParseRegex.Match(uriString, startIndex)).Success &&
                WellFormedPathSegment.TryParse(match.Groups["p"].Value, out PathSegmentList<WellFormedPathSegment> pathSegments))
            {
                Group group = match.Groups["f"];
                string fragment = (group.Success) ? group.Value : null;
                if ((string.IsNullOrEmpty(fragment) || FragmentRegex.IsMatch(fragment)) &&
                    WellFormedQueryParameter.TryParse(((group = match.Groups["q"]).Success) ? group.Value : null, out QueryParameterComponentList<WellFormedQueryParameter> query))
                {
                    result = new WellFormedUri(absoluteBase, pathSegments, query, fragment);
                    return true;
                }
            }
            result = null;
            return false;
        }
    }
}
