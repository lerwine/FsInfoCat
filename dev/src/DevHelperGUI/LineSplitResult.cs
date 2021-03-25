using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DevHelperGUI
{
    public class LineSplitResult
    {
        public BindingList<LineResult> Lines { get; }

        public LineSplitResult(string[] lines)
        {
            Lines = new BindingList<LineResult>(lines.Select((s, i) => new LineResult(s, i + 1)).ToList());
        }
    }
}
