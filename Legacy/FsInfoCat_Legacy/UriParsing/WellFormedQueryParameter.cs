using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace FsInfoCat.UriParsing
{
    public class WellFormedQueryParameter : IUriParameterElement
    {
        // TODO: Get real expression
        public static readonly Regex KeyRegex = new Regex(@"^[a-z][a-z\d-]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        // TODO: Get real expression
        public static readonly Regex ValueRegex = new Regex(@"^[a-z][a-z\d-]*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static readonly Regex ParseRegex = new Regex(@"\G((?<k>[^=&]*)=)?(?<v>[^&]*)(?=&|$)", RegexOptions.Compiled);

        public WellFormedQueryParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        bool IUriComponent.IsWellFormed => true;

        public static bool TryParse(string source, out QueryParameterComponentList<WellFormedQueryParameter> result)
        {
            if (source is null)
                result = null;
            else if (source.Length == 0)
                result = new QueryParameterComponentList<WellFormedQueryParameter>(true);
            else
            {
                MatchCollection matches = ParseRegex.Matches(source);
                Match match;
                if (matches.Count == 0 || ((match = matches[matches.Count - 1]).Index + match.Length) < source.Length)
                {
                    result = null;
                    return false;
                }
                Collection<WellFormedQueryParameter> items = new Collection<WellFormedQueryParameter>();
                foreach (Match m in matches)
                {
                    Group group = m.Groups["k"];

                    string key;
                    if (group.Success)
                    {
                        key = group.Value;
                        if (key.Length > 0 && !KeyRegex.IsMatch(key))
                        {
                            result = null;
                            return false;
                        }
                    }
                    else
                        key = null;
                    string value = m.Groups["v"].Value;
                    if (value.Length == 0 || ValueRegex.IsMatch(value))
                        items.Add(new WellFormedQueryParameter(key, value));
                    else
                    {
                        result = null;
                        return false;
                    }
                }
                result = new QueryParameterComponentList<WellFormedQueryParameter>(items);
            }
            return true;
        }
    }
}
