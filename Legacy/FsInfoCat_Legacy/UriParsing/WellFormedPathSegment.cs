using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace FsInfoCat.UriParsing
{
    public class WellFormedPathSegment : IUriPathSegment
    {
        // TODO: Get real expression
        public static readonly Regex NameRegex = new Regex(@"^[a-z][a-z\d-]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static readonly Regex ParseRegex = new Regex(@"\G
(?<d>[:/])?
(?<n>[^:;/]*)
(;(?<p>[^:/]*))?
(?=[:/]|$)", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        private WellFormedPathSegment(char? delimiter, string name, SegmentParameterComponentList<WellFormedSegmentParameter> parameters)
        {
            // TODO: Failing to create parent...
            Delimiter = delimiter;
            Name = name;
            Parameters = parameters;
        }

        public char? Delimiter { get; }

        public string Name { get; }

        public SegmentParameterComponentList<WellFormedSegmentParameter> Parameters { get; }

        public WellFormedUri Parent { get; }

        bool IUriComponent.IsWellFormed => true;

        IUriComponentList<IUriParameterElement> IUriPathSegment.Parameters => Parameters?.GetGenericWraper();

        IAnyURI IUriPathSegment.Parent => Parent;

        public static bool TryParse(string source, out PathSegmentList<WellFormedPathSegment> result)
        {
            if (source is null)
                result = null;
            else if (source.Length == 0)
                result = new PathSegmentList<WellFormedPathSegment>(true);
            else
            {
                MatchCollection matches = ParseRegex.Matches(source);
                Match match;
                if (matches.Count == 0 || ((match = matches[matches.Count - 1]).Index + match.Length) < source.Length)
                {
                    result = null;
                    return false;
                }
                Collection<WellFormedPathSegment> items = new Collection<WellFormedPathSegment>();
                foreach (Match m in matches)
                {
                    string name = m.Groups["n"].Value;
                    if (name.Length > 0 && !NameRegex.IsMatch(name))
                    {
                        result = null;
                        return false;
                    }
                    Group group = m.Groups["p"];
                    SegmentParameterComponentList<WellFormedSegmentParameter> parameters;
                    if (group.Success)
                    {
                        if (!WellFormedSegmentParameter.TryParse(group.Value, out parameters))
                        {
                            result = null;
                            return false;
                        }
                    }
                    else
                        parameters = null;
                    items.Add(new WellFormedPathSegment((group = m.Groups["d"]).Success ? (char?)group.Value[0] : null, name, parameters));
                }
                result = new PathSegmentList<WellFormedPathSegment>(items);
            }
            return true;
        }
    }
}
