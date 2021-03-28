using System;
using System.Text.RegularExpressions;

namespace DevHelperGUI
{
    public class PatternOptionsEventArgs : EventArgs
    {
        public PatternOptionsEventArgs(RegexOptions value)
        {
            PatternOptions = value;
        }

        public RegexOptions PatternOptions { get; }
    }
}
