using System.Text.RegularExpressions;

namespace DevHelperGUI
{
    public class GroupResult : CaptureResult
    {
        public string Name { get; }
        public GroupResult(Group group)
            : base(group)
        {
            Name = group.Name;
        }
    }
}
