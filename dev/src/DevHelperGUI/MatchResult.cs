using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevHelperGUI
{
    public class MatchResult : CaptureResult
    {
        public int ItemNumber { get; }
        public BindingList<GroupResult> Groups { get; }

        public MatchResult(Match match, int itemNumber = 1)
            : base(match)
        {
            ItemNumber = itemNumber;
            Groups = new BindingList<GroupResult>(match.Groups.Cast<Group>().Select(g => new GroupResult(g)).ToList());
        }

    }
}
