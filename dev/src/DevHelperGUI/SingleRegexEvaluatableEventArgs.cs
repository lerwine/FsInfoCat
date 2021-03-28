using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DevHelperGUI
{
    internal class SingleRegexEvaluatableEventArgs : RegexEvaluatableEventArgs
    {
        private readonly string _input;

        public SingleRegexEvaluatableEventArgs(Regex regex, string input)
            : base(regex)
        {
            _input = input;
        }

        public override bool IsSingleInput => true;

        public override string PrimaryInput => _input;

        public override IEnumerable<string> AllInputs
        {
            get
            {
                yield return _input;
            }
        }
    }
}
