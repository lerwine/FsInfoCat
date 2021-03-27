using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DevHelperGUI
{
    public class MatchCollectionResult
    {
        public int ItemNumber { get; }
        public int MatchCount => Matches.Count;
        public string InputText { get; }
        public List<MatchItem> Matches { get; }

        public MatchCollectionResult(string inputText, MatchCollection matchCollection, int itemNumber = 0)
        {
            ItemNumber = itemNumber;
            InputText = inputText ?? throw new ArgumentNullException(nameof(inputText));
            Matches = matchCollection.Cast<Match>().Select((m, i) => new MatchItem(m, i + 1)).ToList();
        }

        internal static MatchCollectionResult EvaluateMatches(Regex regex, string inputText, int itemNumber = 0)
        {
            if (regex is null)
                throw new ArgumentNullException(nameof(regex));
            if (inputText is null)
                throw new ArgumentNullException(nameof(inputText));
            return new MatchCollectionResult(inputText, regex.Matches(inputText), itemNumber);
        }

        internal XElement ToXElement()
        {
            throw new NotImplementedException();
        }
    }
}
