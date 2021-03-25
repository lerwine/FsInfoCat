using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevHelperGUI
{
    public class MatchCollectionResult
    {
        public BindingList<MatchResult> Matches { get; }

        public MatchCollectionResult(MatchCollection matchCollection)
        {
            Matches = new BindingList<MatchResult>(matchCollection.Cast<Match>().Select((m, i) => new MatchResult(m, i + 1)).ToList());
        }

    }
}
