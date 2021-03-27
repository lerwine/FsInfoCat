using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DevHelperGUI
{
    public class MatchResult : MatchItem
    {
        public string InputText { get; }

        [Obsolete]
        public MatchResult(Match match, int itemNumber = 0) : base(match)
        {
        }

        public MatchResult(string inputText, Match match, int itemNumber = 0) : base(match, itemNumber)
        {
            InputText = inputText ?? throw new ArgumentNullException(nameof(inputText));
        }

        internal static MatchResult EvaluateMatch(Regex regex, string inputText, int itemNumber = 0)
        {
            if (regex is null)
                throw new ArgumentNullException(nameof(regex));
            if (inputText is null)
                throw new ArgumentNullException(nameof(inputText));
            return new MatchResult(inputText, regex.Match(inputText), itemNumber);
        }
    }
}
