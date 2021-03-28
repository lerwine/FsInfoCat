using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace DevHelperGUI
{
    internal class MultiRegexEvaluatableEventArgs : RegexEvaluatableEventArgs
    {
        private readonly ReadOnlyCollection<string> _allInputs;

        public MultiRegexEvaluatableEventArgs(Regex regex, string[] inputs)
            : base(regex)
        {
            _allInputs = new ReadOnlyCollection<string>((inputs is null) ? new string[] { "" } : inputs.Select(s => s ?? "").DefaultIfEmpty("").ToArray());
        }

        public override bool IsSingleInput => false;

        public override string PrimaryInput => _allInputs[0];

        public override IEnumerable<string> AllInputs => _allInputs;
    }
}
