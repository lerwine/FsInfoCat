using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace DevHelperGUI
{
    public class GroupResult : CaptureItem
    {
        public string Name { get; }
        public GroupResult(Group group)
            : base(group)
        {
            Name = group.Name;
        }

        protected override XElement CreateElement() => new XElement("Group");

        protected override void SetElementContent(XElement element)
        {
            if (!string.IsNullOrEmpty(Name))
                element.SetAttributeValue(nameof(Name), Name);
            element.SetAttributeValue(nameof(Success), Success);
            if (Success)
            {
                element.SetAttributeValue(nameof(Index), Index);
                element.SetAttributeValue(nameof(Length), Length);
                element.Add(new XCData(RawValue));
            }
        }
    }
}
